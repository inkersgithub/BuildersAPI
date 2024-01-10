using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Infrastructure.Configurations;
using InkersCore.Models.AuthEntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InkersCore.Infrastructure
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDBContext _context;
        private readonly ILoggerService<UserRepository> _loggerService;

        public PermissionRepository(AppDBContext context, ILoggerService<UserRepository> loggerService)
        {
            _loggerService = loggerService;
            _context = context;
        }

        /// <summary>
        /// Function to get transaction Context
        /// </summary>
        /// <returns>IDbContextTransaction</returns>
        public IDbContextTransaction GetContextTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        /// <summary>
        /// Function to fetch all the permissions based on user
        /// </summary>
        /// <param name="userAccount">UserAccount</param>
        /// <returns>UserPermissionMappingList</returns>
        public List<UserPermissionMapping> GetUserPermissionMappings(UserAccount userAccount)
        {
            return _context.UserPermissionMappings
                .Where(permission => permission.User == userAccount)
                .Include(permission => permission.Permission)
                .ToList();
        }

        /// <summary>
        /// Function to fetch permission based on id
        /// </summary>
        /// <param name="permissionId">PermissionId</param>
        /// <returns>Permission</returns>
        public UserPermission GetPermissionById(int permissionId)
        {
            return _context.UserPermissions
                .First(permission => permission.Id == permissionId && permission.IsDeleted == false
                       && permission.IsActive == true);
        }

        /// <summary>
        /// Function to modify group permission
        /// </summary>
        /// <param name="groupPermissionMapping">UserGroupPermissionMapping</param>
        public void ModifyGroupPermission(List<UserGroupPermissionMapping> groupPermissionMapping)
        {
            try
            {
                _context.UserGroupPermissionMappings.UpdateRange(groupPermissionMapping);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _loggerService.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Function to modify user permission
        /// </summary>
        /// <param name="userPermissionMapping">UserPermissionMapping</param>
        public void ModifyUserPermission(List<UserPermissionMapping> userPermissionMapping)
        {
            try
            {
                _context.UserPermissionMappings.UpdateRange(userPermissionMapping);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _loggerService.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Function to fetch all the permissions based on group
        /// </summary>
        /// <param name="userGroup">UserGroup</param>
        /// <returns>UserGroupPermissionMappingList</returns>
        public List<UserGroupPermissionMapping> GetGroupPermissionMappings(UserGroup userGroup)
        {
            return _context.UserGroupPermissionMappings
                .Where(permission => permission.UserGroup == userGroup)
                .Include(permission => permission.Permission)
                .ToList();
        }
    }
}
