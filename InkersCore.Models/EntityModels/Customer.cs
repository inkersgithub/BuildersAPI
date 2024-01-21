using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.EntityModels
{
    [Table("customer")]
    public class Customer: CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(80, ErrorMessage = "Name cannot be greater than 80 characters")]
        public string Name { get; set; }

        [Column("phone")]
        [MaxLength(14, ErrorMessage = "Phone cannot be greater than 14 characters")]
        public string Phone { get; set; }

        [Column("email")]
        [MaxLength(120, ErrorMessage = "Email cannot be greater than 120 characters")]
        public string Email { get; set; }
    }
}
