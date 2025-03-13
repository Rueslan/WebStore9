using WebStore9Domain.Entities;

namespace WebStore9.Interfaces.Services
{
    public interface ICartStore
    {
        public Cart cart { get; set; }
    }
}
