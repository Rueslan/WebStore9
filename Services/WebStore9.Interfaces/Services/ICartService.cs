using WebStore9Domain.ViewModels;

namespace WebStore9.Interfaces.Services
{
    public interface ICartService
    {
        void Add(int Id);

        void Decrement(int Id);

        void Remove(int Id);

        void Clear();

        CartViewModel GetViewModel();

    }
}
