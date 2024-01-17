using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Models.AuthEntityModels;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using InkersCore.Models.ResponseModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace InkersCore.Domain
{
    public class CompanyManager
    {
        private readonly IGenericRepository<Company> _companyGenericReposiotry;
        private readonly IGenericRepository<Service> _serviceGenericReposiotry;
        private readonly IGenericRepository<CompanyServiceMapping> _companyServiceMappingGenericReposiotry;
        private readonly ILoggerService<CompanyManager> _loggerService;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<UserAccount> _userGenericRepository;
        private readonly ICompanyRepository _companyRepository;

        public CompanyManager(IGenericRepository<Company> companyGenericReposiotry,
            ILoggerService<CompanyManager> loggerService,
            IGenericRepository<CompanyServiceMapping> companyServiceMappingGenericReposiotry,
            IGenericRepository<Service> serviceGenericReposiotry,
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IGenericRepository<UserAccount> userGenericRepository)
        {
            _companyGenericReposiotry = companyGenericReposiotry;
            _serviceGenericReposiotry = serviceGenericReposiotry;
            _companyServiceMappingGenericReposiotry = companyServiceMappingGenericReposiotry;
            _loggerService = loggerService;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _userGenericRepository = userGenericRepository;
        }

        /// <summary>
        /// Function to create company request
        /// </summary>
        /// <param name="companyRequest">AddCompanyRequest</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse CreateCompanyRequest(AddUpdateCompanyRequest companyRequest)
        {
            var response = new CommonResponse();
            companyRequest.IsApproved = companyRequest.IsApproved ?? 0;
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
        public void AddCompanyServiceMappings(Company company, AddUpdateCompanyRequest companyRequest)
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
        public Company ConvertToCompanyObj(AddUpdateCompanyRequest companyRequest, int isApproved = 0)
        {
            var duplicateCompany = _companyGenericReposiotry.Find(new Models.EntityFilter<Company>()
            {
                Predicate = x => (x.Phone == companyRequest.Phone || x.Email == companyRequest.Email) && x.IsActive && x.IsApproved == 1 && !x.IsDeleted
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

        /// <summary>
        /// Function to get Company List
        /// </summary>
        /// <param name="keyword">keyword</param>
        /// <param name="status">status</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetCompanyList(string keyword, string status)
        {
            var response = new CommonResponse();
            try
            {
                response.Result = _companyGenericReposiotry.Find(new Models.EntityFilter<Company>()
                {
                    Predicate = x => (x.Name.Contains(keyword) || x.Phone.Contains(keyword) || x.Email.Contains(keyword)) && x.IsActive && !x.IsDeleted && x.IsApproved.ToString().Contains(status),
                    SortBy = x => x.Id,
                    SortAscending = false,
                    RowCount = 500
                });
                response.Success = true;
                response.SuccessMessage = "Successfully fetched";
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Function to get Company by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetCompanyById(long id)
        {
            var response = new CommonResponse();
            try
            {
                var company = (Company)_companyGenericReposiotry.GetById(id) ?? throw new Exception("Company not found");
                company.RequestedServiceIds = _companyRepository.GetCompanyServiceMappings(company).Select(x => x.Service.Id).ToArray();
                response.Result = company;
                response.Success = true;
                response.SuccessMessage = "Successfully fetched";
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Function to approve or decline company
        /// </summary>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse ApproveCompany(AddUpdateCompanyRequest companyRequest)
        {
            var response = new CommonResponse();
            var transaction = _companyGenericReposiotry.GetContextTransaction();
            try
            {
                if (companyRequest.IsApproved == 1)
                {
                    response.SuccessMessage = StartApproveProcess(companyRequest);
                }
                else
                {
                    response.SuccessMessage = StartDeclineProcess(companyRequest);
                }
                response.Result = _companyGenericReposiotry.GetById(companyRequest.CompanyId);
                response.Success = true;
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
        /// Function to update company
        /// </summary>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse UpdateCompany(AddUpdateCompanyRequest companyRequest)
        {
            var response = new CommonResponse();
            var transaction = _companyGenericReposiotry.GetContextTransaction();
            try
            {
                companyRequest.IsApproved = 1;
                response.SuccessMessage = StartUpdateCompanyProcess(companyRequest);
                response.Result = _companyGenericReposiotry.GetById(companyRequest.CompanyId);
                response.Success = true;
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public string StartUpdateCompanyProcess(AddUpdateCompanyRequest companyRequest)
        {
            ValidateCompanyAndUserForEdit(companyRequest);
            var company = GetCompanyById(companyRequest.CompanyId);
            company.UserAccount.Name = companyRequest.Name;
            company.UserAccount.Phone = companyRequest.Phone;
            company.UserAccount.Email = companyRequest.Email;
            UpdateCompanyDetails(company, companyRequest, company.UserAccount);
            return "Successfully Approved";
        }

        /// <summary>
        /// Function to start Approval Process
        /// </summary>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        /// <returns>string</returns>
        public string StartApproveProcess(AddUpdateCompanyRequest companyRequest)
        {
            ValidateCompanyAndUser(companyRequest);
            var company = GetCompanyById(companyRequest.CompanyId);
            var userAccount = CreateUserAccount(companyRequest);
            companyRequest.IsApproved = 1;
            UpdateCompanyDetails(company, companyRequest, userAccount);
            SendPassword(company, userAccount);
            return "Successfully Approved";
        }

        /// <summary>
        /// Function to validate duplicate User
        /// </summary>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        /// <exception cref="Exception">User not found</exception>
        private void ValidateCompanyAndUser(AddUpdateCompanyRequest companyRequest)
        {
            CompanyUserDuplicateCheck(companyRequest);
            if (_userRepository.GetUserAccountById(companyRequest.UpdatedById ?? throw new Exception("User not found")) == null)
            {
                throw new Exception("User not found");
            }
        }

        /// <summary>
        /// Function to validate duplicate User for edit
        /// </summary>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        /// <exception cref="Exception">User not found</exception>
        private void ValidateCompanyAndUserForEdit(AddUpdateCompanyRequest companyRequest)
        {
            CompanyUserDuplicateCheckForEdit(companyRequest);
            if (_userRepository.GetUserAccountById(companyRequest.UpdatedById ?? throw new Exception("User not found")) == null)
            {
                throw new Exception("User not found");
            }
        }

        /// <summary>
        /// Function to get company by id
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <returns></returns>
        /// <exception cref="Exception">Company not found</exception>
        private Company GetCompanyById(long? companyId)
        {
            return _companyRepository.GetCompanyById(companyId) ??
                   throw new Exception("Company not found");
        }

        /// <summary>
        /// Function to update company objects
        /// </summary>
        /// <param name="company">Company</param>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        private void UpdateCompanyDetails(Company company, AddUpdateCompanyRequest companyRequest, UserAccount userAccount)
        {
            company.Address1 = companyRequest.Address1;
            company.Address2 = companyRequest.Address2;
            company.Address3 = companyRequest.Address3;
            company.ZipCode = companyRequest.ZipCode;
            company.IsApproved = companyRequest.IsApproved ?? throw new Exception("Approved Status not found");
            company.Phone = companyRequest.Phone;
            company.Email = companyRequest.Email;
            company.Name = companyRequest.Name;
            company.UserAccount = userAccount;
            _companyGenericReposiotry.Update(company);
            ModifyServiceMappings(company, companyRequest);
        }

        /// <summary>
        /// Function to modify compnay service mappings
        /// </summary>
        /// <param name="company">Company</param>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        /// <exception cref="Exception">User not found</exception>
        private void ModifyServiceMappings(Company company, AddUpdateCompanyRequest companyRequest)
        {
            var user = _userRepository.GetUserAccountById(companyRequest.UpdatedById ?? throw new Exception("UserId not found"));
            var existingMappings = _companyRepository.GetCompanyServiceMappings(company);
            foreach (var mapping in existingMappings)
            {
                if (!companyRequest.RequestedServiceIds.Contains(mapping.Service.Id))
                {
                    mapping.IsDeleted = true;
                    mapping.IsActive = false;
                }
            }
            _companyServiceMappingGenericReposiotry.UpdateRange(existingMappings.ToList());
            foreach (var serviceId in companyRequest.RequestedServiceIds)
            {
                if (existingMappings.FirstOrDefault(x => x.Service.Id == serviceId) == null)
                {
                    _companyServiceMappingGenericReposiotry.Insert(new CompanyServiceMapping()
                    {
                        Company = company,
                        Service = (Service)_serviceGenericReposiotry.GetById(serviceId),
                        CreatedBy = user,
                        LastUpdatedBy = user
                    });
                }
            }
        }

        /// <summary>
        /// Function to create user account for company
        /// </summary>
        /// <param name="companyRequest">companyRequest</param>
        /// <returns>UserAccount</returns>
        /// <exception cref="Exception">UserId not found</exception>
        private UserAccount CreateUserAccount(AddUpdateCompanyRequest companyRequest)
        {
            var userAccount = _userRepository.GetUserAccountById(companyRequest.UpdatedById ?? throw new Exception("UserId cannot be null"));
            var password = "Test";
            return _userRepository.CreateUserAccount(new UserAccount
            {
                Name = companyRequest.Name,
                Phone = companyRequest.Phone,
                Email = companyRequest.Email,
                Password = password,
                LastUpdatedBy = userAccount,
                CreatedBy = userAccount
            });
        }

        /// <summary>
        /// Function to send password to user
        /// </summary>
        /// <param name="company">Company</param>
        /// <param name="userAccount">UserAccount</param>
        public void SendPassword(Company company, UserAccount userAccount)
        {

        }

        /// <summary>
        /// Function to check company or useraccount duplicate
        /// </summary>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        /// <exception cref="Exception">Duplicate record found</exception>
        public void CompanyUserDuplicateCheck(AddUpdateCompanyRequest companyRequest)
        {
            var duplicateCompany = _companyGenericReposiotry.Find(new Models.EntityFilter<Company>()
            {
                Predicate = x => (x.Phone == companyRequest.Phone || x.Email == companyRequest.Email) && x.IsActive && x.IsApproved == 1 && !x.IsDeleted
            });
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Phone == companyRequest.Phone)
                throw new Exception("Duplicate company phone");
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Email == companyRequest.Email)
                throw new Exception("Duplicate compnay email");
            var duplicateUser = _userGenericRepository.Find(new Models.EntityFilter<UserAccount>()
            {
                Predicate = x => (x.Phone == companyRequest.Phone || x.Email == companyRequest.Email) && x.IsActive && !x.IsDeleted
            });
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Phone == companyRequest.Phone)
                throw new Exception("Duplicate user phone");
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Email == companyRequest.Email)
                throw new Exception("Duplicate company email");
        }

        /// <summary>
        /// Function to check company or useraccount duplicate for Edit
        /// </summary>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        /// <exception cref="Exception">Duplicate record found</exception>
        public void CompanyUserDuplicateCheckForEdit(AddUpdateCompanyRequest companyRequest)
        {
            var duplicateCompany = _companyGenericReposiotry.Find(new Models.EntityFilter<Company>()
            {
                Predicate = x => (x.Phone == companyRequest.Phone || x.Email == companyRequest.Email) && x.IsActive && x.IsApproved == 1 && !x.IsDeleted && x.Id != companyRequest.CompanyId
            });
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Phone == companyRequest.Phone)
                throw new Exception("Duplicate company phone");
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Email == companyRequest.Email)
                throw new Exception("Duplicate compnay email");
            var company = _companyRepository.GetCompanyById(companyRequest.CompanyId);
            var duplicateUser = _userGenericRepository.Find(new Models.EntityFilter<UserAccount>()
            {
                Predicate = x => (x.Phone == companyRequest.Phone || x.Email == companyRequest.Email) && x.IsActive && !x.IsDeleted && x.Id != company.UserAccount.Id
            });
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Phone == companyRequest.Phone)
                throw new Exception("Duplicate user phone");
            if (duplicateCompany.Count() > 0 && duplicateCompany.First().Email == companyRequest.Email)
                throw new Exception("Duplicate company email");
        }

        /// <summary>
        /// Function to start decline process
        /// </summary>
        /// <param name="companyRequest">AddUpdateCompanyRequest</param>
        /// <returns></returns>
        /// <exception cref="Exception">Company not founc</exception>
        public string StartDeclineProcess(AddUpdateCompanyRequest companyRequest)
        {
            var company = (Company)_companyGenericReposiotry.GetById(companyRequest.CompanyId) ??
                throw new Exception("Company not found");
            company.IsApproved = 2;
            _companyGenericReposiotry.Update(company);
            return "Successfully declined";
        }
    }
}
