namespace InkersCore.Models.ResponseModels
{
    public class CommonResponse
    {
        public bool Success { get; set; }
        public bool Error { get; set; }
        public bool Warning { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string WarningMessage { get; set; }
        public object Result { get; set; }

        public CommonResponse()
        {
            Success = false;
            Error = false;
            Warning = false;
        }
    }
}
