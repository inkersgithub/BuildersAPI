using InkersCore.Common;
using InkersCore.Domain;
using InkersCore.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InkersCore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomerController : Controller
    {
        private readonly CustomerManager _customerManager;

        public CustomerController(CustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        [HttpGet]
        public IActionResult GetCustomerById([FromQuery] long id)
        {
            var response = _customerManager.GetCustomerById(id);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet]
        public IActionResult GetCustomerList([FromQuery] string? keyword)
        {
            var response = _customerManager.GetCustomerListByFilter(keyword ?? "");
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] AddUpdateCustomerRequest customerRequest)
        {
            customerRequest.UpdatedById = JsonWebTokenHandler.GetUserIdFromClaimPrincipal(User);
            var response = _customerManager.CreateCustomer(customerRequest);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        public IActionResult UpdateCustomer([FromBody] AddUpdateCustomerRequest customerRequest)
        {
            customerRequest.UpdatedById = JsonWebTokenHandler.GetUserIdFromClaimPrincipal(User);
            var response = _customerManager.UpdateCustomer(customerRequest);
            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
