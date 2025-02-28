using System.Net.Http.Json;
using WebStore9.Interfaces;
using WebStore9.Interfaces.Services;
using WebStore9.WebAPI.Clients.Base;
using WebStore9Domain.DTO;
using WebStore9Domain.Entities.Orders;
using WebStore9Domain.ViewModels;

namespace WebStore9.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient client) : base(client, WebAPIAddresses.Orders) { }

        public async Task<IEnumerable<Order>> GetUserOrders(string user)
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{user}").ConfigureAwait(false);
            return orders.FromDTO();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await GetAsync<OrderDTO>($"{Address}/{id}").ConfigureAwait(false);
            return order.FromDTO();
        }

        public async Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel)
        {
            var model = new CreateOrderDTO
            {
                Items = cart.ToDTO(),
                Order = orderModel,
            };

            var response = await PostAsync($"{Address}/{userName}", model).ConfigureAwait(false);
            var order = await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<OrderDTO>()
                .ConfigureAwait(false);

            return order.FromDTO();
        }
    }
}
