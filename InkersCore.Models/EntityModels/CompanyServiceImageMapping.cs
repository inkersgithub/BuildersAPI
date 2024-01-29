using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkersCore.Models.EntityModels
{
    [Table("company_service_image_mappings")]
    public class CompanyServiceImageMapping : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [ForeignKey("company_id")]
        public Company Company { get; set; }

        [ForeignKey("service_id")]
        public Service Service { get; set; }

        [Column("location")]
        public string Location { get; set; }
    }
}
