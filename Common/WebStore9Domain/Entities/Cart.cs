namespace WebStore9Domain.Entities
{
    public class Cart
    {
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        public int ItemsCount => Items?.Sum(item => item.Quantity) ?? 0;

    }

    public class CartItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
