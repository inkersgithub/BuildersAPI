using InkersCore.Domain.IRepositories;
using InkersCore.Models.AuthEntityModels;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using InkersCore.Models.ResponseModels;

namespace InkersCore.Domain
{
    public class OrderManager
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<Company> _companyGenericRepository;
        private readonly IGenericRepository<Service> _serviceGenericRepository;
        private readonly IGenericRepository<UserAccount> _userGenericRepository;
        private readonly IGenericRepository<Customer> _customerGenericRepository;

        public OrderManager(IOrderRepository orderRepository,
            IGenericRepository<Company> companyGenericRepository,
            IGenericRepository<Service> serviceGenericRepository,
            IGenericRepository<UserAccount> userGenericRepository,
            IGenericRepository<Customer> customerGenericRepository)
        {
            _orderRepository = orderRepository;
            _companyGenericRepository = companyGenericRepository;
            _serviceGenericRepository = serviceGenericRepository;
            _userGenericRepository = userGenericRepository;
            _customerGenericRepository = customerGenericRepository;
        }

        /// <summary>
        /// Function to create door order service
        /// </summary>
        /// <param name="orderRequest">DoorServiceOrderRequest</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse CreateDoorServiceOrder(DoorServiceOrderRequest orderRequest)
        {
            var apiResponse = new CommonResponse();
            var transaction = _orderRepository.GetContextTransaction();
            try
            {
                var order = ConvertToOrderObject(orderRequest);
                ConvertToDoorServiceObject(orderRequest, order);
                apiResponse.Result = _orderRepository.CreateOrder(order);
                apiResponse.Success = true;
                apiResponse.SuccessMessage = "Order placed successfully";

                transaction.Commit();

            }
            catch (Exception ex)
            {
                apiResponse.Error = true;
                apiResponse.ErrorMessage = ex.Message;
                transaction.Rollback();
            }
            return apiResponse;
        }

        /// <summary>
        /// Function to get order by id
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetOrder(long orderId)
        {
            var apiResponse = new CommonResponse();
            try
            {
                apiResponse.Result = _orderRepository.GetOrderById(orderId);
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Error = true;
                apiResponse.ErrorMessage = ex.Message;
            }
            return apiResponse;
        }

        /// <summary>
        /// Function to convert order request to door service object
        /// </summary>
        /// <param name="orderRequest">DoorServiceOrderRequest</param>
        /// <param name="order">Order</param>
        private void ConvertToDoorServiceObject(DoorServiceOrderRequest orderRequest, Order order)
        {
            order.ServiceDoor = new ServiceDoor()
            {
                Category = orderRequest.Category,
                Count = orderRequest.Count,
                PropertyType = orderRequest.PropertyType,
                Type = orderRequest.Type,
                CreatedBy = order.CreatedBy,
                LastUpdatedBy = order.LastUpdatedBy,
            };
        }

        /// <summary>
        /// Function to convert order request to order object
        /// </summary>
        /// <param name="orderRequest">OrderRequest</param>
        /// <returns>Order</returns>
        private Order ConvertToOrderObject(OrderRequest orderRequest)
        {
            var company = (Company)_companyGenericRepository.GetById(orderRequest.CompanyId);
            var service = (Service)_serviceGenericRepository.GetById(orderRequest.ServiceId);
            var user = (UserAccount)_userGenericRepository.GetById(orderRequest.UpdatedById);
            var customer = (Customer)_customerGenericRepository.GetById(orderRequest.CustomerId);
            return new Order()
            {
                Address = orderRequest.Address,
                Amount = 0,
                Company = company,
                Service = service,
                ContactName = orderRequest.ContactName,
                ContactPhone = orderRequest.ContactPhone,
                OrderStatus = OrderStatus.WorkStarted,
                RemarksCustomer = orderRequest.RemarksCustomer,
                LastUpdatedBy = user,
                CreatedBy = user,
                Customer = customer
            };
        }

        /// <summary>
        /// Function to get order list for portal
        /// </summary>
        /// <param name="orderListFilterReqest">OrderListFilterReqest</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetOrderList(OrderListFilterReqest orderListFilterReqest)
        {
            var apiResponse = new CommonResponse();
            try
            {
                apiResponse.Result = _orderRepository.GetAllOrders(orderListFilterReqest);
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Error = true;
                apiResponse.ErrorMessage = ex.Message;
            }
            return apiResponse;
        }

        /// <summary>
        /// Function to get customer order list
        /// </summary>
        /// <param name="orderListFilterReqest">GetCustomerOrderList</param>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetCustomerOrderList(OrderListFilterReqest orderListFilterReqest)
        {
            var apiResponse = new CommonResponse();
            try
            {
                apiResponse.Result = _orderRepository.GetCustomerOrders(orderListFilterReqest);
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Error = true;
                apiResponse.ErrorMessage = ex.Message;
            }
            return apiResponse;
        }
    }
}
