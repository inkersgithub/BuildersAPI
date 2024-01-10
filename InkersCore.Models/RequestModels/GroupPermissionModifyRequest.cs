namespace InkersCore.Models.RequestModels
{
    public class GroupPermissionModifyRequest
    {
        public int GroupId { get; set; }

        public int UpdaedByUserId { get; set; }

        public List<PermissionRequest> PermissionList { get; set; }
    }
}
