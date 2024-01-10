using InkersCore.Common;
using InkersCore.CustomFilters;
using InkersCore.Domain;
using InkersCore.Domain.IRepositories;
using InkersCore.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InkersCore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly AuthManager _authManager;

        public AuthController(AuthManager authManager, IUserRepository _rep)
        {
            _authManager = authManager;
        }

        /// <summary>
        /// Function to login the user
        /// </summary>
        /// <param name="loginRequest">LoginRequest</param>
        /// <returns>LoginResponse</returns>
        [HttpPost]
        public IActionResult Login([FromForm] LoginRequest loginRequest)
        {
            var result = _authManager.StartLoginProcess(loginRequest);
            return Ok(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// Function to logout the user
        /// </summary>
        /// <returns>LogoutResponse</returns>
        [HttpPost]
        [CustomAuthorization]
        public IActionResult Logout()
        {
            var userId = JsonWebTokenHandler.GetClaimValueFromClaimPrincipal(User, "UserId");
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var response = _authManager.StartLogoutProcess(userId, token, false);
            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
