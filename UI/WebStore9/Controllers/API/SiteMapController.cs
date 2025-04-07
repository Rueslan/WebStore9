using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using WebStore9.Interfaces.Services;

namespace WebStore9.Controllers.API
{
    public class SiteMapController : ControllerBase
    {
        public IActionResult Index([FromServices]IProductData productData)
        {
            var nodes = new List<SitemapNode>()
            {
                new(Url.Action("Index","Home")),
                new(Url.Action("ContactUs","Home")),
                new(Url.Action("Index","Catalog")),
                new(Url.Action("Index","WebAPI")),
            };

            nodes.AddRange(productData.GetSections().Select(s => new SitemapNode(Url.Action("Index","Catalog", new {SectionId = s.Id}))));

            foreach (var brand in productData.GetBrands()) 
                nodes.Add(new(Url.Action("Index","Catalog", new {BrandId = brand.Id})));

            foreach (var product in productData.GetProducts())
                nodes.Add(new(Url.Action("Details", "Catalog", new { product.Id })));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
