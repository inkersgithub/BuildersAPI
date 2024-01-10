using InkersCore.Models.ServiceModels;

namespace InkersCore.Domain.IServices
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Function to backup database
        /// </summary>
        /// <param name="databaseInformation">DatabaseServiceData</param>
        public void BackupDatabase(DatabaseServiceData databaseInformation);

        /// <summary>
        /// Function to restore database
        /// </summary>
        /// <param name="databaseInformation">DatabaseServiceData</param>
        public void RestoreDatabase(DatabaseServiceData databseInformation);
    }
}
