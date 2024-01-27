using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;

namespace InkersCore.Domain.IRepositories
{
    public interface ISubscriptionRepository
    {
        public List<SubscriptionTransaction> GetSubscriptionByCompanyId(long companyId);
        public List<SubscriptionTransaction> GetSubscriptionTransactionList(SubscriptionFetchRequest subscriptionFetchRequest);
    }
}
