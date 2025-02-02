using Microsoft.AspNetCore.Mvc;

namespace WebStore9.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
