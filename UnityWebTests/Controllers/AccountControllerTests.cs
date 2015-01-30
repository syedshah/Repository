namespace UnityWebTests.Controllers
{
  using System;
  using System.Web.Mvc;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;

  [TestFixture]
  public class AccountControllerTests : ControllerTestsBase
  {
    private Mock<IUserService> _userService;
    private Mock<IFormsAuthenticationService> _formsAuthenticationService;
    private Mock<ILogger> _logger;
    private AccountController _accontController;
    private readonly string _sessionValue = DateTime.Now.AddDays(-7).ToString();

    [SetUp]
    public void SetUp()
    {
      _userService = new Mock<IUserService>();
      _logger = new Mock<ILogger>();
      _formsAuthenticationService = new Mock<IFormsAuthenticationService>();
      _accontController = new AccountController(_userService.Object, _formsAuthenticationService.Object, _logger.Object);
    }

    [Test]
    public void GivenALoggedInUser_WhenIAskForTheUserSummary_ThenIGetTheCorrectView()
    {
      var userMock = new Mock<ApplicationUser>();
      userMock.SetupGet(u => u.UserName).Returns("test");
      userMock.SetupGet(u => u.LastLoginDate).Returns(DateTime.Now);
      _userService.Setup(m => m.GetApplicationUser()).Returns(userMock.Object);

      _accontController = new AccountController(_userService.Object, _formsAuthenticationService.Object, _logger.Object);

      SetControllerContext(_accontController);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(_sessionValue);
      
      var result = (PartialViewResult)_accontController.Summary();
      result.ViewName.Should().Be("_Summary");
    }

    [Test]
    public void GivenInvalidDate_WhenIGoToTheChangePasswrodAction_IAmRedirectedToTheLogOnPage()
    {
      var result = _accontController.ChangePassword(string.Empty) as RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["action"].Should().Be("New");
      result.RouteValues["controller"].Should().Be("Session");
    }

    [Test]
    public void GivenValidDate_WhenIGoToTheChangePasswrodAction_IAmRedirectedToChangePasswordPage()
    {
      var result = _accontController.ChangePassword("username");

      result.Should().NotBeNull();
      result.Should().BeOfType<RedirectToRouteResult>();
      result.As<RedirectToRouteResult>().RouteValues["controller"].Should().Be("Password");
      result.As<RedirectToRouteResult>().RouteValues["action"].Should().Be("ChangeCurrent");
    }
  }
}
