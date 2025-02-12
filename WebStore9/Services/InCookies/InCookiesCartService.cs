using WebStore9.Services.Interfaces;
using WebStore9.ViewModels;

namespace WebStore9.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly IProductData _productData;
        private readonly string _CartName;

        public InCookiesCartService(HttpContextAccessor httpContextAccessor, IProductData productData)
        {
            _httpContextAccessor = httpContextAccessor;
            _productData = productData;

            var user = httpContextAccessor.HttpContext!.User;
            var user_name = user.Identity.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WebStore9,Cart{user_name}";
        }

        public void Add(int Id)
        {
            throw new NotImplementedException();
        }

        public void Decrement(int Id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public CartViewModel GetViewModel()
        {
            throw new NotImplementedException();
        }
    }
}
