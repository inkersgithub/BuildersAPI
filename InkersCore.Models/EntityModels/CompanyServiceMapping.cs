using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.EntityModels
{
    [Table("company_service_mappings")]
    public class CompanyServiceMapping : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [ForeignKey("company_id")]
        public Company Company { get; set; }

        [ForeignKey("service_id")]
        public Service Service { get; set; }
    }
}
