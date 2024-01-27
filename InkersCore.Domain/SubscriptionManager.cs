using InkersCore.Common;
using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Models.AuthEntityModels;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using InkersCore.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkersCore.Domain
{
    public class SubscriptionManager
    {
        private readonly IGenericRepository<SubscriptionPlan> _subscriptionPlanGenericRepository;
        private readonly IGenericRepository<SubscriptionTransaction> _subscriptionTransactionGenericRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILoggerService<SubscriptionManager> _loggerService;
        private readonly IGenericRepository<Company> _genericCompanyRepository;
        private readonly IGenericRepository<UserAccount> _genericUserRepository;

        public SubscriptionManager(IGenericRepository<SubscriptionPlan> subscriptionPlanGenericRepository,
            IGenericRepository<SubscriptionTransaction> subscriptionTransactionGenericRepository,
            ISubscriptionRepository subscriptionRepository, ILoggerService<SubscriptionManager> loggerService,
            IGenericRepository<Company> genericCompanyRepository, IGenericRepository<UserAccount> genericUserRepository)
        {
            _subscriptionPlanGenericRepository = subscriptionPlanGenericRepository;
            _subscriptionTransactionGenericRepository = subscriptionTransactionGenericRepository;
            _subscriptionRepository = subscriptionRepository;
            _loggerService = loggerService;
            _genericCompanyRepository = genericCompanyRepository;
            _genericUserRepository = genericUserRepository;
        }

        /// <summary>
        /// Function to get active subsctiptions plans
        /// </summary>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetSubscriptionPlans()
        {
            var response = new CommonResponse();
            try
            {
                response.Result = _subscriptionPlanGenericRepository.Find(new Models.EntityFilter<SubscriptionPlan>()
                {
                    Predicate = x => x.IsActive && !x.IsDeleted,
                    SortBy = x => x.Days,
                    SortAscending = true
                });
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Function to add manuall payment
        /// </summary>
        /// <param name="addManualPaymentRequest">CheckForActivePlan()</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse AddManualPayment(AddManualPaymentRequest addManualPaymentRequest)
        {
            var response = new CommonResponse();
            var transaction = _subscriptionTransactionGenericRepository.GetContextTransaction();
            try
            {
                StartManualPaymentProcess(addManualPaymentRequest);
                response.Success = true;
                response.SuccessMessage = "Successfully added";
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Function to start manuall payment
        /// </summary>
        /// <param name="addManualPaymentRequest">AddManualPaymentRequest</param>
        /// <exception cref="Exception">Please provide user id</exception>
        public void StartManualPaymentProcess(AddManualPaymentRequest addManualPaymentRequest)
        {
            var company = (Company)(_genericCompanyRepository.Find(new Models.EntityFilter<Company>()
            {
                Predicate = x=>x.Phone == addManualPaymentRequest.Phone
            }).FirstOrDefault()??throw new Exception("Company not found"));
            var user = (UserAccount)_genericUserRepository.GetById(addManualPaymentRequest.UpdatedById ?? throw new Exception("Please provide valid user id"));
            var plan = (SubscriptionPlan)_subscriptionPlanGenericRepository.GetById(addManualPaymentRequest.SubscriptionPlanId);
            CheckForActivePlan();
            _subscriptionTransactionGenericRepository.Insert(new SubscriptionTransaction()
            {
                Company = company,
                SubscriptionPlan = plan,
                TransactionStatus = SubscriptionTransactionStatus.Success,
                CreatedBy = user,
                LastUpdatedBy = user,
                StartDate = DateTimeHandler.GetDateTime(),
                EndDate = DateTimeHandler.GetDateTime().AddDays(plan.Days),
                Reference = addManualPaymentRequest.Reference
            });
        }

        /// <summary>
        /// Function to check for active plan
        /// </summary>
        /// <exception cref="Exception">Active plan found</exception>
        public void CheckForActivePlan()
        {
            var today = DateTimeHandler.GetDateTime().Date;
            var activePlans = _subscriptionTransactionGenericRepository.Find(new Models.EntityFilter<SubscriptionTransaction>()
            {
                // Ensure today's date falls between StartDate and EndDate (inclusive)
                Predicate = x => (x.StartDate.Date <= today && today <= x.EndDate.Date),
                SortAscending = false,
                SortBy = x => x.Id
            })?.FirstOrDefault();

            if (activePlans != null)
            {
                throw new Exception("A plan is already active");
            }
        }

        /// <summary>
        /// Function to get company subscription transaction
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetCompanySubscriptionTransaction(long companyId)
        {
            var response = new CommonResponse();
            try
            {
                var company = _genericCompanyRepository.GetById(companyId);
                response.Success = true;
                response.Result = _subscriptionRepository.GetSubscriptionByCompanyId(companyId);

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Function to get transaction list by filter
        /// </summary>
        /// <param name="subscriptionFetchRequest">SubscriptionFetchRequest</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetSubscriptionTransactionList(SubscriptionFetchRequest subscriptionFetchRequest)
        {
            var response = new CommonResponse();
            try
            {
                if ((subscriptionFetchRequest.EndDate - subscriptionFetchRequest.StartDate).Days > 31)
                {
                    throw new Exception("You can only filter for one month");
                }
                response.Success = true;
                response.Result = _subscriptionRepository.GetSubscriptionTransactionList(subscriptionFetchRequest);

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
