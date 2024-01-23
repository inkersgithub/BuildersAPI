using InkersCore.CustomFilters;
using InkersCore.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InkersCore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ServiceController : Controller
    {
        private ServiceManager _serviceManager;

        public ServiceController(ServiceManager serviceManager)
        {
                _serviceManager = serviceManager;
        }

        [HttpGet]
        //[CustomAuthorization(Permission ="SERR")]
        public ActionResult GetServiceList()
        {
            var result = _serviceManager.GetServiceList();
            return Ok(JsonConvert.SerializeObject(result));
        }
    }
}
