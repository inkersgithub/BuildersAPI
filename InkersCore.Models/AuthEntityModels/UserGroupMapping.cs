using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InkersCore.Models.EntityModels;

namespace InkersCore.Models.AuthEntityModels
{
    [Table("user_group_mapping")]
    public class UserGroupMapping : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [ForeignKey("user_id")]
        public UserAccount UserAccount { get; set; }

        [ForeignKey("user_group_id")]
        public UserGroup UserGroup { get; set; }
    }
}
