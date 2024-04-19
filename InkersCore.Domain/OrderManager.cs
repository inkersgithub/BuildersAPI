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

        public CommonResponse CreateOrder(OrderRequestCommon orderRequest)
        {
            var apiResponse = new CommonResponse();
            var transaction = _orderRepository.GetContextTransaction();
            try
            {
                var order = ConvertToOrderObject(orderRequest);
                ConvertToServiceObject(orderRequest, order);
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
        /// Function to convert to corresponding service object
        /// </summary>
        /// <param name="orderRequest">orderRequest</param>
        /// <param name="order">order</param>
        private void ConvertToServiceObject(OrderRequestCommon orderRequest, Order order)
        {
            switch (orderRequest.ServiceId)
            {
                case 1:
                    order.ServicePlan = orderRequest.ServicePlan;
                    break;
                case 2:
                    order.ServiceDoor = orderRequest.ServiceDoor;
                    break;
                case 3:
                    order.ServiceWindow = orderRequest.ServiceWindow;
                    break;
                case 4:
                    order.ServiceKitchen = orderRequest.ServiceKitchen;
                    break;
                case 5:
                    order.ServicePool = orderRequest.ServicePool;
                    break;
                default:
                    break;
            }
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
        /// Function to convert order request to order object
        /// </summary>
        /// <param name="orderRequest">OrderRequest</param>
        /// <returns>Order</returns>
        private Order ConvertToOrderObject(OrderRequestCommon orderRequest)
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
