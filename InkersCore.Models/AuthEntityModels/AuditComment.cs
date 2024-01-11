using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InkersCore.Models.EntityModels;

namespace InkersCore.Models.AuthEntityModels
{
    [Table("audit_comment")]
    public class AuditComment : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("audit_log_id")]
        public AuditLog AuditLog { get; set; }

        [Column("comment")]
        [MaxLength(250, ErrorMessage = "Name cannot be greater than 250 characters")]
        public string Comment { get; set; }
    }
}
