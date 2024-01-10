namespace InkersCore.Models.RequestModels
{
    public class LoginRequest : CommonRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
