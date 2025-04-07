using WebStore9Domain.Entities;

namespace WebStore9Domain.ViewModels
{
    public class BreadCrumbsViewModel
    {
        public Section Section { get; set; }

        public Brand Brand { get; set; }

        public string ProductName { get; set; }
    }
}
