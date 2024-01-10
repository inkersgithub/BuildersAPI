using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.AuthEntityModels
{

    [Table("user_group_permission_mapping")]
    public class UserGroupPermissionMapping : Common
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [ForeignKey("user_group_id")]
        public UserGroup UserGroup { get; set; }

        [ForeignKey("permission_id")]
        public UserPermission Permission { get; set; }

        [Column("add")]
        public bool Add { get; set; }

        [Column("view")]
        public bool View { get; set; }

        [Column("update")]
        public bool Update { get; set; }

        [Column("remove")]
        public bool Remove { get; set; }

        [Column("approve")]
        public bool Approve { get; set; }
    }
}
