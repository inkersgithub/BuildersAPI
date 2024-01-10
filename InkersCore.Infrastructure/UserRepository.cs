using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Infrastructure.Configurations;
using InkersCore.Models.AuthEntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InkersCore.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;
        private readonly ILoggerService<UserRepository> _loggerService;

        public UserRepository(AppDBContext context, ILoggerService<UserRepository> loggerService)
        {
            _context = context;
            _loggerService = loggerService;
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
        /// Function to fetch user based on user id
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>UserAccount</returns>
        public UserAccount? GetUserAccountById(int userId)
        {
            return _context.UserAccounts.FirstOrDefault(user => user.Id == userId);
        }

        /// <summary>
        /// Function to validate user credentials
        /// </summary>
        /// <param name="username">Email/Phone</param>
        /// <param name="password">Password</param>
        /// <returns>UserAccount</returns>
        public UserAccount? ValidateUserCredentials(string username, string password)
        {
            return _context.UserAccounts
                   .FirstOrDefault(user =>
                   (user.Phone == username || user.Email == username)
                   && user.Password == password && user.IsDeleted == false && user.IsActive == true);
        }


        /// <summary>
        /// Function to remove user based on user id
        /// </summary>
        /// <param name="userId">UserId</param>
        public void RemoveUser(int userId)
        {
            try
            {
                var userAccount = GetUserAccountById(userId);
                if (userAccount == null)
                    throw new Exception("User not found");
                userAccount.IsDeleted = true;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _loggerService.LogException(ex);
            }
        }

        /// <summary>
        /// Function to create new user account
        /// </summary>
        /// <param name="userAccount">UserAccount</param>
        /// <returns>UserAccount</returns>
        public UserAccount CreateUserAccount(UserAccount userAccount)
        {
            try
            {
                _context.UserAccounts.Add(userAccount);
                return userAccount;
            }
            catch (Exception ex)
            {
                _loggerService.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Function to update user account
        /// </summary>
        /// <param name="userAccount">UserAccount</param>
        public void UpdateUserAccount(UserAccount userAccount)
        {
            try
            {
                _context.UserAccounts.Update(userAccount);
            }
            catch (Exception ex)
            {
                _loggerService.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///Function to get user group by id
        /// </summary>
        /// <param name="groupId">GroupdId</param>
        /// <returns>UserAccount</returns>
        public UserGroup? GetUserGroupById(int groupId)
        {
            return _context.UserGroups.FirstOrDefault(group => group.Id == groupId);
        }

        /// <summary>
        /// Function to get user group mappings
        /// </summary>
        /// <param name="user">UserAccount</param>
        /// <returns>UserGroupMappingList</returns>
        public List<UserGroupMapping> GetUserGroupMappings(UserAccount user)
        {
            return _context.UserGroupMappings
                .Where(groupMapping => groupMapping.UserAccount == user &&
                groupMapping.IsDeleted == false && groupMapping.IsActive == true)
                .Include(x => x.UserGroup)
                .ToList();
        }
    }
}
