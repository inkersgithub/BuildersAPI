using InkersCore.Domain;
using InkersCore.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InkersCore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SubscriptionController : Controller
    {
        private readonly SubscriptionManager _subscriptionManager;

        public SubscriptionController(SubscriptionManager subscriptionManager)
        {
            _subscriptionManager = subscriptionManager;
        }

        [HttpGet]
        public IActionResult GetSubscriptionPlanList()
        {
            var response = _subscriptionManager.GetSubscriptionPlans();
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        public IActionResult AddManualPayment([FromBody] AddManualPaymentRequest addManualPaymentRequest)
        {
            addManualPaymentRequest.UpdatedById = 1;//JsonWebTokenHandler.GetUserIdFromClaimPrincipal(User);
            var response = _subscriptionManager.AddManualPayment(addManualPaymentRequest);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet]
        public IActionResult GetSubscriptionByCompnayId([FromQuery] long companyId)
        {
            var response = _subscriptionManager.GetCompanySubscriptionTransaction(companyId);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet]
        public IActionResult GetSubscritptionList([FromQuery] SubscriptionFetchRequest subscriptionFetchRequest)
        {
            var response = _subscriptionManager.GetSubscriptionTransactionList(subscriptionFetchRequest);
            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
