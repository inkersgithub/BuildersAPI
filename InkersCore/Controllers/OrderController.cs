using InkersCore.Common;
using InkersCore.Domain;
using InkersCore.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InkersCore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly OrderManager _orderManager;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public OrderController(OrderManager orderManager)
        {
            _orderManager = orderManager;
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        [HttpPost]
        public IActionResult CreateDoorServiceOrder([FromBody] DoorServiceOrderRequest orderRequest)
        {
            orderRequest.UpdatedById = 1;//JsonWebTokenHandler.GetUserIdFromClaimPrincipal(User);
            var response = _orderManager.CreateDoorServiceOrder(orderRequest);
            return Ok(JsonConvert.SerializeObject(response, _jsonSerializerSettings));
        }

        [HttpGet]
        public IActionResult GetOrder([FromQuery] long orderId)
        {
            var response = _orderManager.GetOrder(orderId);
            return Ok(JsonConvert.SerializeObject(response, _jsonSerializerSettings));
        }

        [HttpGet]
        public IActionResult GetOrderList([FromQuery]OrderListFilterReqest orderListFilter)
        {
            var response = _orderManager.GetOrderList(orderListFilter);
            return Ok(JsonConvert.SerializeObject(response, _jsonSerializerSettings));
        }

        [HttpGet]
        public IActionResult GetCustomerOrderList([FromQuery] OrderListFilterReqest orderListFilter)
        {
            var response = _orderManager.GetCustomerOrderList(orderListFilter);
            return Ok(JsonConvert.SerializeObject(response, _jsonSerializerSettings));
        }
    }
}
