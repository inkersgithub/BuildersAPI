using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using Microsoft.EntityFrameworkCore.Storage;

namespace InkersCore.Domain.IRepositories
{
    public interface IOrderRepository
    {
        IDbContextTransaction GetContextTransaction();
        Order CreateOrder(Order order);
        ServiceDoor CreateServiceDoor(ServiceDoor serviceDoor);
        Order GetOrderById(long orderId);
        List<Order> GetAllOrders(OrderListFilterReqest filterReqest);
        List<Order> GetCustomerOrders(OrderListFilterReqest filterReqest);
    }
}
