using WebStore9Domain.Entities;
using WebStore9Domain.Entities.Orders;
using WebStore9Domain.ViewModels;

namespace WebStore9Domain.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Date { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }

    public class OrderItemDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }

    public class CreateOrderDTO
    {
        public OrderViewModel Order { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }

    public static class OrderDTOMapper
    {
        public static OrderItemDTO ToDTO(this OrderItem item) => item is null
            ? null
            : new OrderItemDTO
            {
                Id = item.Id,
                ProductId = item.Product.Id,
                Price = item.Price,
                Quantity = item.Quantity,
            };

        public static OrderItem FromDTO(this OrderItemDTO itemDto) => itemDto is null
            ? null
            : new OrderItem
            {
                Id = itemDto.Id,
                Product = new Product{ Id = itemDto.ProductId },
                Price = itemDto.Price,
                Quantity = itemDto.Quantity,
            };

        public static OrderDTO ToDTO(this Order order) => order is null
            ? null
            : new OrderDTO()
            {
                Id = order.Id,
                Address = order.Address,
                Phone = order.Phone,
                Date = order.Date,
                Description = order.Description,
                Items = order.Items.Select(ToDTO),
            };

        public static Order FromDTO(this OrderDTO orderDto) => orderDto is null
            ? null
            : new Order()
            {
                Id = orderDto.Id,
                Address = orderDto.Address,
                Phone = orderDto.Phone,
                Date = orderDto.Date,
                Description = orderDto.Description,
                Items = orderDto.Items.Select(FromDTO).ToList(),
            };

        public static IEnumerable<OrderDTO> ToDTO(this IEnumerable<Order> orders) => orders.Select(ToDTO);

        public static IEnumerable<Order> FromDTO(this IEnumerable<OrderDTO> ordersDto) => ordersDto.Select(FromDTO);

        public static IEnumerable<OrderItemDTO> ToDTO(this CartViewModel cartViewModel) =>
            cartViewModel.Items.Select(p => new OrderItemDTO
            {
                ProductId = p.Product.Id,
                Price = p.Product.Price,
                Quantity = p.Quantity,
            });

        public static CartViewModel ToCartView(this IEnumerable<OrderItemDTO> items) => new()
        {
            Items = items.Select(p => (new ProductViewModel { Id = p.ProductId, Price = p.Price }, p.Quantity))
        };
    }
}
