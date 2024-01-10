using InkersCore.Common;
using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Infrastructure.Configurations;
using InkersCore.Models;
using InkersCore.Models.AuthEntityModels;

namespace InkersCore.Infrastructure
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDBContext _context;
        private readonly ILoggerService<AuditRepository> _loggerService;

        public AuditRepository(AppDBContext context, ILoggerService<AuditRepository> loggerService)
        {
            _context = context;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Function to add audit log
        /// </summary>
        /// <param name="auditData">AuditData</param>
        public void AddAuditLog(AuditData auditData)
        {
            try
            {
                var auditLog = InsertToAuditLog(auditData);
                if (auditData.Type == AuditType.Updated)
                {
                    InsertAuditUpdationLog(auditData, auditLog);
                }
                InsertAuditComment(auditData, auditLog);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _loggerService.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Function to insert new record in audit updation log
        /// </summary>
        /// <param name="auditData">AuditData</param>
        /// <param name="auditLog">AuditLog</param>
        private void InsertAuditUpdationLog(AuditData auditData, AuditLog auditLog)
        {
            auditData.AuditUpdationLog.AuditLog = auditLog;
            _context.AuditUpdationLogs.Add(auditData.AuditUpdationLog);
        }

        /// <summary>
        /// Function to insert new record into audit comment
        /// </summary>
        /// <param name="auditData">AuditData</param>
        /// <param name="auditLog">AuditLog</param>
        private void InsertAuditComment(AuditData auditData, AuditLog auditLog)
        {
            if (auditData.Comment != null && auditData.Comment != string.Empty)
                _context.AuditComments.Add(new AuditComment()
                {
                    AuditLog = auditLog,
                    Comment = auditData.Comment,
                    CreatedBy = auditData.User,
                    LastUpdatedBy = auditData.User
                });
        }

        /// <summary>
        /// Function to convert AuditData to AuditLog object
        /// </summary>
        /// <param name="auditData">AuditData</param>
        /// <returns>AuditLog</returns>
        private AuditLog InsertToAuditLog(AuditData auditData)
        {
            var auditLog = new AuditLog()
            {
                CreatedBy = auditData.User,
                LastUpdatedBy = auditData.User,
                Type = auditData.Type,
                Function = auditData.Function
            };
            _context.AuditLogs.Add(auditLog);
            return auditLog;
        }
    }
}
