using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore9Domain.Entities
{
    public class Cart
    {
        public ICollection<CartItem> items { get; set; } = new List<CartItem>();

        public int ItemsCount => items?.Sum(item => item.Quantity) ?? 0;

    }

    public class CartItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
