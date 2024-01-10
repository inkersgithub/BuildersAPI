using InkersCore.Models.AuthEntityModels;
using Microsoft.EntityFrameworkCore.Storage;

namespace InkersCore.Domain.IRepositories
{
    public interface IPermissionRepository
    {
        /// <summary>
        /// Function to get transaction Context
        /// </summary>
        /// <returns>IDbContextTransaction</returns>
        public IDbContextTransaction GetContextTransaction();

        /// <summary>
        /// Function to fetch all the permissions based on user
        /// </summary>
        /// <param name="userAccount">UserAccount</param>
        /// <returns>UserPermissionMappingList</returns>
        List<UserPermissionMapping> GetUserPermissionMappings(UserAccount userAccount);

        /// <summary>
        /// Function to fetch permission based on id
        /// </summary>
        /// <param name="permissionId">PermissionId</param>
        /// <returns>UserPermission</returns>
        UserPermission GetPermissionById(int permissionId);

        /// <summary>
        /// Function to modify user permission
        /// </summary>
        /// <param name="userPermissionMapping">UserPermissionMapping</param>
        void ModifyUserPermission(List<UserPermissionMapping> userPermissionMapping);

        /// <summary>
        /// Function to modify group permission
        /// </summary>
        /// <param name="groupPermissionMapping">UserGroupPermissionMapping</param>
        void ModifyGroupPermission(List<UserGroupPermissionMapping> groupPermissionMapping);

        /// <summary>
        /// Function to fetch all the permissions based on group
        /// </summary>
        /// <param name="userGroup">UserGroup</param>
        /// <returns>UserGroupPermissionMappingList</returns>
        List<UserGroupPermissionMapping> GetGroupPermissionMappings(UserGroup userGroup);
    }
}
