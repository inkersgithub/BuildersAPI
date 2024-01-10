using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.AuthEntityModels
{
    [Table("audit_updation_log")]
    public class AuditUpdationLog:Common
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("audit_log_id")]
        public AuditLog AuditLog { get; set; }

        [Column("existing_data")]
        public string ExisintgData { get; set; }

        [Column("updated_data")]
        public string UpdatedData { get; set; }
    }
}
