using Microsoft.AspNetCore.Mvc;
using Moq;
using WebStore9.Controllers;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities;
using WebStore9Domain.ViewModels;
using Assert = Xunit.Assert;

namespace WebStore9.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void DetailReturnsWithCorrectView()
        {
            const int expectedId = 321;
            const string expectedName = "Test Product";
            const int expectedOrder = 5;
            const decimal expectedPrice = 1025m;
            const string expectedImgUrl = "/img/product.jpg";
            const int expectedBrandId = 7;
            const string expectedBrandName = "Test Brand";
            const int expectedBrandOrder = 10;
            const int expectedSectionId = 14;
            const string expectedSectionName = "Test Section";
            const int expectedSectionOrder = 122;

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(s => s.GetProductById(It.Is<int>(id => id > 0)))
                .Returns<int>(id => new Product
                {
                    Id = id,
                    Name = expectedName,
                    Order = expectedOrder,
                    SectionId = expectedSectionId,
                    Section = new()
                    {
                        Id = expectedSectionId,
                        Name = expectedSectionName,
                        Order = expectedSectionOrder,
                    },
                    BrandId = expectedBrandId,
                    Brand = new()
                    {
                        Id = expectedBrandId,
                        Name = expectedBrandName,
                        Order = expectedBrandOrder
                    },
                    ImageUrl = expectedImgUrl,
                    Price = expectedPrice
                });

            var controller = new CatalogController(productDataMock.Object);

            var result = controller.Details(expectedId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.Model);

            Assert.Equal(expectedId, model.Id);
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedPrice, model.Price);
            Assert.Equal(expectedImgUrl, model.ImageUrl);
            Assert.Equal(expectedBrandName, model.BrandName);
            Assert.Equal(expectedSectionName, model.SectionName);

            productDataMock.Verify(s => s.GetProductById(It.Is<int>(id => id > 0)));
            productDataMock.VerifyNoOtherCalls();
        }
    }
}
