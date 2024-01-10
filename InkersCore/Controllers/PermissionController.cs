using InkersCore.Common;
using InkersCore.Domain;
using InkersCore.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InkersCore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PermissionController : Controller
    {
        private readonly PermissionManager _permissionManager;

        public PermissionController(PermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// Function to modify user permission
        /// </summary>
        /// <param name="modifyPermissionRequest">UserPermissionModifyRequest</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult ModifyUserPermission(UserPermissionModifyRequest modifyPermissionRequest)
        {
            modifyPermissionRequest.UpdaedByUserId = JsonWebTokenHandler.GetUserIdFromClaimPrincipal(User);
            var response = _permissionManager.ModifyUserPermission(modifyPermissionRequest);
            return Ok(JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// Function to modify group permission
        /// </summary>
        /// <param name="modifyPermissionRequest">GroupPermissionModifyRequest</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult ModifyGroupPermission(GroupPermissionModifyRequest modifyPermissionRequest)
        {
            modifyPermissionRequest.UpdaedByUserId = JsonWebTokenHandler.GetUserIdFromClaimPrincipal(User);
            var response = _permissionManager.ModifyGroupPermission(modifyPermissionRequest);
            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
