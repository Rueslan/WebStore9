using Microsoft.AspNetCore.Mvc;
using WebStore9.Controllers;
using Assert = Xunit.Assert;

namespace WebStore9.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexReturnsView()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ContactUsReturnsView()
        {
            var controller = new HomeController();

            var result = controller.ContactUs();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void StatusWithId404ReturnsView()
        {
            #region Arrange

            const string id = "404";
            const string expectedViewName = "Page404";
            var controller = new HomeController();

            #endregion

            #region Act

            var result = controller.Status(id);

            #endregion

            #region Assert

            var viewResult = Assert.IsType<ViewResult>(result);

            var actualViewName = viewResult.ViewName;

            Assert.Equal(expectedViewName, actualViewName);

            #endregion
        }

        [TestMethod]
        [DataRow("QWE")]
        [DataRow("123")]
        public void StatusWithIdReturnsView(string id)
        {
            #region Arrange

            var expectedContent = "Status - " + id;
            var controller = new HomeController();

            #endregion

            #region Act

            var result = controller.Status(id);

            #endregion

            #region Assert

            var contentResult = Assert.IsType<ContentResult>(result);

            var actualContent = contentResult.Content;

            Assert.Equal(expectedContent, actualContent);

            #endregion
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        public void StatusThrownArgumentNullExceptionWhenIdIsNull(string id)
        {
            var controller = new HomeController();

            var result = controller.Status(id);
        }

        [TestMethod]
        public void StatusThrownArgumentNullExceptionWhenIdIsNull2()
        {
            const string expectedParameterName = "id";
            var controller = new HomeController();

            var actualException = Assert.Throws<ArgumentNullException>(() => controller.Status(null));
            string actualParameterName = actualException.ParamName; 
            Assert.Equal(expectedParameterName, actualParameterName);
        }

    }
}
