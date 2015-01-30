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
  using UnityWeb.Models.Otf;

  [TestFixture]
  public class OtfControllerTests : ControllerTestsBase
  {
    private Mock<IAppManCoEmailService> _appManCoEmailService;
    private Mock<IApplicationService> _applicationService;
    private Mock<IManCoService> _manCoService;
    private Mock<IDocTypeService> _docTypeService;
    private Mock<IUserService> _userService;
    private Mock<ILogger> _logger;
    private OtfController _controller;
    private const string ExpectedRefererUrl = "value";

    [SetUp]
    public void SetUp()
    {
        var headers = new FormCollection { { "Referer", ExpectedRefererUrl } };
        MockRequest.Setup(r => r.Headers).Returns(headers);

      _appManCoEmailService = new Mock<IAppManCoEmailService>();
      _applicationService = new Mock<IApplicationService>();
      _manCoService = new Mock<IManCoService>();
      _docTypeService = new Mock<IDocTypeService>();
      _userService = new Mock<IUserService>();
      _logger = new Mock<ILogger>();

      _controller = new OtfController(_appManCoEmailService.Object,
          _applicationService.Object,
          _manCoService.Object,
          _docTypeService.Object,
          _userService.Object,
          _logger.Object);

      SetControllerContext(_controller);
    }

    [Test]
    public void GivenOtfController_WhenTheIndexPageIsAccessed_ThenTheIndexViewAndRightModelIsReturned()
    {
      this._applicationService.Setup(x => x.GetApplications()).Returns(new List<Application>());
      this._manCoService.Setup(x => x.GetManCos()).Returns(new List<ManCo>());
    
      var result = _controller.Index();
      var viewResult = result as ViewResult;
      result.Should().BeOfType<ViewResult>();
      viewResult.Model.Should().BeOfType<SelectAppManCoEmailsViewModel>();

      this._applicationService.Verify(x => x.GetApplications(), Times.AtLeastOnce);
      this._manCoService.Verify(x => x.GetManCos(), Times.AtLeastOnce);
    }

    [Test]
    public void GivenOtfController_WhenTheShowPartialViewIsAccessedAjaxCallIsFalse_ThenARedirectIsDoneToTheControllerIndex()
    {
      var paramModel = new OtfParameterModel();
      paramModel.IsAjaxCall = false;
      paramModel.Page = 2;
        
      var result = _controller.Show(paramModel);
      var redirectToRouteResult = result as RedirectToRouteResult;
      result.Should().BeOfType<RedirectToRouteResult>();
      redirectToRouteResult.RouteValues["action"].Should().Be("Index");
      redirectToRouteResult.RouteValues["controller"].Should().Be("Otf");
    }

    [Test]
    public void GivenOtfController_WhenTheShowPartialViewIsAccessedAndEmailsIsLessThanOne_ThenTheRightPartialViewAndRightViewModelIsReturned()
    {
      var pagedAppManCoEmails = new PagedResult<AppManCoEmail>();
        var listAppManCoEmails = new List<AppManCoEmail>();
        pagedAppManCoEmails.Results = listAppManCoEmails;
        pagedAppManCoEmails.ItemsPerPage = 10;
      this._applicationService.Setup(x => x.GetApplications()).Returns(new List<Application>());
      this._manCoService.Setup(x => x.GetManCos()).Returns(new List<ManCo>());
      this._docTypeService.Setup(x => x.GetDocTypes()).Returns(new List<DocType>());
      this._appManCoEmailService.Setup(x => x.GetPagedAppManCoEmails(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).Returns(pagedAppManCoEmails);

      var paramModel = new OtfParameterModel();
      paramModel.AppId = 2;
      paramModel.ManCoId = 4;
      paramModel.IsAjaxCall = true;
      paramModel.Page = 1;

      var result = _controller.Show(paramModel);
      var partialViewResult = result as PartialViewResult;
      result.Should().BeOfType<PartialViewResult>();
      partialViewResult.Model.Should().BeOfType<OtfItemsViewModel>();
      partialViewResult.TempData["comment"].Should().Be("No records for the selected criteria");

      this._applicationService.Verify(x => x.GetApplications(), Times.AtLeastOnce());
      this._manCoService.Verify(x => x.GetManCos(), Times.AtLeastOnce());
      this._docTypeService.Verify(x => x.GetDocTypes(), Times.AtLeastOnce());
      this._appManCoEmailService.Verify(x => x.GetPagedAppManCoEmails(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce());
    }

    [Test]
    public void GivenOtfController_WhenTheShowPartialViewIsAccessedAndEmailsIsMoreThanOne_ThenTheRightPartialViewAndRightViewModelIsReturned()
    {
        var pagedAppManCoEmails = new PagedResult<AppManCoEmail>();
        var otfs = new List<AppManCoEmail>();
        otfs.Add(new AppManCoEmail() { DocTypeId = 3, DocType = new DocType("code", "description"), ApplicationId = 5, Application = new Application("code", "description"), ManCoId = 7, ManCo = new ManCo("code", "description"), Id = 1, Email = "john@dstoutput.co.uk", AccountNumber = "2547 963" });
        otfs.Add(new AppManCoEmail() { DocTypeId = 3, DocType = new DocType("code2", "description2"), ApplicationId = 8, Application = new Application("code2", "description2"), ManCoId = 9, ManCo = new ManCo("code2", "description2"), Id = 2, Email = "garyn@dstoutput.co.uk", AccountNumber = "2547 954" });
        pagedAppManCoEmails.Results = otfs;
        pagedAppManCoEmails.ItemsPerPage = 10;
        this._applicationService.Setup(x => x.GetApplications()).Returns(new List<Application>());
        this._manCoService.Setup(x => x.GetManCos()).Returns(new List<ManCo>());
        this._docTypeService.Setup(x => x.GetDocTypes()).Returns(new List<DocType>());
        this._appManCoEmailService.Setup(x => x.GetPagedAppManCoEmails(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).Returns(pagedAppManCoEmails);

        var paramModel = new OtfParameterModel();
        paramModel.AppId = 2;
        paramModel.ManCoId = 4;
        paramModel.IsAjaxCall = true;
        paramModel.Page = 1;

        var result = _controller.Show(paramModel);
        var partialViewResult = result as PartialViewResult;
        result.Should().BeOfType<PartialViewResult>();
        partialViewResult.Model.Should().BeOfType<OtfItemsViewModel>();

        this._applicationService.Verify(x => x.GetApplications(), Times.AtLeastOnce());
        this._manCoService.Verify(x => x.GetManCos(), Times.AtLeastOnce());
        this._docTypeService.Verify(x => x.GetDocTypes(), Times.AtLeastOnce());
        this._appManCoEmailService.Verify(x => x.GetPagedAppManCoEmails(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce());
    }

    [Test]
    public void GivenOtfController_WhenTheEditPageIsAccessed_ThenTheEditViewAndRightModelIsReturned()
    {
      this._applicationService.Setup(x => x.GetApplications()).Returns(new List<Application>());
      this._manCoService.Setup(x => x.GetManCos()).Returns(new List<ManCo>());
      this._docTypeService.Setup(x => x.GetDocTypes()).Returns(new List<DocType>());
      this._appManCoEmailService.Setup(x => x.GetAppManCoEmail(It.IsAny<int>())).Returns(new AppManCoEmail());

      var result = _controller.Edit(It.IsAny<int>());
      var viewResult = result as ViewResult;
      result.Should().BeOfType<ViewResult>();
      viewResult.Model.Should().BeOfType<EditAppManCoEmailViewModel>();

      this._applicationService.Verify(x => x.GetApplications(), Times.AtLeastOnce);
      this._manCoService.Verify(x => x.GetManCos(), Times.AtLeastOnce);
      this._docTypeService.Verify(x => x.GetDocTypes(), Times.AtLeastOnce);
      this._appManCoEmailService.Verify(x => x.GetAppManCoEmail(It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenOtfController_WhenYouNeedToPostAnEdit_AndTheModelIsValid_ThenIndexViewIsReturned()
    {
      this._appManCoEmailService.Setup(x => x.UpdateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
      this._userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser() {  UserName = "userName"});

      var result = _controller.Edit(new EditAppManCoEmailViewModel());
      result.Should().BeOfType<JsonResult>();

      this._appManCoEmailService.Verify(x => x.UpdateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenOtfController_WhenYouNeedToPostAnEdit_AndTheModelIsNotValid_ThenEditViewIsReturned()
    {
      _controller.ModelState.AddModelError("Email", "Email is required");

      var result = _controller.Edit(new EditAppManCoEmailViewModel());
      var redirectResult = result as RedirectResult;
      redirectResult.Should().BeOfType<RedirectResult>();
      redirectResult.Url.Should().BeEquivalentTo(ExpectedRefererUrl);
    }

    [Test]
    public void GivenOtfController_WhenYouNeedToPostACreate_AndTheModelIsValid_ThenIndexViewIsReturned()
    {
      this._appManCoEmailService.Setup(x => x.CreateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()));

      var result = _controller.Create(new AddAppManCoEmailViewModel());
      var redirectToRouteResult = result as RedirectToRouteResult;
      result.Should().BeOfType<RedirectToRouteResult>();
      redirectToRouteResult.RouteValues["action"].Should().Be("Index");

      this._appManCoEmailService.Verify(x => x.CreateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenOtfController_WhenYouNeedToPostACreate_AndTheModelIsNotValid_ThenEditViewIsReturned()
    {
      _controller.ModelState.AddModelError("Email", "Email is required");

      var result = _controller.Create(new AddAppManCoEmailViewModel());
      var redirectResult = result as RedirectResult;
      redirectResult.Should().BeOfType<RedirectResult>();
      redirectResult.Url.Should().BeEquivalentTo(ExpectedRefererUrl);
    }

    [Test]
    public void GivenOtfController_WhenIDeleteAnOTF_AJsonResultIsReturned()
    {
      var result = this._controller.Delete(It.IsAny<int>());

      result.Should().NotBeNull();
      result.Should().BeOfType<JsonResult>();
    }
  }
}
