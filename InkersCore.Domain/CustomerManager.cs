using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Models.AuthEntityModels;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using InkersCore.Models.ResponseModels;
using System.Net.Http.Headers;

namespace InkersCore.Domain
{
    public class CustomerManager
    {
        private readonly ILoggerService<CustomerManager> _loggerService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IGenericRepository<Customer> _customerGenericRepository;
        private readonly IGenericRepository<UserAccount> _userGenericRepository;

        public CustomerManager(ILoggerService<CustomerManager> loggerService,
            ICustomerRepository customerRepository,
            IGenericRepository<Customer> customerGenericRepository,
            IGenericRepository<UserAccount> userGenericRepository)
        {
            _loggerService = loggerService;
            _customerRepository = customerRepository;
            _customerGenericRepository = customerGenericRepository;
            _userGenericRepository = userGenericRepository;
        }

        /// <summary>
        /// Function to create Customer
        /// </summary>
        /// <param name="addUpdateCompanyRequest">AddUpdateCustomerRequest</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse CreateCustomer(AddUpdateCustomerRequest addUpdateCompanyRequest)
        {
            var response = new CommonResponse();
            var transaction = _customerGenericRepository.GetContextTransaction();
            try
            {
                var customer = ConvertToCustomerObj(addUpdateCompanyRequest);
                response.Result = _customerGenericRepository.Insert(customer);
                response.Success = true;
                response.SuccessMessage = "Successfully created";
                transaction.Commit();
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Function to update Customer
        /// </summary>
        /// <param name="addUpdateCompanyRequest">AddUpdateCustomerRequest</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse UpdateCustomer(AddUpdateCustomerRequest addUpdateCompanyRequest)
        {
            var response = new CommonResponse();
            var transaction = _customerGenericRepository.GetContextTransaction();
            try
            {
                DuplicateCustomerCheck(addUpdateCompanyRequest);
                var customer = ConvertToCustomerObj(addUpdateCompanyRequest);
                response.Result = _customerGenericRepository.Update(customer);
                response.Success = true;
                response.SuccessMessage = "Successfully updated";
                transaction.Commit();
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Function to check duplicate customer
        /// </summary>
        /// <param name="addUpdateCompanyRequest">AddUpdateCustomerRequest</param>
        /// <exception cref="Exception">Duplicate user found</exception>
        public void DuplicateCustomerCheck(AddUpdateCustomerRequest addUpdateCompanyRequest)
        {
            var customerObj = (_customerGenericRepository.Find(new Models.EntityFilter<Customer>()
            {
                Predicate = x => (x.Phone == addUpdateCompanyRequest.Phone || x.Email == addUpdateCompanyRequest.Email)
                            && !x.IsDeleted && x.Id != addUpdateCompanyRequest.CustomerId
            }).FirstOrDefault());
            if (customerObj != null)
            {
                var customer = (Customer)customerObj;
                if (customer.Phone == addUpdateCompanyRequest.Phone)
                {
                    throw new Exception("Duplicate phone number found");
                }
                if (customer.Email == addUpdateCompanyRequest.Email)
                {
                    throw new Exception("Duplicate email found");
                }
            }
        }

        /// <summary>
        /// Function to get Customer list by search keyword
        /// </summary>
        /// <param name="searchKeyword">searchKeyword</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetCustomerListByFilter(string searchKeyword)
        {
            var response = new CommonResponse();
            try
            {
                response.Result = _customerGenericRepository.Find(new Models.EntityFilter<Customer>()
                {
                    Predicate = x => !x.IsDeleted &&
                    (x.Name.Contains(searchKeyword) ||
                     x.Phone.Contains(searchKeyword) ||
                     x.Email.Contains(searchKeyword)),
                    SortBy = x => x.Id,
                    SortAscending = false,
                    RowCount = 500
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
        /// Function to get customer by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetCustomerById(long id)
        {
            var response = new CommonResponse();
            try
            {
                response.Success = true;
                response.Result = (Customer)(_customerGenericRepository.GetById(id)
                    ?? throw new Exception("Customer not found"));
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Function to convert request object to entity object
        /// </summary>
        /// <param name="addUpdateCompanyRequest">AddUpdateCustomerRequest</param>
        /// <returnsCustomer></returns>
        /// <exception cref="Exception">Customer not found</exception>
        public Customer ConvertToCustomerObj(AddUpdateCustomerRequest addUpdateCompanyRequest)
        {
            var updatedBy = (UserAccount)_userGenericRepository.GetById(addUpdateCompanyRequest.UpdatedById);
            var customer = new Customer();
            if (addUpdateCompanyRequest.CustomerId != null && addUpdateCompanyRequest.CustomerId != 0)
            {
                customer = (Customer)(_customerGenericRepository.GetById(addUpdateCompanyRequest.CustomerId) ??
                    throw new Exception("Customer not found"));
            }
            else
            {
                customer.CreatedBy = updatedBy;
            }
            customer.Name = addUpdateCompanyRequest.Name;
            customer.Email = addUpdateCompanyRequest.Email;
            customer.Phone = addUpdateCompanyRequest.Phone;
            customer.IsActive = addUpdateCompanyRequest.IsActive;
            customer.LastUpdatedBy = updatedBy;
            return customer;
        }
    }
}
