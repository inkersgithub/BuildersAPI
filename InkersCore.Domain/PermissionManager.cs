using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Models.AuthEntityModels;
using InkersCore.Models.RequestModels;
using InkersCore.Models.ResponseModels;
using System.Security.Claims;

namespace InkersCore.Domain
{
    public class PermissionManager
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userAccountRepository;
        private readonly ILoggerService<PermissionManager> _loggerService;
        private UserAccount _requestUser;
        private UserAccount _updatedBy;
        private UserGroup _userGroup;

        public PermissionManager(IPermissionRepository permissionRepository, IUserRepository userAccountRepository, ILoggerService<PermissionManager> loggerService)
        {
            _permissionRepository = permissionRepository;
            _userAccountRepository = userAccountRepository;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Function to modify user permissions
        /// </summary>
        /// <param name="modifyPermissionRequest">UserPermissionModifyRequest</param>
        /// <returns>PermissionModifyResponse</returns>
        public PermissionModifyResponse ModifyUserPermission(UserPermissionModifyRequest modifyPermissionRequest)
        {
            var transaction = _permissionRepository.GetContextTransaction();
            try
            {
                ValidateAndAssingUser(modifyPermissionRequest);
                var permissionList = _permissionRepository.GetUserPermissionMappings(_requestUser);
                permissionList.AddRange(ModifyUserPermissionMappings(modifyPermissionRequest, permissionList));
                _permissionRepository.ModifyUserPermission(permissionList);
                transaction.Commit();
                return new PermissionModifyResponse() { Success = true, SuccessMesssage = "Permission modified" };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _loggerService.LogException(ex);
                return new PermissionModifyResponse() { Error = false, ErrorMessage = "Some error occured" };
            }
        }

        /// <summary>
        /// Function to validate and assign user
        /// </summary>
        /// <param name="modifyPermissionRequest">UserPermissionModifyRequest</param>
        /// <exception cref="Exception">Exception</exception>
        private void ValidateAndAssingUser(UserPermissionModifyRequest modifyPermissionRequest)
        {
            var requestUser = _userAccountRepository.GetUserAccountById(modifyPermissionRequest.UserId);
            var updatedBy = _userAccountRepository.GetUserAccountById(modifyPermissionRequest.UpdaedByUserId);
            _requestUser = requestUser ?? throw new Exception("User not found");
            _updatedBy = updatedBy ?? throw new Exception("User not found");
        }

        /// <summary>
        /// Function to modify user permission mappings
        /// </summary>
        /// <param name="modifyPermissionRequest">ModifyPermissionRequest</param>
        /// <param name="permissionList">PermissionList</param>
        /// <returns>UserPermissionMappingList</returns>
        private List<UserPermissionMapping> ModifyUserPermissionMappings(UserPermissionModifyRequest modifyPermissionRequest, List<UserPermissionMapping> permissionList)
        {
            var userPermissionMappings = new List<UserPermissionMapping>();
            foreach (var requestPermission in modifyPermissionRequest.PermissionList)
            {
                ModifyPermissionForSingleMapping(permissionList, userPermissionMappings, requestPermission);
            }
            return userPermissionMappings;
        }

        /// <summary>
        /// Function to modify permission for single mapping
        /// </summary>
        /// <param name="permissionList">PermissionList</param>
        /// <param name="userPermissionMappings">UserPermissionMappings</param>
        /// <param name="requestPermission">RequestPermission</param>
        private void ModifyPermissionForSingleMapping(List<UserPermissionMapping> permissionList, List<UserPermissionMapping> userPermissionMappings, PermissionRequest requestPermission)
        {
            var permission = permissionList.FirstOrDefault(permission => permission.Permission.Id == requestPermission.PermissionId);
            if (permission != null)
            {
                ModifyExistingUserPermission(permission, requestPermission);
            }
            else
            {
                AssingNewUserPermission(userPermissionMappings, requestPermission);
            }
        }

        /// <summary>
        /// Function to modify existing user permission mappings
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <param name="requestPermission">RequesPermission</param>
        private static void ModifyExistingUserPermission(UserPermissionMapping permission, PermissionRequest requestPermission)
        {
            permission.Add = requestPermission.Add;
            permission.Update = requestPermission.Update;
            permission.View = requestPermission.View;
            permission.Approve = requestPermission.Approve;
            permission.Remove = requestPermission.Remove;
        }

        /// <summary>
        /// Function to assing new user permission mapping
        /// </summary>
        /// <param name="userPermissionMappings">UserPermissionMapping</param>
        /// <param name="requestPermission">PermissionRequest</param>
        private void AssingNewUserPermission(List<UserPermissionMapping> userPermissionMappings, PermissionRequest requestPermission)
        {
            userPermissionMappings.Add(new UserPermissionMapping()
            {
                Permission = _permissionRepository.GetPermissionById(requestPermission.PermissionId),
                User = _requestUser,
                Add = requestPermission.Add,
                View = requestPermission.View,
                Approve = requestPermission.Approve,
                Remove = requestPermission.Remove,
                Update = requestPermission.Update,
                CreatedBy = _updatedBy,
                LastUpdatedBy = _updatedBy
            });
        }

        /// <summary>
        /// Function to modify group permission
        /// </summary>
        /// <param name="groupPermissionRequest">GroupPermissionRequest</param>
        /// <returns>PermissionModifyResponse</returns>
        public PermissionModifyResponse ModifyGroupPermission(GroupPermissionModifyRequest groupPermissionRequest)
        {
            var transaction = _permissionRepository.GetContextTransaction();
            try
            {
                ValidateAndAssingUserAndGroup(groupPermissionRequest);
                var permissionList = _permissionRepository.GetGroupPermissionMappings(_userGroup);
                permissionList.AddRange(ModifyUserGroupPermissionMappings(groupPermissionRequest, permissionList));
                _permissionRepository.ModifyGroupPermission(permissionList);
                transaction.Commit();
                return new PermissionModifyResponse() { Success = true, SuccessMesssage = "Permission modified" };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _loggerService.LogException(ex);
                return new PermissionModifyResponse() { Error = false, ErrorMessage = "Some error occured" };
            }
        }

        /// <summary>
        /// Function to modify group permission mappings
        /// </summary>
        /// <param name="modifyPermissionRequest">GroupPermissionModifyRequest</param>
        /// <param name="permissionList">UserGroupPermissionMappingList</param>
        /// <returns>UserGroupPermissionMappingList</returns>
        private List<UserGroupPermissionMapping> ModifyUserGroupPermissionMappings(GroupPermissionModifyRequest modifyPermissionRequest, List<UserGroupPermissionMapping> permissionList)
        {
            var groupPermissionMappings = new List<UserGroupPermissionMapping>();
            foreach (var requestPermission in modifyPermissionRequest.PermissionList)
            {
                ModifyGroupPermissionForSingleMapping(permissionList, groupPermissionMappings, requestPermission);
            }
            return groupPermissionMappings;
        }

        /// <summary>
        /// Function to modify permission for single mapping
        /// </summary>
        /// <param name="permissionList">PermissionList</param>
        /// <param name="userGroupPermissionMappings">UserGroupPermissionMappings</param>
        /// <param name="requestPermission">RequestPermission</param>
        private void ModifyGroupPermissionForSingleMapping(List<UserGroupPermissionMapping> permissionList, List<UserGroupPermissionMapping> userGroupPermissionMappings, PermissionRequest requestPermission)
        {
            var permission = permissionList.FirstOrDefault(permission => permission.Permission.Id == requestPermission.PermissionId);
            if (permission != null)
            {
                ModifyExistingUserGroupPermission(permission, requestPermission);
            }
            else
            {
                AssingNewUserGroupPermission(userGroupPermissionMappings, requestPermission);
            }
        }

        /// <summary>
        /// Function to modify permission mappings of exising group
        /// </summary>
        /// <param name="permission">UserGroupPermissionMapping</param>
        /// <param name="requestPermission">PermissionRequest</param>
        private static void ModifyExistingUserGroupPermission(UserGroupPermissionMapping permission, PermissionRequest requestPermission)
        {
            permission.Add = requestPermission.Add;
            permission.Update = requestPermission.Update;
            permission.View = requestPermission.View;
            permission.Approve = requestPermission.Approve;
            permission.Remove = requestPermission.Remove;
        }

        /// <summary>
        /// Function to assign and create new group permission mappings
        /// </summary>
        /// <param name="userGroupPermissionMappings">UserGroupPermissionMappingList</param>
        /// <param name="requestPermission">PermissionRequest</param>
        private void AssingNewUserGroupPermission(List<UserGroupPermissionMapping> userGroupPermissionMappings, PermissionRequest requestPermission)
        {
            userGroupPermissionMappings.Add(new UserGroupPermissionMapping()
            {
                Permission = _permissionRepository.GetPermissionById(requestPermission.PermissionId),
                UserGroup = _userGroup,
                Add = requestPermission.Add,
                View = requestPermission.View,
                Approve = requestPermission.Approve,
                Remove = requestPermission.Remove,
                Update = requestPermission.Update,
                CreatedBy = _updatedBy,
                LastUpdatedBy = _updatedBy
            });
        }

        /// <summary>
        /// Function to validate and assign user and group
        /// </summary>
        /// <param name="groupPermissionRequest"></param>
        /// <exception cref="Exception"></exception>
        private void ValidateAndAssingUserAndGroup(GroupPermissionModifyRequest groupPermissionRequest)
        {
            var userGroup = _userAccountRepository.GetUserGroupById(groupPermissionRequest.GroupId);
            var updatedBy = _userAccountRepository.GetUserAccountById(groupPermissionRequest.UpdaedByUserId);
            _userGroup = userGroup ?? throw new Exception("Group not found");
            _updatedBy = updatedBy ?? throw new Exception("User not found");
        }

        /// <summary>
        /// Function to get user role list
        /// </summary>
        /// <param name="user">UserAccount</param>
        /// <returns>ClaimList</returns>
        public List<string> GetUserRoleList(UserAccount user)
        {
            var roleList = GetUserPermissionMappingRoles(user).Distinct().ToList();
            return roleList;
        }

        /// <summary>
        /// Function to get user permission mapping roles
        /// </summary>
        /// <param name="user">UserAccount</param>
        /// <returns>RoleList</returns>
        private List<string> GetUserPermissionMappingRoles(UserAccount user)
        {
            var roleList = new List<string>();
            GetUserPermissionMappingRoles(user, roleList);
            GetGroupPermissionMappingRoles(user, roleList);
            return roleList;
        }

        /// <summary>
        /// Function to get user permission mapping roles
        /// </summary>
        /// <param name="user">UserAccount</param>
        /// <param name="roleList">StringList</param>
        private void GetUserPermissionMappingRoles(UserAccount user, List<string> roleList)
        {
            var userPermissions = _permissionRepository.GetUserPermissionMappings(user);
            foreach (var permission in userPermissions)
            {
                GetRoleBasedOnStatus(permission, roleList);
            }
        }

        /// <summary>
        /// Function to get group permission mapping roles
        /// </summary>
        /// <param name="user">UserAccount</param>
        /// <param name="roleList">StringList</param>
        private void GetGroupPermissionMappingRoles(UserAccount user, List<string> roleList)
        {
            var userGroupMappingList = _userAccountRepository.GetUserGroupMappings(user);
            foreach (var userGroup in userGroupMappingList)
            {
                GetPermissionMappingRoles(userGroup.UserGroup, roleList);
            }
        }

        /// <summary>
        /// Function to get permission mapping roles
        /// </summary>
        /// <param name="userGroup">UserGroup</param>
        /// <param name="roleList">StringList</param>
        private void GetPermissionMappingRoles(UserGroup userGroup, List<string> roleList)
        {
            var mappings = _permissionRepository.GetGroupPermissionMappings(userGroup);
            foreach (var mapping in mappings)
            {
                GetRoleBasedOnStatus(mapping, roleList);
            }
        }

        /// <summary>
        /// Function to get roles based on status
        /// </summary>
        /// <param name="permission">UserGroupPermissionMapping</param>
        /// <param name="roleList">StringList</param>
        private static void GetRoleBasedOnStatus(UserGroupPermissionMapping permission, List<string> roleList)
        {
            if (permission.Add)
                roleList.Add(permission.Permission.Code + 'C');
            if (permission.View)
                roleList.Add(permission.Permission.Code + 'R');
            if (permission.Update)
                roleList.Add(permission.Permission.Code + 'U');
            if (permission.Remove)
                roleList.Add(permission.Permission.Code + 'D');
            if (permission.Approve)
                roleList.Add(permission.Permission.Code + 'A');
        }

        /// <summary>
        /// Function to get roles based on status
        /// </summary>
        /// <param name="permission">UserPermissionMapping</param>
        /// <param name="roleList">StringList</param>
        private static void GetRoleBasedOnStatus(UserPermissionMapping permission, List<string> roleList)
        {
            if (permission.Add)
                roleList.Add(permission.Permission.Code + 'A');
            if (permission.View)
                roleList.Add(permission.Permission.Code + 'V');
            if (permission.Update)
                roleList.Add(permission.Permission.Code + 'U');
            if (permission.Remove)
                roleList.Add(permission.Permission.Code + 'R');
            if (permission.Approve)
                roleList.Add(permission.Permission.Code + 'P');
        }
    }
}
