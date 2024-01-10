using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.AuthEntityModels
{
    [Table("user_group")]
    public class UserGroup : Common
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(20, ErrorMessage = "Name cannot be greater than 20 characters")]
        public string Name { get; set; }
    }
}
