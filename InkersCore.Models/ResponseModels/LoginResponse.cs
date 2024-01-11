using InkersCore.Models.AuthEntityModels;

namespace InkersCore.Models.ResponseModels
{
    public class LoginResponse: CommonResponse
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public List<string> RoleList { get; set; }
        public List<UserGroupMapping> UserGroups { get; set; }
    }
}
