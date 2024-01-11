using InkersCore.Domain;
using InkersCore.Domain.IRepositories;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InkersCore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CompanyController : Controller
    {
        private readonly CompanyManager _companyManager;

        public CompanyController(CompanyManager companyManager)
        {
             _companyManager    = companyManager;
        }

        [HttpPost]
        public IActionResult CreateCompanyRequest([FromBody] AddCompanyRequest company)
        {
            var response = _companyManager.CreateCompanyRequest(company);
            return Ok(JsonConvert.SerializeObject(response));
        }

    }
}
