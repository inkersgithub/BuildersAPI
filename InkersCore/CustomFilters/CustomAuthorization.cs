using InkersCore.Common;
using InkersCore.Domain.IServices;
using InkersCore.Models.ResponseModels;
using InkersCore.Models.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace InkersCore.CustomFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
        private ITokenCacheService? _tokenCacheService;
        public string Permission { get; set; }

        /// <summary>
        /// Function to authorize user based on permission
        /// </summary>
        /// <param name="filterContext">AuthorizationFilterContext</param>
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext != null)
            {
                GetTokenCacheService(filterContext.HttpContext);
                StartValidationProcess(filterContext);
            }
        }

        /// <summary>
        /// Function to start user/permission validation
        /// </summary>
        /// <param name="filterContext">AuthorizationFilterContext</param>
        private void StartValidationProcess(AuthorizationFilterContext filterContext)
        {
            var userId = JsonWebTokenHandler.GetUserIdFromClaimPrincipal(filterContext.HttpContext.User);
            if (ValidateUserInCache(filterContext, userId, out var authData))
            {
                if (ValidateToken(filterContext, authData))
                {
                    ValidateUserPermission(filterContext, authData);
                }
                else
                {
                    SetUnAutorized(filterContext, "Invalid Token");
                }
            }
        }

        /// <summary>
        /// Function to start token validation
        /// </summary>
        /// <param name="filterContext">AuthorizationFilterContext</param>
        /// <param name="authData">UserAuthCacheData</param>
        /// <returns>TokenStatus</returns>
        private static bool ValidateToken(AuthorizationFilterContext filterContext, UserAuthCacheData? authData)
        {
            var token = filterContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null || authData?.TokenList.FirstOrDefault(x => x.Contains(token)) == null) return false;
            return JsonWebTokenHandler.ValidateToken(token);
        }

        /// <summary>
        /// Function to TokenService from request services
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <exception cref="Exception">Exception</exception>
        private void GetTokenCacheService(HttpContext httpContext)
        {
            _tokenCacheService = httpContext.RequestServices.GetService(typeof(ITokenCacheService)) as ITokenCacheService;
            if (_tokenCacheService == null) throw new Exception();
        }

        /// <summary>
        /// Function to validate user exist in cache
        /// </summary>
        /// <param name="filterContext"><AuthorizationFilterContext/param>
        /// <param name="userId">UserId</param>
        private bool ValidateUserInCache(AuthorizationFilterContext filterContext, int userId, out UserAuthCacheData? authData)
        {
            var existingAuthData = _tokenCacheService?.FetchValue(userId.ToString());
            if (existingAuthData != null)
            {
                authData = JsonConvert.DeserializeObject<UserAuthCacheData>(existingAuthData);
                return true;
            }
            else
            {
                SetUnAutorized(filterContext, "Invalid token");
                authData = null;
                return false;
            }
        }

        /// <summary>
        /// Function to validate user permission
        /// </summary>
        /// <param name="filterContext">AuthorizationFilterContext</param>
        /// <param name="authData">UserAuthCacheData</param>
        private void ValidateUserPermission(AuthorizationFilterContext filterContext, UserAuthCacheData? authData)
        {
            if (authData == null || (Permission != null && authData.RoleList.FirstOrDefault(x => x.Contains(Permission)) == null))
            {
                SetUnAutorized(filterContext, "You dont have enough permission to perform this operation");
            }
        }

        /// <summary>
        /// Function to set UnAuthorized message
        /// </summary>
        /// <param name="filterContext">FilterContext</param>
        /// <param name="errorMessage">ErrorMessage</param>
        private static void SetUnAutorized(AuthorizationFilterContext filterContext, string errorMessage)
        {
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            filterContext.Result = new JsonResult(new UnAuthorizedResponse() { Error = true, ErrorMessage = errorMessage });
        }
    }
}
