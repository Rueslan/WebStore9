using Microsoft.AspNetCore.Mvc;

namespace WebStore9.Components
{
    public class BrandsViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
