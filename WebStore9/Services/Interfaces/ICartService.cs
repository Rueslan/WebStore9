using WebStore9.ViewModels;

namespace WebStore9.Services.Interfaces
{
    public interface ICartService
    {
        void Add(int Id);

        void Decrement(int Id);

        void Remove(int Id);

        void Clear();

        Task<CartViewModel> GetViewModelAsync();

    }
}
