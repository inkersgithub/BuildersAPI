namespace InkersCore.Models.RequestModels
{
    public class PermissionRequest
    {
        public int PermissionId { get; set; }

        public bool Add { get; set; }

        public bool View { get; set; }

        public bool Update { get; set; }

        public bool Remove { get; set; }

        public bool Approve { get; set; }
    }
}
