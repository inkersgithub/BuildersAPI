using InkersCore.Models.AuthEntityModels;
using Microsoft.EntityFrameworkCore.Storage;

namespace InkersCore.Domain.IRepositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Function to get transaction Context
        /// </summary>
        /// <returns>IDbContextTransaction</returns>
        public IDbContextTransaction GetContextTransaction();

        /// <summary>
        /// Function to validate user credentials
        /// </summary>
        /// <param name="username">Email/Phone</param>
        /// <param name="password">Password</param>
        /// <returns>UserAccount</returns>
        UserAccount? ValidateUserCredentials(string username, string password);

        /// <summary>
        /// Function to fetch user based on user id
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>UserAccount</returns>
        UserAccount? GetUserAccountById(long userId);

        /// <summary>
        /// Function to remove user based on user id
        /// </summary>
        /// <param name="userId">UserId</param>
        void RemoveUser(int userId);

        /// <summary>
        /// Function to create new user account
        /// </summary>
        /// <param name="userAccount">UserAccount</param>
        /// <returns>UserAccount</returns>
        UserAccount CreateUserAccount(UserAccount userAccount);

        /// <summary>
        /// Function to update user account
        /// </summary>
        /// <param name="userAccount">UserAccount</param>
        void UpdateUserAccount(UserAccount userAccount);

        /// <summary>
        /// Get user group by id
        /// </summary>
        /// <param name="groupId">GroupdId</param>
        /// <returns>UserAccount</returns>
        UserGroup? GetUserGroupById(int groupId);

        /// <summary>
        /// Function to get user group mappings
        /// </summary>
        /// <param name="user">UserAccount</param>
        /// <returns>UserGroupMappingList</returns>
        List<UserGroupMapping> GetUserGroupMappings(UserAccount user);
    }
}
