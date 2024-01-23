using InkersCore.Common;
using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Models.AuthEntityModels;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using InkersCore.Models.ResponseModels;
using InkersCore.Models.ServiceModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace InkersCore.Domain
{
    public class AuthManager
    {
        private readonly IUserRepository _userAccountRepository;
        private readonly ITokenCacheService _tokenCacheService;
        private readonly PermissionManager _permissionManager;
        private readonly ILoggerService<AuthManager> _loggerService;
        private readonly IConfiguration _config;
        private readonly IGenericRepository<Company> _companyGenericRepository;

        public AuthManager(IUserRepository userAccountRepository, ITokenCacheService tokenCacheService,
            ILoggerService<AuthManager> loggerService,
            PermissionManager permissionManager, IConfiguration config, IGenericRepository<Company> companyGenericRepository)
        {
            _userAccountRepository = userAccountRepository;
            _tokenCacheService = tokenCacheService;
            _loggerService = loggerService;
            _permissionManager = permissionManager;
            _config = config;
            _companyGenericRepository = companyGenericRepository;
        }

        /// <summary>
        /// Function to start logout process
        /// </summary>
        /// <param name="userId">UserId</param>
        public LogoutResponse StartLogoutProcess(string? userId, string? token, bool logoutAll)
        {
            try
            {
                if (userId == null) throw new Exception();
                RemoveAuthDataFromCache(userId, token, logoutAll);
                return new LogoutResponse() { Success = true, SuccessMessage = "Successfully logged out" };
            }
            catch (Exception ex)
            {
                _loggerService.LogException(ex);
                return new LogoutResponse() { Success = false, Error = true, ErrorMessage = "Some error occured" };
            }
        }

        /// <summary>
        /// Function to remove auth data from cache
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="token">Token</param>
        /// <param name="logoutAll">LogoutAll</param>
        private void RemoveAuthDataFromCache(string userId, string? token, bool logoutAll)
        {
            if (logoutAll)
            {
                _tokenCacheService.Delete(userId);
            }
            else
            {
                RemoveSpecificTokenFromCache(userId, token);
            }
        }

        /// <summary>
        /// Function to remove specific token from cache
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="token">Token</param>
        /// <exception cref="Exception">Exception</exception>
        private void RemoveSpecificTokenFromCache(string userId, string? token)
        {
            var existinAuthData = _tokenCacheService.FetchValue(userId);
            if (existinAuthData == null || token == null) throw new Exception();
            var authData = JsonConvert.DeserializeObject<UserAuthCacheData>(existinAuthData);
            authData?.TokenList.Remove(token);
            _tokenCacheService.Insert(userId, JsonConvert.SerializeObject(authData));
        }

        /// <summary>
        /// Function to start login process
        /// </summary>
        /// <param name="loginReqest">LoginRequest</param>
        /// <returns>LoginResponse</returns>
        public LoginResponse StartLoginProcess(LoginRequest loginReqest)
        {
            try
            {
                var user = ValidateUserCredentials(loginReqest);
                var response = user != null ? GenerateSuccessLoginResponse(user) : GenerateFailureLoginResponse("Invalid credentials");
                return response;
            }
            catch (Exception ex)
            {
                _loggerService.LogException(ex);
                return new LoginResponse() { Success = false, Error = true, ErrorMessage = "Some error occured" };
            }

        }

        /// <summary>
        /// Function to validate user credentials
        /// </summary>
        /// <param name="loginReqest">LoginRequest</param>
        /// <returns>UserAccount</returns>
        private UserAccount? ValidateUserCredentials(LoginRequest loginReqest)
        {
            return _userAccountRepository.ValidateUserCredentials(loginReqest.UserName, loginReqest.Password);
        }

        /// <summary>
        /// Function to generate response for successfull login
        /// </summary>
        /// <param name="userAccount">UserAccount</param>
        /// <returns>LoginResponse</returns>
        private LoginResponse GenerateSuccessLoginResponse(UserAccount userAccount)
        {
            var token = GenerateToken(userAccount);
            InsertAuthDetailsToCache(token, userAccount, out List<string> roleList);
            var userGroups = _userAccountRepository.GetUserGroupMappings(userAccount);
            var loginResponse = new LoginResponse()
            {
                Success = true,
                SuccessMessage = "Login Success",
                Name = userAccount.Name,
                Token = token,
                RoleList = roleList,
                UserGroups = userGroups,
            };
            if (userGroups[0].UserGroup.Name != "Admin")
            {
                loginResponse.Company = (_companyGenericRepository.Find(new Models.EntityFilter<Company>()
                {
                    Predicate = x => x.UserAccount == userAccount && x.IsActive && !x.IsDeleted
                }).FirstOrDefault() ?? throw new Exception("Company not found"));
            }
            return loginResponse;
        }

        /// <summary>
        /// Function to generate response fo failure login
        /// </summary>
        /// <param name="errorMessage">ErrorMessage</param>
        /// <returns>Login Response</returns>
        private static LoginResponse GenerateFailureLoginResponse(string errorMessage)
        {
            return new LoginResponse()
            {
                Success = false,
                Error = true,
                ErrorMessage = errorMessage
            };
        }

        /// <summary>
        /// Function to generate JWT token
        /// </summary>
        /// <param name="userAccount">UserAccount</param>
        /// <returns>JsonWebToken</returns>
        private string GenerateToken(UserAccount userAccount)
        {
            var securityKey = _config["JWTSettings:SecurityKey"] ?? throw new Exception();
            var signinCredential = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256);
            var tokeOptions = GetTokenOptions(signinCredential, userAccount);
            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        /// <summary>
        /// Function to get token options
        /// </summary>
        /// <param name="signinCredential">SigningCredentials</param>
        /// <param name="userAccount">UserAccount</param>
        /// <returns>JwtSecurityToken</returns>
        private JwtSecurityToken GetTokenOptions(SigningCredentials signinCredential, UserAccount userAccount)
        {
            return new JwtSecurityToken(
                claims: FetchClaimsToGenerateToken(userAccount),
                issuer: _config["JWTSettings:Issuer"],
                audience: _config["JWTSettings:Audience"],
                expires: DateTimeHandler.GetDateTime().AddMinutes(Convert.ToInt32(_config["JWTSettings:ExpireMinutes"])),
                signingCredentials: signinCredential
            );
        }

        /// <summary>
        /// Function to create claim list to generate token
        /// </summary>
        /// <param name="user">UserAccount</param>
        /// <returns>ClaimsList</returns>
        private static List<Claim> FetchClaimsToGenerateToken(UserAccount user)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Name.ToString()),
                new Claim("UserId",user.Id.ToString()),
                new Claim("Email", user.Email.ToString()),
                new Claim("Phone", user.Phone.ToString()),
            };
        }

        /// <summary>
        /// Function to insert auth details to cache
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="user">UserAccount</param>
        private void InsertAuthDetailsToCache(string token, UserAccount user, out List<string> roleList)
        {
            roleList = _permissionManager.GetUserRoleList(user);
            var authData = _tokenCacheService.FetchValue(user.Id.ToString());
            if (authData == null)
            {
                InsertAuthData(token, roleList, user.Id.ToString());
            }
            else
            {
                UpdateAuthData(authData, token, roleList);
            }
        }

        /// <summary>
        /// Function to insert new auth data to cache
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="roleList">RoleList</param>
        /// <param name="userId">UserId</param>
        private void InsertAuthData(string token, List<string> roleList, string userId)
        {
            var authData = new UserAuthCacheData()
            {
                UserId = userId,
                TokenList = new List<string>() { token },
                RoleList = roleList
            };
            _tokenCacheService.Insert(userId, JsonConvert.SerializeObject(authData));
        }

        /// <summary>
        /// Function to update existing auth data in cache
        /// </summary>
        /// <param name="existingAuthData">AuthData</param>
        /// <param name="token">Token</param>
        private void UpdateAuthData(string existingAuthData, string token, List<string> roleList)
        {
            var authData = JsonConvert.DeserializeObject<UserAuthCacheData>(existingAuthData);
            if (authData == null) throw new Exception();
            authData.RoleList = roleList;
            authData.TokenList.Add(token);
            _tokenCacheService.Insert(authData.UserId, JsonConvert.SerializeObject(authData));
        }
    }
}
