using InkersCore.Domain.IRepositories;
using InkersCore.Infrastructure.Configurations;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InkersCore.Infrastructure
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDBContext _dbContext;
        public OrderRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IDbContextTransaction GetContextTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }

        /// <summary>
        /// Function to create order
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Order</returns>
        public Order CreateOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
            return order;
        }

        /// <summary>
        /// Function to create Door Service
        /// </summary>
        /// <param name="serviceDoor">ServiceDoor</param>
        /// <returns>ServiceDoor</returns>
        public ServiceDoor CreateServiceDoor(ServiceDoor serviceDoor)
        {
            _dbContext.ServiceDoors.Add(serviceDoor);
            _dbContext.SaveChanges();
            return serviceDoor;
        }

        /// <summary>
        /// Function to get order by Id
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <returns>Order</returns>
        public Order GetOrderById(long orderId)
        {
            return _dbContext.Orders
                .Where(x => x.Id == orderId)
                .Include(x=>x.Service)
                .Include(x => x.Company)
                .Include(x => x.ServiceDoor)
                .Include(x => x.ServicePlan)
                .Include(x => x.ServiceKitchen)
                .Include(x => x.ServicePool)
                .Include(x => x.ServiceWindow)
                .First();

        }

        public List<Order> GetAllOrders(OrderListFilterReqest filterReqest)
        {
            if (filterReqest.IsAdmin)
            {
                if (filterReqest.Status == null || filterReqest.Status == 0)
                {
                    return _dbContext.Orders.Where(x => x.IsActive && !x.IsDeleted && x.CreatedTime.Date >= filterReqest.StartDate.Date && x.CreatedTime.Date <= filterReqest.EndDate.Date)
                        .Include(x => x.Company)
                        .Include(x => x.ServiceDoor)
                        .Include(x => x.Service)
                        .OrderByDescending(x => x.CreatedTime)
                        .Take(100)
                        .ToList();
                }
                else
                {
                    return _dbContext.Orders.Where(x => x.IsActive && !x.IsDeleted && x.CreatedTime.Date >= filterReqest.StartDate.Date && x.CreatedTime.Date <= filterReqest.EndDate.Date && x.OrderStatus == (OrderStatus)filterReqest.Status)
                        .Include(x => x.Company)
                        .Include(x => x.ServiceDoor)
                        .Include(x => x.Service)
                        .OrderByDescending(x => x.CreatedTime)
                        .Take(100)
                        .ToList();
                }
            }
            else
            {
                var company = _dbContext.Companys.First(x => x.Id == filterReqest.CompanyId);
                if (filterReqest.Status == null || filterReqest.Status == 0)
                {
                    return _dbContext.Orders
                        .Where(x => x.IsActive && !x.IsDeleted && x.CreatedTime.Date >= filterReqest.StartDate.Date && x.CreatedTime.Date <= filterReqest.EndDate.Date && x.Company == company)
                        .Include(x => x.Company)
                        .Include(x => x.ServiceDoor)
                        .Include(x => x.Service)
                        .OrderByDescending(x => x.CreatedTime)
                        .Take(100)
                        .ToList();
                }
                else
                {
                    return _dbContext.Orders
                        .Where(x => x.IsActive && !x.IsDeleted && x.CreatedTime.Date >= filterReqest.StartDate.Date && x.CreatedTime.Date <= filterReqest.EndDate.Date && x.Company == company && x.OrderStatus == (OrderStatus)filterReqest.Status)
                        .Include(x => x.Company)
                        .Include(x => x.ServiceDoor)
                        .Include(x => x.Service)
                        .OrderByDescending(x => x.CreatedTime)
                        .Take(100)
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Function to get customer order list
        /// </summary>
        /// <param name="filterReqest">OrderListFilterReqest</param>
        /// <returns>OrderList</returns>
        public List<Order> GetCustomerOrders(OrderListFilterReqest filterReqest)
        {
            var customer = _dbContext.Customer.First(x => x.Id == filterReqest.CustomerId);
            return _dbContext.Orders.Where(x => x.IsActive && !x.IsDeleted && x.CreatedTime.Date >= filterReqest.StartDate.Date && x.CreatedTime.Date <= filterReqest.EndDate.Date && x.Customer == customer)
                        .Include(x => x.Company)
                        .Include(x => x.ServiceDoor)
                        .Take(100)
                        .ToList();
        }
    }
}
