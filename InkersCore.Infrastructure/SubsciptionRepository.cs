using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Infrastructure.Configurations;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace InkersCore.Infrastructure
{
    public class SubsciptionRepository : ISubscriptionRepository
    {
        private readonly AppDBContext _context;
        private readonly ILoggerService<SubsciptionRepository> _loggerService;

        public SubsciptionRepository(AppDBContext context, ILoggerService<SubsciptionRepository> loggerService)
        {
            _loggerService = loggerService;
            _context = context;
        }

        public List<SubscriptionTransaction> GetSubscriptionByCompanyId(long companyId)
        {
            var company = _context.Companys.First(x => x.Id == companyId);
            return _context.SubscriptionTransactions
                .Where(x => x.Company == company && x.IsActive && !x.IsDeleted)
                .Include(x => x.SubscriptionPlan)
                .OrderByDescending(x => x.CreatedTime)
                .Take(100)
                .ToList();
        }

        public List<SubscriptionTransaction> GetSubscriptionTransactionList(SubscriptionFetchRequest subscriptionFetchRequest)
        {
            if (subscriptionFetchRequest.Status == 0)
            {
                return _context.SubscriptionTransactions
                    .Where(x => x.IsActive && !x.IsDeleted
                    && x.CreatedTime.Date >= subscriptionFetchRequest.StartDate.Date
                    && x.CreatedTime.Date <= subscriptionFetchRequest.EndDate.Date)
                    .OrderByDescending(x=>x.CreatedTime)
                    .Include(x => x.SubscriptionPlan)
                    .Include(x => x.Company)
                    .ToList();
            }
            else
            {
                var status = (SubscriptionTransactionStatus)subscriptionFetchRequest.Status;
                return _context.SubscriptionTransactions
                    .Where(x => x.IsActive && !x.IsDeleted
                    && x.CreatedTime.Date >= subscriptionFetchRequest.StartDate.Date
                    && x.CreatedTime.Date <= subscriptionFetchRequest.EndDate.Date
                    && x.TransactionStatus == status)
                    .OrderByDescending(x => x.CreatedTime)
                    .Include(x => x.SubscriptionPlan)
                    .Include(x => x.Company)
                    .ToList();
            }
        }
    }
}
