using WebStore9Domain.Entities.Orders;
using WebStore9Domain.ViewModels;

namespace WebStore9.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrders(string user);

        Task<Order> GetOrderById(int id);

        Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel);
    }
}
