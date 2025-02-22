using Microsoft.AspNetCore.Mvc;

namespace WebStore9.Components
{
    public class UserInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => User.Identity?.IsAuthenticated == true
            ? View("UserInfo")
            : View();
    }
}
