using InkersCore.Models.AuthEntityModels;
using InkersCore.Models.EntityModels;

namespace InkersCore.Models.ResponseModels
{
    public class LoginResponse: CommonResponse
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public List<string> RoleList { get; set; }
        public List<UserGroupMapping> UserGroups { get; set; }
        public Company Company { get; set; }
    }
}
