namespace UnityWebTests.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Web.Mvc;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using UnityWeb.Controllers;
  using ServiceInterfaces;
  using Entities;
  using UnityWeb.Models.Password;

  [TestFixture]
  public class PasswordControllerTests : ControllerTestsBase
  {
    private Mock<IUserService> _userService;
    private Mock<IPasswordHistoryService> _passwordHistoryService;
    private Mock<ISecurityQuestionService> _securityQuestionService;
    private Mock<ISecurityAnswerService> _securityAnswerService;
    private Mock<ILogger> _logger;
    private PasswordController _controller;
 
    [SetUp]
    public void SetUp()
    {
      this._userService = new Mock<IUserService>();
      this._passwordHistoryService = new Mock<IPasswordHistoryService>();
      this._securityQuestionService = new Mock<ISecurityQuestionService>();
      this._securityAnswerService = new Mock<ISecurityAnswerService>();
      _logger = new Mock<ILogger>();

      this._controller = new PasswordController(this._userService.Object,
          this._passwordHistoryService.Object,
          this._securityQuestionService.Object,
          this._securityAnswerService.Object,
          this._logger.Object);
    }

    [Test]
    public void WhenIClickOnTheForgotPasswordLink_ThenTheForgotPasswordPageShouldBeAccessed()
    {
      var result = this._controller.Forgotten();
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void WhenIClickOnTheForgotPasswordLink_ThenTheForgottenViewShouldContainTheModel()
    {
      var result = (ViewResult)this._controller.Forgotten();
      result.Model.Should().BeOfType<ForgottenPasswordModel>();
    }

    [Test]
    public void WhenISubmitForgottenPasswordPage_AndUserNameFieldIsEmpty_ThenTheForgottenViewShouldContainTheModel()
    {
      var model = new ForgottenPasswordModel();

      _controller.ModelState.AddModelError("UserName", "UserName is required");

      var result = (ViewResult)this._controller.Forgotten(model);
      result.Model.Should().BeOfType<ForgottenPasswordModel>();
      result.TempData["message"].ToString().ShouldBeEquivalentTo("Please correct the errors and try again.");
    }

    [Test]
    public void WhenISubmitForgottenPasswordPage_IfModelStateIsValidButUserDoesNotExist_ThenTheForgottenViewShouldContainTheModel()
    {
      var model = new ForgottenPasswordModel();

      this._userService.Setup(x => x.GetApplicationUser(It.IsAny<string>())).Returns(It.IsAny<ApplicationUser>());

      var result = (ViewResult)this._controller.Forgotten(model);

      this._userService.Verify(x => x.GetApplicationUser(It.IsAny<string>()), Times.AtLeastOnce);
      result.Model.Should().BeOfType<ForgottenPasswordModel>();
      result.TempData["message"].ToString().ShouldBeEquivalentTo("Please contact your administrator.");
    }

    [Test]
    public void WhenISubmitForgottenPasswordPage_IfModelStateIsValidUserExistsButHasNoSecurityAnswers_ThenTheForgottenViewShouldContainTheModel()
    {
      var model = new ForgottenPasswordModel();

      this._userService.Setup(x => x.GetApplicationUser(It.IsAny<string>())).Returns(new ApplicationUser());

      this._securityAnswerService.Setup(x => x.HasSecurityAnswers(It.IsAny<string>())).Returns(false);

      var result = (ViewResult)this._controller.Forgotten(model);
      this._userService.Verify(x => x.GetApplicationUser(It.IsAny<string>()), Times.AtLeastOnce);
      this._securityAnswerService.Verify(x => x.HasSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);
      result.Model.Should().BeOfType<ForgottenPasswordModel>();
      result.TempData["message"].ToString().ShouldBeEquivalentTo("Please contact your administrator.");
    }

    [Test]
    public void WhenISubmitForgottenPasswordPage_IfModelStateIsValidUserExistsAndHasNoSecurityAnswers_ThenIGetTheCorrectView()
    {
      var model = new ForgottenPasswordModel();

      var user = new ApplicationUser();

      this._userService.Setup(x => x.GetApplicationUser(It.IsAny<string>())).Returns(user);

      this._securityAnswerService.Setup(x => x.HasSecurityAnswers(It.IsAny<string>())).Returns(true);

      var result = this._controller.Forgotten(model);
      this._userService.Verify(x => x.GetApplicationUser(It.IsAny<string>()), Times.AtLeastOnce);
      this._securityAnswerService.Verify(x => x.HasSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);
      result.Should().BeOfType<RedirectToRouteResult>();
    }

    [Test]
    public void WhenIAskForSecurityQuestionsPage_ThenTheSecurityQuestionsViewShouldContainTheModel()
    {
      var listSecurityQuestions = new List<SecurityQuestion>();
      listSecurityQuestions.Add(new SecurityQuestion("Question 1"));
      listSecurityQuestions.Add(new SecurityQuestion("Question 2"));
      listSecurityQuestions.Add(new SecurityQuestion("Question 3"));

      _controller.TempData["UserId"] = "reguireh";

      this._securityQuestionService.Setup(x => x.GetThreeRandomSecurityQuestions()).Returns(listSecurityQuestions);
      
      var result = (ViewResult)this._controller.SecurityQuestions();
      result.Model.Should().BeOfType<PasswordSecurityQuestionsModel>(); 
      this._securityQuestionService.Verify(x => x.GetThreeRandomSecurityQuestions(), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIAskForSecurityQuestionsPage_ThenTheSecurityQuestionsViewShouldBeReturned()
    {
      var listSecurityQuestions = new List<SecurityQuestion>();
      listSecurityQuestions.Add(new SecurityQuestion("Question 1"));
      listSecurityQuestions.Add(new SecurityQuestion("Question 2"));
      listSecurityQuestions.Add(new SecurityQuestion("Question 3"));

      _controller.TempData["UserId"] = "reguireh";

      this._securityQuestionService.Setup(x => x.GetThreeRandomSecurityQuestions()).Returns(listSecurityQuestions);

      var result = this._controller.SecurityQuestions();
      result.Should().BeOfType<ViewResult>();
      this._securityQuestionService.Verify(x => x.GetThreeRandomSecurityQuestions(), Times.AtLeastOnce);
    }

    [Test]
    public void WhenISubmitSecurityQuestionsPage_IfModelStateIsNotvalid_TheCorrectViewIsReturnedAndItContainsTheModel()
    {
      _controller.ModelState.AddModelError("Answer1", "Answer for question is required");  

      PasswordSecurityQuestionsModel model = new PasswordSecurityQuestionsModel();

      var result = (ViewResult)this._controller.SecurityQuestions(model);
      result.Model.Should().BeOfType<PasswordSecurityQuestionsModel>();
      result.TempData["message"].ToString().ShouldBeEquivalentTo("Please correct the errors and try again.");
    }

    [Test]
    public void WhenISubmitSecurityQuestionsPage_ModelStateIsvalidAndAnswersAreInvalid_TheCorrectviewIsTypeReturned()
    {
      PasswordSecurityQuestionsModel model = new PasswordSecurityQuestionsModel();
      model.UserId = Guid.NewGuid().ToString();

      this._securityAnswerService.Setup(
          x => x.SecurityAnswerIsValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

      var result = this._controller.SecurityQuestions(model) as RedirectToRouteResult;

      this._securityAnswerService.Verify(
          x => x.SecurityAnswerIsValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(3));

      result.Should().NotBeNull();
      result.Should().BeOfType<RedirectToRouteResult>();
      result.RouteValues["controller"].Should().Be("Password");
      result.RouteValues["action"].Should().Be("SecurityQuestions");
       
    }

    [Test]
    public void WhenISubmitSecurityQuestionsPage_ModelStateIsvalidAndAnswersAreValid_TheCorrectviewIsTypeReturned()
    {
      PasswordSecurityQuestionsModel model = new PasswordSecurityQuestionsModel();
      model.UserId = Guid.NewGuid().ToString();
        
      this._securityAnswerService.Setup(
          x => x.SecurityAnswerIsValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

      var result = this._controller.SecurityQuestions(model) as RedirectToRouteResult;
         
      this._securityAnswerService.Verify(
            x => x.SecurityAnswerIsValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(3));

      result.Should().NotBeNull();
      result.Should().BeOfType<RedirectToRouteResult>();
      result.RouteValues["controller"].Should().Be("Password");
      result.RouteValues["action"].Should().Be("Change");
    }

    [Test]
    public void WhenIAskForTheChangePassowrdPage_TheViewIsReturnedAndContainsTheRightModel()
    {
      _controller.TempData["UserId"] = "reguireh";

      var result = (ViewResult)_controller.Change();

      result.Model.Should().BeOfType<ChangePasswordModel>();
    }

    [Test]
    public void WhenISubmitTheChangePasswordPage_AndTheModelIsInValid_ItShouldReturnTheRightViewAndContainTheModel()
    {
      _controller.ModelState.AddModelError("Password", "Password has already been used recently");

      var model = new ChangePasswordModel();

      var result = (ViewResult)this._controller.Change(model);

      result.Should().NotBeNull();
      result.Model.Should().BeOfType<ChangePasswordModel>();
      result.TempData["message"].ToString().ShouldBeEquivalentTo("Please correct the errors and try again.");
    }

    [Test]
    public void WhenISubmitTheChangePasswordPage_AndTheModelIsValid_ItShouldReturnTheRightViewAndContainTheModel()
    {
      var model = new ChangePasswordModel();

      var user = new ApplicationUser();

      this._userService.Setup(x => x.ChangePassword(It.IsAny<string>(), It.IsAny<string>()));
      this._userService.Setup(x => x.GetApplicationUserById(It.IsAny<string>())).Returns(user);
      this._userService.Setup(x => x.SignIn(user, false));
      this._userService.Setup(x => x.UpdateUserLastLogindate(It.IsAny<string>()));

      var result = this._controller.Change(model) as RedirectToRouteResult;

      result.Should().NotBeNull();

      result.Should().BeOfType<RedirectToRouteResult>();
      this._userService.Verify(x => x.ChangePassword(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
      this._userService.Verify(x => x.GetApplicationUserById(It.IsAny<string>()), Times.AtLeastOnce);
      this._userService.Verify(x => x.SignIn(user, false), Times.AtLeastOnce);
      this._userService.Verify(x => x.UpdateUserLastLogindate(It.IsAny<string>()), Times.AtLeastOnce);
      result.RouteValues["controller"].Should().Be("Dashboard");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void WhenIWantToGetThePasswordComplexityPartialView_ThePasswordComplexityPartialViewIsRetrieved()
    {
      var result = (PartialViewResult)_controller.PasswordComplexity();
      result.ViewName.Should().Be("_PasswordComplexity");
    }

    [Test]
    public void WhenIAskForTheChangeCurrentPassowrdPage_TheViewIsReturnedAndContainsTheRightModel()
    {
        _controller.TempData["username"] = "Greg";

        var user = new ApplicationUser("Greg");

        this._userService.Setup(x => x.GetApplicationUser(It.IsAny<string>())).Returns(user);

        var result = (ViewResult)_controller.ChangeCurrent();

        result.Model.Should().BeOfType<ChangeCurrentPasswordModel>();

        this._userService.Verify(x => x.GetApplicationUser(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenISubmitTheChangeCurrentPasswordPage_AndTheModelIsValid_ItShouldReturnTheRightViewAndContainTheModel()
    {
        var model = new ChangeCurrentPasswordModel();

        var user = new ApplicationUser();

        this._userService.Setup(x => x.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        this._userService.Setup(x => x.GetApplicationUserById(It.IsAny<string>())).Returns(user);
        this._userService.Setup(x => x.SignIn(user, false));
        this._userService.Setup(x => x.UpdateUserLastLogindate(It.IsAny<string>()));

        var result = this._controller.ChangeCurrent(model) as RedirectToRouteResult;

        result.Should().NotBeNull();

        result.Should().BeOfType<RedirectToRouteResult>();
        this._userService.Verify(x => x.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        this._userService.Verify(x => x.GetApplicationUserById(It.IsAny<string>()), Times.AtLeastOnce);
        this._userService.Verify(x => x.SignIn(user, false), Times.AtLeastOnce);
        this._userService.Verify(x => x.UpdateUserLastLogindate(It.IsAny<string>()), Times.AtLeastOnce);
        result.RouteValues["controller"].Should().Be("Dashboard");
        result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void WhenISubmitTheChangeCurrentPasswordPage_AndTheModelIsInValid_ItShouldReturnTheRightViewAndContainTheModel()
    {
        _controller.ModelState.AddModelError("Password", "Password has already been used recently");

        var model = new ChangeCurrentPasswordModel();

        var result = (ViewResult)this._controller.ChangeCurrent(model);

        result.Should().NotBeNull();
        result.Model.Should().BeOfType<ChangeCurrentPasswordModel>();
        result.TempData["message"].ToString().ShouldBeEquivalentTo("Please correct the errors and try again.");
    }
  }
}
