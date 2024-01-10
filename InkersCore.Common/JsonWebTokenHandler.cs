using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InkersCore.Common
{
    public static class JsonWebTokenHandler
    {
        private static IConfiguration config;

        public static void AppSettingConfigure(IConfiguration Configuration)
        {
            config = Configuration;
        }

        /// <summary>
        /// Function to validate token
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public static bool ValidateToken(string authToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(authToken, GetValidationParameters(), out var validatedToken);
                if (validatedToken == null) throw new Exception();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Function to get validation parameter for verifying token
        /// </summary>
        /// <returns>TokenValidationParameters</returns>
        /// <exception cref="Exception">Exception</exception>
        private static TokenValidationParameters GetValidationParameters()
        {
            var securityKey = config["JWTSettings:SecurityKey"] ?? throw new Exception();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            return new TokenValidationParameters()
            {
                ValidateLifetime = Convert.ToBoolean(config["JWTSettings:ValidateLifetime"]),
                ValidateAudience = Convert.ToBoolean(config["JWTSettings:ValidateAudience"]),
                ValidateIssuer = Convert.ToBoolean(config["JWTSettings:ValidateIssuer"]),
                ValidIssuer = config["JWTSettings:Issuer"],
                ValidAudience = config["JWTSettings:Audience"],
                IssuerSigningKey = signingKey
            };
        }

        /// <summary>
        /// Function to get claim by decoding the token
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="claimName">ClaimName</param>
        /// <returns>Claim</returns>
        public static string? GetClaimValueFromToken(string token, string claimName)
        {
            try
            {
                var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return decodedToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Function to get claim from Claim Principal
        /// </summary>
        /// <param name="UserClaim">UserClaim</param>
        /// <param name="claimName">claimName</param>
        /// <returns>Claim</returns>
        public static string? GetClaimValueFromClaimPrincipal(ClaimsPrincipal UserClaim, string claimName)
        {
            try
            {
                return UserClaim.Claims.Where(c => c.Type == claimName)
                   .Select(c => c.Value).SingleOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Function to get user id from claim princial
        /// </summary>
        /// <param name="UserClaim">UserClaim</param>
        /// <returns>UserId</returns>
        public static int GetUserIdFromClaimPrincipal(ClaimsPrincipal UserClaim)
        {
            try
            {
                var userId = UserClaim.Claims.Where(c => c.Type == "UserId")
                   .Select(c => c.Value).SingleOrDefault();
                return Convert.ToInt32(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
