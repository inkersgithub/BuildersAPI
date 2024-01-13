using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using InkersCore.Models.ResponseModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InkersCore.Domain
{
    public class CompanyManager
    {
        private readonly IGenericRepository<Company> _companyGenericReposiotry;
        private readonly IGenericRepository<Service> _serviceGenericReposiotry;
        private readonly IGenericRepository<CompanyServiceMapping> _companyServiceMappingGenericReposiotry;
        private readonly ILoggerService<CompanyManager> _loggerService;
        private readonly IUserRepository _userRepository;

        public CompanyManager(IGenericRepository<Company> companyGenericReposiotry,
            ILoggerService<CompanyManager> loggerService,
            IGenericRepository<CompanyServiceMapping> companyServiceMappingGenericReposiotry,
            IGenericRepository<Service> serviceGenericReposiotry,
            IUserRepository userRepository)
        {
            _companyGenericReposiotry = companyGenericReposiotry;
            _serviceGenericReposiotry = serviceGenericReposiotry;
            _companyServiceMappingGenericReposiotry = companyServiceMappingGenericReposiotry;
            _loggerService = loggerService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Function to create company request
        /// </summary>
        /// <param name="companyRequest">AddCompanyRequest</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse CreateCompanyRequest(AddCompanyRequest companyRequest)
        {
            var response = new CommonResponse();
            var transaction = _companyGenericReposiotry.GetContextTransaction();
            try
            {
                var company = ConvertToCompanyObj(companyRequest);
                _companyGenericReposiotry.Insert(company);
                AddCompanyServiceMappings(company, companyRequest);
                response.Success = true;
                response.SuccessMessage = "Successfully created";
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
        /// Function to add Company Service Mappings
        /// </summary>
        /// <param name="company">Company</param>
        /// <param name="companyRequest">AddCompanyRequest</param>
        /// <exception cref="Exception">Service not found</exception>
        public void AddCompanyServiceMappings(Company company, AddCompanyRequest companyRequest)
        {

            var serviceMappingList = new List<CompanyServiceMapping>();
            foreach (var item in companyRequest.RequestedServiceIds)
            {
                var service = _serviceGenericReposiotry?.GetById(item) ?? throw new Exception("Service not found");
                serviceMappingList.Add(new CompanyServiceMapping()
                {
                    Company = company,
                    Service = (Service)service,
                    CreatedBy = company.CreatedBy,
                    LastUpdatedBy = company.LastUpdatedBy
                });
            }
            _companyServiceMappingGenericReposiotry.InsertRange(serviceMappingList);
        }

        /// <summary>
        /// Function to convert to Company object
        /// </summary>
        /// <param name="companyRequest">AddCompanyRequest</param>
        /// /// <param name="isApproved">IsApproved</param>
        /// <returns></returns>
        /// <exception cref="Exception">User not found</exception>
        public Company ConvertToCompanyObj(AddCompanyRequest companyRequest, bool isApproved = false)
        {
            var duplicateCompany = _companyGenericReposiotry.Find(new Models.EntityFilter<Company>()
            {
                Predicate = x=>(x.Phone == companyRequest.Phone || x.Email == companyRequest.Email) && x.IsActive && x.IsApproved && !x.IsDeleted
            });
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Phone == companyRequest.Phone) throw new Exception("Duplicate phone");
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Email == companyRequest.Email) throw new Exception("Duplicate email");
            
            var user = _userRepository.GetUserAccountById(1) ?? throw new Exception("User not found");
            return new Company()
            {
                Name = companyRequest.Name,
                Email = companyRequest.Email,
                Phone = companyRequest.Phone,
                Address1 = companyRequest.Address1,
                Address2 = companyRequest.Address2,
                Address3 = companyRequest.Address3,
                IsApproved = isApproved,
                ZipCode = companyRequest.ZipCode,
                LastUpdatedBy = user,
                CreatedBy = user,
            };
        }
    }
}
