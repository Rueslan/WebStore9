using Microsoft.AspNetCore.Identity;
using WebStore9.DAL.Context;
using WebStore9.Services.Interfaces;
using WebStore9.ViewModels;
using WebStore9Domain.Entities.Identity;
using WebStore9Domain.Entities.Orders;

namespace WebStore9.Services.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStore9DB _db;
        private readonly UserManager<User> _userManager;


        public SqlOrderService(WebStore9DB db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public Task<IEnumerable<Order>> GetUserOrders(string user)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel)
        {
            throw new NotImplementedException();
        }
    }
}
