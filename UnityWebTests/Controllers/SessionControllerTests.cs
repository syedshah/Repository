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
  using UnityWeb.Models.User;

  [TestFixture]
  public class SessionControllerTests : ControllerTestsBase
  {
    private SessionController _sessionController;
    private Mock<ILogger> _logger;
    private Mock<IUserService> _userService;
    private Mock<ISecurityAnswerService> _securityAnswerService;
    private Mock<ISessionService> _sessionService;
    private readonly string _sessionValue = DateTime.Now.AddDays(-7).ToString();
   

    [SetUp]
    public void Setup()
    {
      _logger = new Mock<ILogger>();
      _userService = new Mock<IUserService>();
      _securityAnswerService = new Mock<ISecurityAnswerService>();
      _sessionService = new Mock<ISessionService>();

      _sessionController = new SessionController(_userService.Object, _securityAnswerService.Object, _sessionService.Object, _logger.Object);
       
      SetControllerContext(_sessionController);
    }

    [Test]
    public void GivenAnInvalidEMail_WhenILogin_ThenIGetRedirectedToTheRegistrationPage()
    {
      _sessionController.ModelState.AddModelError("error", "message");

      var result = _sessionController.Create(new LoginUserViewModel(), string.Empty) as ViewResult;
      
      result.Should().NotBeNull();
      result.ViewName.Should().Be("New");
    }

    [Test]
    public void GivenAnAuthenticatedUser_WhenILogin_ThenIGetTheRedirectView()
    {
      SetControllerContext(_sessionController);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);

      MockHttpContext.Setup(h => h.User).Returns(new UserViewModel() { IsLoggedIn = true });
      var result = _sessionController.New() as RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["action"].Should().Be("Index");
      result.RouteValues["controller"].Should().Be("Dashboard");
    }

    [Test]
    public void GivenNoAuthenticatedUser_WhenILogin_ThenIGetTheLoginView()
    {
      SetControllerContext(_sessionController);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);

      MockHttpContext.Setup(h => h.User).Returns(new UserViewModel());
      ActionResult result = _sessionController.New();

      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAValidUserAndPassword_WhenILogin_ThenIGetRedirectedToTheDashboardPage()
    {
      var userMock = new Mock<ApplicationUser>();
      _userService.Setup(m => m.GetApplicationUser("username","password")).Returns(userMock.Object);
      userMock.SetupGet(u => u.IsApproved).Returns(true);
      userMock.SetupGet(u => u.IsLockedOut).Returns(false);

      this._userService.Setup(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(false);
      this._userService.Setup(x => x.SignIn(userMock.Object, false));
      this._userService.Setup(x => x.UpdateUserLastLogindate(It.IsAny<string>()));
      this._securityAnswerService.Setup(x => x.HasSecurityAnswers(It.IsAny<string>())).Returns(true);

      SetControllerContext(_sessionController);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var result =
          _sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
          RedirectToRouteResult;

      _userService.Verify(m => m.GetApplicationUser(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
      this._userService.Verify(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.AtLeastOnce);
      this._userService.Verify(x => x.SignIn(userMock.Object, false), Times.AtLeastOnce);
      this._userService.Verify(x => x.UpdateUserLastLogindate(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
      this._securityAnswerService.Verify(x => x.HasSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);

      result.Should().NotBeNull();
      result.RouteValues["controller"].Should().Be("Dashboard");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void GivenAValidUserAndPassword_WhenILoginAsALockedOutUser_ThenIGetRedirectedToTheLockedOutPage()
    {
      var userMock = new Mock<ApplicationUser>();
      _userService.Setup(m => m.GetApplicationUser("username", "password")).Returns(userMock.Object);
      userMock.SetupGet(u => u.IsLockedOut).Returns(true);

      var result =
          _sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
          RedirectToRouteResult;

      result.RouteValues["controller"].Should().Be("Session");
      result.RouteValues["action"].Should().Be("LockedOut");
    }

    [Test]
    public void GivenAValidUserAndPassword_WhenILoginAsAnInactiveUser_ThenIGetRedirectedToTheInactivePage()
    {
      var userMock = new Mock<ApplicationUser>();
      _userService.Setup(m => m.GetApplicationUser("username", "password")).Returns(userMock.Object);
      userMock.SetupGet(u => u.IsLockedOut).Returns(false);
      userMock.SetupGet(u => u.IsApproved).Returns(false);

      var result =
          _sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
          RedirectToRouteResult;

      result.RouteValues["controller"].Should().Be("Session");
      result.RouteValues["action"].Should().Be("InActive");
    }

    [Test]
    public void GivenAValidUserAndPasswordAndUserHasNoSecurityAnswers_WhenILogin_ThenIGetRedirectedToSecurityQuestionsPage()
    {
        var userMock = new Mock<ApplicationUser>();
        _userService.Setup(m => m.GetApplicationUser("username", "password")).Returns(userMock.Object);
        userMock.SetupGet(u => u.IsApproved).Returns(true);
        userMock.SetupGet(u => u.IsLockedOut).Returns(false);

        this._userService.Setup(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(false);
        this._userService.Setup(x => x.SignIn(userMock.Object, false));
        this._userService.Setup(x => x.UpdateUserLastLogindate(It.IsAny<string>()));
        this._securityAnswerService.Setup(x => x.HasSecurityAnswers(It.IsAny<string>())).Returns(false);

        SetControllerContext(_sessionController);
        MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

        var result =
            _sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
            RedirectToRouteResult;

        _userService.Verify(m => m.GetApplicationUser(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        this._userService.Verify(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.AtLeastOnce);
        this._userService.Verify(x => x.SignIn(userMock.Object, false), Times.AtLeastOnce);
        this._userService.Verify(x => x.UpdateUserLastLogindate(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        this._securityAnswerService.Verify(x => x.HasSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);

        result.Should().NotBeNull();
        result.RouteValues["controller"].Should().Be("Security");
        result.RouteValues["action"].Should().Be("AddAnswers");
    }

    [Test]
    public void GivenAValidUserAndPasswordAndPasswordHasExpired_WhenILogin_ThenIGetRedirectedToTheChangePasswordPage()
    {
        var userMock = new Mock<ApplicationUser>();
        _userService.Setup(m => m.GetApplicationUser("username", "password")).Returns(userMock.Object);
        userMock.SetupGet(u => u.IsApproved).Returns(true);
        userMock.SetupGet(u => u.IsLockedOut).Returns(false);

        this._userService.Setup(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

        SetControllerContext(_sessionController);
        MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

        var result =
            _sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
            RedirectToRouteResult;

        _userService.Verify(m => m.GetApplicationUser(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        this._userService.Verify(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.AtLeastOnce);

        result.Should().NotBeNull();
        result.RouteValues["controller"].Should().Be("Account");
        result.RouteValues["action"].Should().Be("ChangePassword");
    }

    [Test]
    public void GivenAnInValidUserAndPassword_WhenILogin_TheTempDataContainsTheCorrectMessagee()
    {
      _userService.Setup(m => m.GetApplicationUser(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<ApplicationUser>());
      _userService.Setup(m => m.UpdateUserFailedLogin(It.IsAny<string>()));

      SetControllerContext(_sessionController);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var result =
          _sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
          ViewResult;

      _userService.Verify(m => m.GetApplicationUser(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
      TempData["message"].Should().Be("Login was unsuccessful. Please correct the errors and try again.");
    }

    [Test]
    public void GivenALockedOutUser_WhenILogin_IGetRedirectedToTheLockedOutView()
    {
      _userService.Setup(m => m.GetApplicationUser("username")).Returns(new ApplicationUser { IsLockedOut = true });
      _userService.Setup(m => m.UpdateUserFailedLogin(It.IsAny<string>()));
      _userService.Setup(m => m.IsLockedOut(It.IsAny<string>())).Returns(true);

      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var result = _sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as RedirectToRouteResult;
      
      result.RouteValues["action"].Should().Be("LockedOut");
      result.RouteValues["controller"].Should().Be("Session");
    }

    [Test]
    public void GivenALoggedInUser_WhenILoginOut_ThenIGetTheEndView()
    {
      SetControllerContext(_sessionController);

      MockHttpContext.SetupGet(x => x.Session["SessionGuid"]).Returns("guid");

      _sessionService.Setup(s => s.Update("guid", It.IsAny<DateTime>()));

      MockHttpContext.Setup(x => x.Session.Clear());

      var result = _sessionController.Remove() as ActionResult;
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenALoggedInUser_WhenMySessionExpires_ThenIGetTheExpiredView()
    {
      SetControllerContext(_sessionController);

      MockHttpContext.Setup(x => x.Session.Clear());

      var result = _sessionController.Expired() as ActionResult;
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenALockedOutUser_WhenITryToLogIn_IGetTheLockedOutView()
    {
      var result = _sessionController.LockedOut() as ActionResult;
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAnInValidPassword_WhenILogin_TheFailedLogInCountIsIncremented()
    {
      ApplicationUser applicationUser = null;

      _userService.Setup(m => m.GetApplicationUser("username", "InValidPassword")).Returns(applicationUser);
      _userService.Setup(m => m.GetApplicationUser("username")).Returns(new ApplicationUser { Id = "1" });

      var result =
          _sessionController.Create(new LoginUserViewModel { Username = "username", Password = "InValidPassword" }, string.Empty) as
          ViewResult;

      _userService.Verify(m => m.UpdateUserFailedLogin(It.IsAny<string>()), Times.Once);
    }
  }
}
