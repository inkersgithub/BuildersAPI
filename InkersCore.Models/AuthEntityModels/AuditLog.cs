using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.AuthEntityModels
{
    [Table("audit_log")]
    public class AuditLog : Common
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("related_id")]
        public long RelatedId { get; set; }

        [Column("type")]
        public AuditType Type { get; set; }

        [Column("function")]
        public AuditFunction Function { get; set; }
    }

    public enum AuditType
    {
        Inserted = 1,
        Updated = 2,
        Deleted = 3
    }

    public enum AuditFunction
    {
        User = 1
    }
}
