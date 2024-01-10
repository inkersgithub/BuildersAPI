using InkersCore.Models;

namespace InkersCore.Domain.IRepositories
{
    public interface IAuditRepository
    {
        /// <summary>
        /// Function to add audit log
        /// </summary>
        /// <param name="auditData">AuditData</param>
        void AddAuditLog(AuditData auditData);
    }
}
