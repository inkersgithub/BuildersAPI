using InkersCore.Common;
using InkersCore.Domain.IServices;

namespace InkersCore.Middlewares
{
    public class TokenInterceptorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenCacheService _tokenCacheService;

        public TokenInterceptorMiddleware(RequestDelegate next, ITokenCacheService tokenCacheService)
        {
            _next = next;
            _tokenCacheService = tokenCacheService;
        }

        public Task Invoke(HttpContext httpContext)
        {
            InterceptToken(httpContext);
            return _next(httpContext);
        }

        /// <summary>
        /// Function to intercept and validate token
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        private void InterceptToken(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                ValidateAndRemove(httpContext, token);
            }
        }

        /// <summary>
        /// Function to validate and remove token from authorization header
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <param name="token">Token</param>
        private void ValidateAndRemove(HttpContext httpContext, string token)
        {
            var userId = GetUserIdFromToken(token);
            if (userId == null || _tokenCacheService.FetchValue(userId) == null)
            {
                httpContext.Request.Headers.Remove("Authorization");
            }
        }

        /// <summary>
        /// Function to get UserId by decoding the token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>UserId</returns>
        private static string? GetUserIdFromToken(string token)
        {
            try
            {
                return JsonWebTokenHandler.GetClaimValueFromToken(token,"UserId");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Extension method used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class TokenInterceptorMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenInterceptorMiddleware>();
        }
    }
}
