using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.AuthEntityModels
{
    [Table("user_permission")]
    public class UserPermission : Common
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(50, ErrorMessage = "Name cannot be greater than 50 characters")]
        public string Name { get; set; }

        [Column("code")]
        [MaxLength(6, ErrorMessage = "Name cannot be greater than 6 characters")]
        public string Code { get; set; }
    }
}
