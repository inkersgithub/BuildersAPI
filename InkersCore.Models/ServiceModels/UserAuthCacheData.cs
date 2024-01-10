namespace InkersCore.Models.ServiceModels
{
    public class UserAuthCacheData
    {
        public string UserId { get; set; }
        public List<string> TokenList { get; set; }
        public List<string> RoleList { get; set; }
    }
}
