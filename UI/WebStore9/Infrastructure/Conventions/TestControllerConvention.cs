using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebStore9.Infrastructure.Conventions
{
    public class TestControllerConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerName.Contains("Test"))
            {
                controller.ApiExplorer.IsVisible = false;
            }
        }
    }
}
