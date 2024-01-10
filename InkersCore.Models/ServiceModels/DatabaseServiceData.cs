namespace InkersCore.Models.ServiceModels
{
    public class DatabaseServiceData
    {
        public string ConnectionString { get; set; }
        public string BackupFileLocation { get; set; }
        public string RestoreFileLocation { get; set; }
    }
}
