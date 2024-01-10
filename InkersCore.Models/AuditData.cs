using InkersCore.Models.AuthEntityModels;

namespace InkersCore.Models
{
    public class AuditData
    {
        public long RelatedId { get; set; }
        
        public UserAccount User { get; set; }

        public string Comment { get; set; }

        public AuditType Type { get; set; }

        public AuditFunction Function { get; set; }

        public AuditUpdationLog AuditUpdationLog { get; set; }
    }
}
