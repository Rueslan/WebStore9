using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore9.DAL.Context;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities.Identity;
using WebStore9Domain.Entities.Orders;
using WebStore9Domain.ViewModels;

namespace WebStore9.Services.Services.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStore9DB _db;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<SqlOrderService> _logger;


        public SqlOrderService(WebStore9DB db, UserManager<User> userManager, ILogger<SqlOrderService> logger)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            _logger.LogInformation("Получение заказов пользователя {0}", userName);

            var orders = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(o => o.Product)
                .Where(o => o.User.UserName == userName)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return orders;
        }

        public async Task<Order> GetOrderById(int id)
        {
            _logger.LogInformation("Получение заказа по id {0}", id);

            var order = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == id)
                .ConfigureAwait(false);

            return order;
        }

        public async Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel)
        {
            var user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);

            if (user is null)
                throw new InvalidOperationException($"Пользователь {userName} не найден");

            using (_logger.BeginScope("Формирование заказа для {0}", userName))
            {
                await using var transaction = await _db.Database.BeginTransactionAsync();

                var order = new Order
                {
                    User = user,
                    Address = orderModel.Address,
                    Phone = orderModel.Phone,
                    Description = orderModel.Description,
                };

                var productIds = cart.Items.Select(i => i.Product.Id).ToArray();

                var cartProducts = await _db.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToArrayAsync();

                order.Items = cart.Items.Join(
                    cartProducts,
                    cartItem => cartItem.Product.Id,
                    cartProduct => cartProduct.Id,
                    (cartItem, cartProduct) => new OrderItem
                    {
                        Order = order,
                        Product = cartProduct,
                        Price = cartProduct.Price, //можно добавить скидку тут!
                        Quantity = cartItem.Quantity,
                    }
                ).ToArray();

                await _db.AddAsync(order);

                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation("Заказ id:{0} успешно сформирован для пользователя {1}", order.Id, userName);

                return order;
            }
        }
    }
}
