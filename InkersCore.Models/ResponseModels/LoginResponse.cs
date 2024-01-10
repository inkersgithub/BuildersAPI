namespace InkersCore.Models.ResponseModels
{
    public class LoginResponse: CommonResponse
    {
        public string Name { get; set; }

        public string Token { get; set; }
    }
}
