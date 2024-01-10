namespace InkersCore.Models.RequestModels
{
    public class UserPermissionModifyRequest
    {
        public int UserId { get; set; }

        public int UpdaedByUserId { get; set; }

        public List<PermissionRequest> PermissionList { get; set; }
    }
}
