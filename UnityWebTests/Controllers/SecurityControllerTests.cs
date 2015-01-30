namespace UnityWebTests.Controllers
{
  using System.Collections.Generic;
  using System.Web.Mvc;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models;
  using UnityWeb.Models.Security;

    [TestFixture]
  public class SecurityControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> _logger;
    private Mock<IUserService> _userService;
    private Mock<ISecurityQuestionService> _securityQuestionService;
    private Mock<ISecurityAnswerService> _securityAnswerService;
    private SecurityController _controller;

    [SetUp]
    public void SetUp()
    {
      _logger = new Mock<ILogger>();
      _securityQuestionService = new Mock<ISecurityQuestionService>();
      _securityAnswerService = new Mock<ISecurityAnswerService>();
      _userService = new Mock<IUserService>();

      _controller = new SecurityController(_userService.Object, _securityQuestionService.Object, _securityAnswerService.Object, _logger.Object);
    }

    [Test]
    public void GivenASecurityController_WhenTheIndexPageIsAccessed()
    {
      var result = this._controller.Index();
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void WhenIClickOnTheAddOrUpdateLink_IfHasSecurityAnswersIsTrue_ThenIGetTheCorrectRedirectResult()
    {
      this._userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      this._securityAnswerService.Setup(x => x.HasSecurityAnswers(It.IsAny<string>())).Returns(true);

      var result = this._controller.AddOrUpdate() as RedirectToRouteResult;

      result.Should().BeOfType<RedirectToRouteResult>();

      this._userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);

      this._securityAnswerService.Verify(x => x.HasSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);

      result.Should().NotBeNull();

      result.RouteValues["Controller"].Should().Be("Security");

      result.RouteValues["action"].Should().Be("EditAnswers");

    }

    [Test]
    public void WhenIClickOnTheAddOrUpdateLink_IfHasSecurityAnswersIsFalse_ThenIGetTheCorrectRedirectResult()
    {
        this._userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

        this._securityAnswerService.Setup(x => x.HasSecurityAnswers(It.IsAny<string>())).Returns(false);

        var result = this._controller.AddOrUpdate() as RedirectToRouteResult;

        result.Should().BeOfType<RedirectToRouteResult>();

        this._userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);

        this._securityAnswerService.Verify(x => x.HasSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);

        result.Should().NotBeNull();

        result.RouteValues["Controller"].Should().Be("Security");

        result.RouteValues["action"].Should().Be("AddAnswers");

    }

    [Test]
    public void IAskForTheAddAnswersPage_ThenTheAddViewShouldBeReturned()
    {
      this._userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      var listSecurityQuestions = new List<SecurityQuestion>();

      listSecurityQuestions.Add(new SecurityQuestion());
      listSecurityQuestions.Add(new SecurityQuestion());
      listSecurityQuestions.Add(new SecurityQuestion());
      listSecurityQuestions.Add(new SecurityQuestion());

      this._securityQuestionService.Setup(x => x.GetSecurityQuestions()).Returns(listSecurityQuestions);

      var result = (ViewResult)_controller.AddAnswers();

      result.Should().NotBeNull();
      result.Should().BeOfType<ViewResult>();
      result.Model.Should().BeOfType<AddSecurityAnswersModel>();

      this._userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
      this._securityQuestionService.Verify(x => x.GetSecurityQuestions(), Times.AtLeastOnce);
    }

    [Test]
    public void WhenISubmittTheAddAnswersPage_IfModelIsValid_ThenTheCorrectViewIsReturned()
    {
      var model = new AddSecurityAnswersModel();

        model.Answer1 = "answer1";
        model.Question1Id = "dssf";
        model.UserId = "eee";

        model.Answer1 = "answer1";
        model.Answer2 = "answer2";
        model.Answer3 = "answer3";
        model.Answer4 = "answer4";
        model.Answer5 = "answer5";
        model.Answer6 = "answer6";
        model.Answer7 = "answer7";
        model.Answer8 = "answer8";
        model.Answer9 = "answer9";
        model.Answer10 = "answer10";

        this._securityAnswerService.Setup(x => x.SaveSecurityAnswers(It.IsAny<List<SecurityAnswer>>(), It.IsAny<string>()));

      var result = this._controller.AddAnswers(model);
      var redirectResult = (RedirectToRouteResult)result;

      result.Should().BeOfType<RedirectToRouteResult>();
      redirectResult.RouteValues["action"].Should().Be("Index");

      this._securityAnswerService.Verify(x => x.SaveSecurityAnswers(It.IsAny<List<SecurityAnswer>>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void ISubmittTheAddAnswersPage_IfModelIsNotValid_ThenCorrectViewIsReturned()
    {
        _controller.ModelState.AddModelError("Answer1", "Answer for question is required");

        var model = new AddSecurityAnswersModel();

        model.Answer1 = "answer1";
        model.Question1Id = "dssf";
        model.UserId = "eee";

        var result = this._controller.AddAnswers(model);
        var viewResult = (ViewResult)result;

        result.Should().BeOfType<ViewResult>();
        viewResult.Model.Should().BeOfType<AddSecurityAnswersModel>();
        viewResult.TempData["message"].ToString().ShouldBeEquivalentTo("Please correct the errors and try again.");
        viewResult.ViewName.Should().BeEquivalentTo("Add");
    }
    
    [Test]
    public void IAskForTheEditAnswersPage_ThenTheEditViewShouldBeReturned()
    {
        this._userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

        var listSecurityAnswers = new List<SecurityAnswer>();

        listSecurityAnswers.Add(new SecurityAnswer() { Answer = "answer1", SecurityQuestion = new SecurityQuestion("Question1")});
        listSecurityAnswers.Add(new SecurityAnswer() { Answer = "answer2", SecurityQuestion = new SecurityQuestion("Question2") });
        listSecurityAnswers.Add(new SecurityAnswer() { Answer = "answer3", SecurityQuestion = new SecurityQuestion("Question3") });
        listSecurityAnswers.Add(new SecurityAnswer() { Answer = "answer4", SecurityQuestion = new SecurityQuestion("Question4") });

        this._securityAnswerService.Setup(x => x.GetSecurityAnswers(It.IsAny<string>())).Returns(listSecurityAnswers);

        var result = (ViewResult)_controller.EditAnswers();

        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
        result.Model.Should().BeOfType<EditSecuirtyAnswersModel>();

        this._userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
        this._securityAnswerService.Verify(x => x.GetSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenISubmittTheEditAnswersPage_IfModelIsValid_ThenTheCorrectViewIsReturned()
    {
        var model = new EditSecuirtyAnswersModel();

        model.Answer1 = "answer1";
        model.Answer1Id = "dhd";

        this._securityAnswerService.Setup(x => x.UpdateSecurityAnswers(It.IsAny<List<SecurityAnswer>>()));

        var result = this._controller.EditAnswers(model);
        var viewResult = (ViewResult)result;

        result.Should().BeOfType<ViewResult>();
        viewResult.TempData["message"].ToString().ShouldBeEquivalentTo("Update was successful.");
        viewResult.ViewName.Should().BeEquivalentTo("Edit");

        this._securityAnswerService.Verify(x => x.UpdateSecurityAnswers(It.IsAny<List<SecurityAnswer>>()), Times.AtLeastOnce);
    }

    [Test]
    public void ISubmittTheEditAnswersPage_IfModelIsNotValid_ThenCorrectViewIsReturned()
    {
        _controller.ModelState.AddModelError("Answer1", "Answer for question is required");

        var model = new EditSecuirtyAnswersModel();

        model.Answer1 = "answer1";
        model.Answer1Id = "dhd";

        var result = this._controller.EditAnswers(model);
        var viewResult = (ViewResult)result;

        result.Should().BeOfType<ViewResult>();
        viewResult.Model.Should().BeOfType<EditSecuirtyAnswersModel>();
        viewResult.TempData["message"].ToString().ShouldBeEquivalentTo("Update was unsuccessful.");
        viewResult.ViewName.Should().BeEquivalentTo("Edit");
    }
  }
}
