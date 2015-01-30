namespace UnityWebTests.Controllers
{
  using System.Web;
  using System.Web.Mvc;
  using System.Web.Routing;
  using Moq;
  using NUnit.Framework;
  using UnityWeb;
  using UnityWeb.Controllers;
  using UnityWebTests.Helpers;

  public class ControllerTestsBase
  {
    protected ControllerContext ControllerContext;
    protected FakeResponse FakeResponse;
    protected Mock<HttpContextBase> MockHttpContext;
    protected Mock<HttpRequestBase> MockRequest;
    protected RouteCollection Routes;
    protected TempDataDictionary TempData;

    [SetUp]
    public void BaseSetup()
    {
      MockHttpContext = new Mock<HttpContextBase>();
      MockRequest = new Mock<HttpRequestBase>();
      FakeResponse = new FakeResponse();
      Routes = new RouteCollection();
      Routes.RegisterRoutes();

      MockHttpContext.Setup(m => m.Request).Returns(MockRequest.Object);
      MockHttpContext.Setup(m => m.Response).Returns(FakeResponse);
      TempData = new TempDataDictionary();
    }

    protected void SetControllerContext(BaseController controller)
    {
      ControllerContext = new ControllerContext(MockHttpContext.Object, new RouteData(), controller);
      controller.ControllerContext = ControllerContext;
      controller.TempData = TempData;
    }
  }
}
