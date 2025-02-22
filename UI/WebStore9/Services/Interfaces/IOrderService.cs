using WebStore9.ViewModels;
using WebStore9Domain.Entities.Orders;

namespace WebStore9.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrders(string user);

        Task<Order> GetOrderById(int id);

        Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel);
    }
}
