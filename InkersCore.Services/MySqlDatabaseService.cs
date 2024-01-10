using InkersCore.Domain.IServices;
using InkersCore.Models.ServiceModels;
using MySql.Data.MySqlClient;

namespace InkersCore.Services
{
    public class MySqlDatabaseService : IDatabaseService
    {
        private readonly ILoggerService<MySqlDatabaseService> _logger;

        public MySqlDatabaseService(ILoggerService<MySqlDatabaseService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Function to backup database
        /// </summary>
        /// <param name="databaseInformation">DatabaseServiceData</param>
        public void BackupDatabase(DatabaseServiceData databaseInformation)
        {
            try
            {
                BackupDatabaseToFile(databaseInformation.ConnectionString, databaseInformation.BackupFileLocation);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Function to restore database
        /// </summary>
        /// <param name="databaseInformation">DatabaseServiceData</param>
        public void RestoreDatabase(DatabaseServiceData databaseInformation)
        {
            try
            {
                RestoreDatabaseFromFile(databaseInformation.ConnectionString, databaseInformation.RestoreFileLocation);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Function to backup database to file
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="backupLocation">BackupLocation</param>
        private static void BackupDatabaseToFile(string connectionString, string backupLocation)
        {
            using MySqlConnection connection = new(connectionString);
            using MySqlCommand mysqlCommand = new();
            using MySqlBackup mysqlBackup = new(mysqlCommand);
            mysqlCommand.Connection = connection;
            connection.Open();
            mysqlBackup.ExportToFile(backupLocation);
            connection.Close();
        }

        /// <summary>
        /// Function to restore database from file
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="backupLocation">BackupLocation</param>
        private static void RestoreDatabaseFromFile(string connectionString, string backupLocation)
        {
            using MySqlConnection connection = new(connectionString);
            using MySqlCommand mysqlCommand = new();
            using MySqlBackup mysqlBackup = new(mysqlCommand);
            mysqlCommand.Connection = connection;
            connection.Open();
            mysqlBackup.ImportFromFile(backupLocation);
            connection.Close();
        }

    }
}
