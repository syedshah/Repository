namespace UnityWebTests.Controllers
{
  using System;
  using System.Web.Mvc;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using UnityWeb.Controllers;
  using UnityWeb.Models;


  [TestFixture]
  internal class BaseControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> _logger;

    [SetUp]
    public void Setup()
    {
      _logger = new Mock<ILogger>();
    }

    [Test]
    public void GivenANullLogger_WhenCreatingTheController_ThenAnExceptionIsThrown()
    {
      Action act = () => new TestableBaseController(null);
      act.ShouldThrow<ArgumentNullException>();
    }


    [Test]
    public void GivenThereIsNoFilterContext_ThenTheErrorIsNotHandled()
    {
      var controller = new TestableBaseController(_logger.Object);
      controller.OnException(null);
    }

    [Test]
    public void GivenThereIsNoCustomError_ThenTheErrorIsHandled()
    {
      var controller = new TestableBaseController(_logger.Object);
      var filterContext = new ExceptionContext();
      MockHttpContext.Setup(h => h.IsCustomErrorEnabled).Returns(false);
      filterContext.HttpContext = MockHttpContext.Object;
      controller.OnException(filterContext);
      filterContext.ExceptionHandled.Should().BeFalse();
    }

    [Test]
    public void GivenThereIsACustomError_AndItIsNotAnHttpRequestValidationException_ThenTheViewIsSetToTheErrorView()
    {
      var controller = new TestableBaseController(_logger.Object);
      var filterContext = new ExceptionContext();
      MockHttpContext.Setup(h => h.IsCustomErrorEnabled).Returns(true);
      filterContext.HttpContext = MockHttpContext.Object;
      filterContext.Exception = new Exception();
      controller.OnException(filterContext);
      filterContext.Result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenThereIsACustomError_AndItIsNotAnHttpRequestValidationException_ThenTheViewIsSetToError()
    {
      var controller = new TestableBaseController(_logger.Object);
      var filterContext = new ExceptionContext();
      MockHttpContext.Setup(h => h.IsCustomErrorEnabled).Returns(true);
      filterContext.HttpContext = MockHttpContext.Object;
      filterContext.Exception = new Exception();
      controller.OnException(filterContext);
      filterContext.Result.As<ViewResult>().ViewName.Should().Be("Error");
    }

    [Test]
    public void GivenAnError_ThenAJSONViewResultIsreturned()
    {
      var controller = new TestableBaseController(_logger.Object);
      SetControllerContext(controller);
      var error = controller.JsonError(new Exception(), new ErrorCode(), "");
      error.Should().BeOfType<JsonResult>();
    }

    [Test]
    public void GivenAnError_ThenTheJSONViewResultContainsTheCorrectData()
    {
      var controller = new TestableBaseController(_logger.Object);
      SetControllerContext(controller);
      const ErrorCode code = new ErrorCode();
      var ex = new Exception("error message");
      var error = controller.JsonError(ex, code, "message");
      error.As<JsonResult>().Data.As<ErrorViewModel>().DisplayMessage.Should().Be("message");
      error.As<JsonResult>().Data.As<ErrorViewModel>().ErrorCode.Should().Be(code);
      error.As<JsonResult>().Data.As<ErrorViewModel>().ErrorMessage.Should().Be("error message");
    }
  }

  internal class TestableBaseController : BaseController
  {
    public TestableBaseController(ILogger logger)
      : base(logger)
    {
    }

    public void OnException(System.Web.Mvc.ExceptionContext filterContext)
    {
      base.OnException(filterContext);
    }

    public JsonResult JsonError(Exception e, ErrorCode errorCode, string displayMessage)
    {
      return base.JsonError(e, errorCode, displayMessage);
    }
  }
}

