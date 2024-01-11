using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.EntityModels
{
    [Table("service")]
    public class Service : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(40, ErrorMessage = "Name cannot be greater than 40 characters")]
        public string Name { get; set; }
    }
}
