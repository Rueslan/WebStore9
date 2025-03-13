using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebStore9.Controllers;
using WebStore9.Interfaces.TestAPI;
using Assert = Xunit.Assert;

namespace WebStore9.Tests.Controllers
{
    [TestClass]
    public class WebAPIControllerTests
    {
        [TestMethod]
        public void IndexReturnsViewWithDataValues()
        {
            var data = Enumerable.Range(1, 10)
                .Select(i => $"Value - {i}")
                .ToArray();

            var valueServiceMock = new Mock<IValuesService>();
            valueServiceMock.Setup(c => c.GetAll())
                .Returns(data);

            var controller = new WebAPIController(valueServiceMock.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<string>>(viewResult.Model);

            int i = 0;
            foreach (var actualValue in model)
            {
                var expectedValue = data[i++];
                Assert.Equal(expectedValue, actualValue);
            }

            valueServiceMock.Verify(s => s.GetAll());
            valueServiceMock.VerifyNoOtherCalls();
        }
    }
}
