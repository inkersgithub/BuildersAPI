using InkersCore.Common;
using InkersCore.CustomFilters;
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
            _companyManager = companyManager;
        }

        [HttpPost]
        public IActionResult CreateCompanyRequest([FromBody] AddUpdateCompanyRequest company)
        {
            var response = _companyManager.CreateCompanyRequest(company);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet]
        //[CustomAuthorization(Permission ="COMR")]
        public IActionResult GetCompanyList([FromQuery] string? keyword, string? status)
        {
            var response = _companyManager.GetCompanyList(keyword ?? "", status ?? "");
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet]
        //[CustomAuthorization(Permission ="COMR")]
        public IActionResult GetCompanyById([FromQuery] long id)
        {
            var response = _companyManager.GetCompanyById(id);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        public IActionResult ApproveCompanyRequest([FromBody] AddUpdateCompanyRequest companyRequest)
        {
            companyRequest.UpdatedById = JsonWebTokenHandler.GetUserIdFromClaimPrincipal(User);
            var response = _companyManager.ApproveCompany(companyRequest);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        public IActionResult UpdateCompany([FromBody] AddUpdateCompanyRequest companyRequest)
        {
            companyRequest.UpdatedById = JsonWebTokenHandler.GetUserIdFromClaimPrincipal(User);
            var response = _companyManager.UpdateCompany(companyRequest);
            return Ok(JsonConvert.SerializeObject(response));
        }

    }
}
