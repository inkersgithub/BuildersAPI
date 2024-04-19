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


        [HttpGet]
        public async Task<IActionResult> GetJsonData()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "ServiceData.json");

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("JSON file not found.");
            }

            // Read the file asynchronously
            var jsonData = await System.IO.File.ReadAllTextAsync(filePath);

            // Return the file content as a JSON object
            return Ok(jsonData);
        }
    }


}
