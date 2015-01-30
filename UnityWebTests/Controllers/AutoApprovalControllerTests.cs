namespace UnityWebTests.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.AutoApproval;

  [TestFixture]
  public class AutoApprovalControllerTests : ControllerTestsBase
  {
    private Mock<IAutoApprovalService> _autoApprovalService;

    private Mock<IDocTypeService> _docTypeService;

    private Mock<ISubDocTypeService> _subDocTypeService;

    private Mock<IManCoService> _manCoService;

    private Mock<IUserService> _userService;

    private Mock<ILogger> _logger;

    private AutoApprovalController _controller;

    private List<AutoApproval> _approvals;

    private List<DocType> _docTypes;

    private List<SubDocType> _subDocTypes;

    private List<ManCo> _manCos;

    private AutoApproval _approval1;

    private AutoApproval _approval2;

    private AutoApproval _approval3;

    private AutoApproval _approval4;

    private AutoApproval _approval5;

    private DocType _docType1;

    private DocType _docType2;

    private DocType _docType3;

    private SubDocType _subDocType1;

    private SubDocType _subDocType2;

    private SubDocType _subDocType3;

    private SubDocType _subDocType4;

    private SubDocType _subDocType5;

    private SubDocType _subDocType6;

    private SubDocType _subDocType7;

    private SubDocType _subDocType8;

    private ManCo _manCo1;

    private ManCo _manCo2;

    private ManCo _manCo3;

    private const string ExpectedRefererUrl = "value";

    [SetUp]
    public void SetUp()
    {
      var headers = new FormCollection { { "Referer", ExpectedRefererUrl } };
      MockRequest.Setup(r => r.Headers).Returns(headers);

      this._autoApprovalService = new Mock<IAutoApprovalService>();
      this._docTypeService = new Mock<IDocTypeService>();
      this._subDocTypeService = new Mock<ISubDocTypeService>();
      this._manCoService = new Mock<IManCoService>();
      this._userService = new Mock<IUserService>();
      this._logger = new Mock<ILogger>();

      this.SetUpVariables();

      _controller = new AutoApprovalController(
        this._autoApprovalService.Object,
        this._docTypeService.Object,
        this._subDocTypeService.Object,
        this._manCoService.Object,
        this._userService.Object,
        this._logger.Object);

      SetControllerContext(_controller);
    }

    [Test]
    public void GivenADocumentApprovalController_WhenTheDocumentApprovalsActionIsAccessed_ThenTheCorrectModelIsReturned()
    {
      this.SetUpServiceGetCalls();
      this._autoApprovalService.Setup(x => x.GetAutoApprovals(1)).Returns(_approvals);

      var result = (PartialViewResult)_controller.AutoApprovals(1);
      var model = (AutoApprovalsViewModel)result.Model;

      model.AddAutoApprovalViewModel.Should().NotBeNull();
      result.ViewName.Should().Be("_ShowAutoApprovals");
    }

    [Test]
    public void GivenADocumentApprovalController_WhenTheDocumentApprovalsActionIsAccessed_ThenTheCorrectValuesInTheViewModelAreReturned()
    {
      this.SetUpServiceGetCalls();
      this._autoApprovalService.Setup(x => x.GetAutoApprovals(1)).Returns(_approvals);

      var result = (PartialViewResult)_controller.AutoApprovals(1);
      var model = (AutoApprovalsViewModel)result.Model;

      model.AutoApprovals.Should().HaveCount(3);
      model.AutoApprovals.Should()
           .Contain(d => d.DocTypeViewModel.Code == "Code1" && d.SubDocTypeViewModel.Code == "All")
           .And.Contain(d => d.DocTypeViewModel.Code == "Code2" && d.SubDocTypeViewModel.Code == "SubCode4")
           .And.Contain(d => d.DocTypeViewModel.Code == "Code3" && d.SubDocTypeViewModel.Code == "SubCode7");
    }

    [Test]
    public void GivenADocumentApprovalController_WhenTheIndexPageIsAccessed_ThenTheIndexViewModelIsReturned()
    {
      this.SetUpServiceGetCalls();

      var result = _controller.Index();
      result.Should().BeOfType<ViewResult>();

      this._manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void GivenADocumentApprovalController_WhenTheIndexPageIsAccessed_ThenTheIndexViewShouldContainTheModel()
    {
      this.SetUpServiceGetCalls();

      var result = (ViewResult)_controller.Index();
      result.Model.Should().BeOfType<ManCosApprovalViewModel>();

      this._manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void
      GivenADocumentApprovalController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfDocumentApprovals
      ()
    {
      this.SetUpServiceGetCalls();

      var result = _controller.Index() as ViewResult;

      var model = result.Model as ManCosApprovalViewModel;

      model.ManCos.Should().HaveCount(3);

      this._manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void GivenAnAutoApprovalController_WhenICallItsCreateMethod_AndThereIsAModelError_TheUserIsNotified()
    {
      _controller.ModelState.AddModelError("error", "message");

      var result = (RedirectResult)_controller.Create(new AddAutoApprovalViewModel());

      _controller.TempData.Should().NotBeNull();
      _controller.TempData["comment"].Should().Be("Required files are missing");
    }

    [Test]
    public void GivenAValidModel_WhenAllSubTypesAreSelected_AutoApprovalsForAllSubDocTypesAreCreated()
    {
      this._subDocTypeService.Setup(x => x.GetSubDocTypes(2)).Returns(_subDocTypes);
      this._autoApprovalService.Setup(x => x.AddAutoApproval(1, 2, It.IsAny<int>()));

      var result = (RedirectResult)_controller.Create(new AddAutoApprovalViewModel
                                                        {
                                                          SubDocTypeId = -1,
                                                          ManCoId = 1,
                                                          DocTypeId = 2
                                                        });

      _autoApprovalService.Verify(a => a.AddAutoApproval(1, 2, It.IsAny<int>()), Times.Exactly(8));

      var redirectResult = result as RedirectResult;
      redirectResult.Should().BeOfType<RedirectResult>();
      redirectResult.Url.Should().BeEquivalentTo(ExpectedRefererUrl);
    }

    [Test]
    public void GivenAValidModel_WhenASingleSubTypeIsSelected_AnAutoApprovalAlTheSubDocTypeIsCreated()
    {
      var result = (RedirectResult)_controller.Create(new AddAutoApprovalViewModel
      {
        SubDocTypeId = 1,
        ManCoId = 2,
        DocTypeId = 3
      });
      
      this._autoApprovalService.Setup(x => x.AddAutoApproval(2, 3, 1));
      
      _autoApprovalService.Verify(a => a.AddAutoApproval(2, 3, 1), Times.Once);

      var redirectResult = result as RedirectResult;
      redirectResult.Should().BeOfType<RedirectResult>();
      redirectResult.Url.Should().BeEquivalentTo(ExpectedRefererUrl);
    }

    [Test]
    public void GivenAValidModel_WhenASingleAutoApprovalIsDeleted_TheAutoApprovalIsDeleted()
    {
      this._autoApprovalService.Setup(x => x.Delete(1));
      var result = (RedirectToRouteResult)_controller.Delete(1, "2", 3);

      _autoApprovalService.Verify(a => a.Delete(1), Times.Once);
      _controller.TempData["SelectedManCoId"].Should().Be(3);

      result.RouteValues["controller"].Should().Be("AutoApproval");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void GivenAValidModel_WhenAllAutoApprovalsAreDeltedByDocTypeCode_TheAutoApprovalsForTheDocTypeAreDeleted()
    {
      this._autoApprovalService.Setup(x => x.Delete("2"));
      var result = (RedirectToRouteResult)_controller.Delete(-1, "2", 3);

      _autoApprovalService.Verify(a => a.Delete("2"), Times.Once);

      result.RouteValues["controller"].Should().Be("AutoApproval");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void GivenAValidModel_WhenASingleSubTypeIsSelected_AndTheAutoApprovalAlreadyExists_ErrorMessageIsDisplayed()
    {
      this._autoApprovalService.Setup(x => x.AddAutoApproval(2, 3, 1)).Throws<UnityAutoApprovalAlreadyExistsException>();

      var result = (RedirectResult)_controller.Create(new AddAutoApprovalViewModel
      {
        SubDocTypeId = 1,
        ManCoId = 2,
        DocTypeId = 3
      });

      _controller.TempData["comment"].Should().Be("Auto approval already exists");

      var redirectResult = result as RedirectResult;
      redirectResult.Should().BeOfType<RedirectResult>();
      redirectResult.Url.Should().BeEquivalentTo(ExpectedRefererUrl);
    }
    
    [Test]
    public void GivenAValidAutoApproval_WhenITryAndEditAnAutoApproval_ThenIGetTheCorrectView()
    {
      this._autoApprovalService.Setup(x => x.GetAutoApproval(It.IsAny<int>())).Returns(_approval1);

      this._docTypeService.Setup(x => x.GetDocTypes()).Returns(_docTypes);
      this._subDocTypeService.Setup(x => x.GetSubDocTypes()).Returns(_subDocTypes);
      this._manCoService.Setup(x => x.GetManCos()).Returns(_manCos);

      ActionResult result = _controller.Edit(1, "code");
      result.Should().BeOfType<ViewResult>();

      this._autoApprovalService.Verify(x => x.GetAutoApproval(1), Times.Once());
      this._docTypeService.Verify(x => x.GetDocTypes(), Times.Once());
      this._subDocTypeService.Verify(x => x.GetSubDocTypes(), Times.Once());
      this._manCoService.Verify(x => x.GetManCos(), Times.Once());
    }

    [Test]
    public void GivenAllAutoApprovalsPerDocType_WhenITryAndEditTheAutoApprovals_ThenIGetTheCorrectView()
    {
      this._autoApprovalService.Setup(x => x.GetAutoApproval(It.IsAny<int>())).Returns(_approval1);

      this._docTypeService.Setup(x => x.GetDocTypes()).Returns(_docTypes);
      this._subDocTypeService.Setup(x => x.GetSubDocTypes()).Returns(_subDocTypes);
      this._manCoService.Setup(x => x.GetManCos()).Returns(_manCos);
      this._autoApprovalService.Setup(x => x.GetAutoApprovals("code"))
          .Returns(new List<AutoApproval>() { new AutoApproval { ManCoId = 1, DocTypeId = 2 } });

      ActionResult result = _controller.Edit(-1, "code");
      result.Should().BeOfType<ViewResult>();

      this._autoApprovalService.Verify(x => x.GetAutoApprovals("code"), Times.Once());
      this._docTypeService.Verify(x => x.GetDocTypes(), Times.Once());
      this._subDocTypeService.Verify(x => x.GetSubDocTypes(), Times.Once());
      this._manCoService.Verify(x => x.GetManCos(), Times.Once());
    }

    [Test]
    public void GivenAnAutoApprovalController_WhenICallItsUpdateMethod_AndThereIsAModelError_TheUserIsNotified()
    {
      _controller.ModelState.AddModelError("error", "message");

      var result = (RedirectToRouteResult)_controller.Update(new EditAutoApprovalViewModel());

      _controller.TempData.Should().NotBeNull();
      var errorList = _controller.ModelState.Values.SelectMany(x => x.Errors).ToList();
      _controller.TempData["comment"].Should().Be("Please correct the errors and try again");
      result.RouteValues["controller"].Should().Be("AutoApproval");
      result.RouteValues["action"].Should().Be("Edit");
    }

    [Test]
    public void GivenAllSubDocTypes_WhenITryToUpdateASingleAutoApprovals_TheSingleAutoApprovalsIsAdded()
    {
      var result = (RedirectToRouteResult)_controller.Update(new EditAutoApprovalViewModel()
                                                        {
                                                          AutoApprovalId = -1,
                                                          DocTypeCode = "code",
                                                          ManCoId = 2,
                                                          DocTypeId = 3,
                                                          SubDocTypeId = 4
                                                        });

      _controller.TempData["SelectedManCoId"].Should().Be(2);

      _autoApprovalService.Verify(a => a.Delete("code"), Times.Once);
      _autoApprovalService.Verify(a => a.AddAutoApproval(2, 3, 4), Times.Once);

      result.RouteValues["controller"].Should().Be("AutoApproval");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void GivenAllSubDocTypes_WhenITryToUpdateASingleAutoApprovals_AndTheAutoApprovalAlreadyExists_TheCorrectErrorMessageIsShown()
    {
      this._autoApprovalService.Setup(x => x.AddAutoApproval(2, 3, 4)).Throws<UnityAutoApprovalAlreadyExistsException>();

      var result = (RedirectToRouteResult)_controller.Update(new EditAutoApprovalViewModel()
                                                        {
                                                          AutoApprovalId = -1,
                                                          DocTypeCode = "code",
                                                          ManCoId = 2,
                                                          DocTypeId = 3,
                                                          SubDocTypeId = 4
                                                        });

      _controller.TempData["SelectedManCoId"].Should().Be(2);

      _controller.TempData["comment"].Should().Be("Auto approval already exists");
    }

    [Test]
    public void GivenSingleSubDocType_WhenITryToUpdateAllAutoApprovals_AllAutoApprovalsAreAdded()
    {
      this._subDocTypeService.Setup(x => x.GetSubDocTypes(3)).Returns(_subDocTypes);

      var result = (RedirectToRouteResult)_controller.Update(new EditAutoApprovalViewModel()
      {
        AutoApprovalId = 1,
        DocTypeCode = "code",
        ManCoId = 2,
        DocTypeId = 3,
        SubDocTypeId = -1
      });

      _controller.TempData["SelectedManCoId"].Should().Be(2);

      _autoApprovalService.Verify(a => a.AddAutoApproval(2, 3, It.IsAny<int>()), Times.Exactly(8));

      result.RouteValues["controller"].Should().Be("AutoApproval");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void GivenSingleSubDocType_WhenITryToUpdateToAnotherSiingleAutoApprovals_TheApprovalIsUpdated()
    {
      var result = (RedirectToRouteResult)_controller.Update(new EditAutoApprovalViewModel()
      {
        AutoApprovalId = 1,
        DocTypeCode = "code",
        ManCoId = 2,
        DocTypeId = 3,
        SubDocTypeId = 4
      });

      _controller.TempData["SelectedManCoId"].Should().Be(2);

      _autoApprovalService.Verify(a => a.Update(1, 2, 3, 4), Times.Once());

      result.RouteValues["controller"].Should().Be("AutoApproval");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void
      GivenAValidDocumentApproval_WhenITryAndUpdateADocumentApproval_AndModelStateIsInValid_ThenIGetTheCorrectView()
    {

      _controller.ModelState.AddModelError("DocTypeId", "DocTypeId is required");
      var result = _controller.Update(new EditAutoApprovalViewModel { AutoApprovalId = 1, ManCoId = 6 });

      result.Should().BeOfType<RedirectToRouteResult>();

      var redirectToRouteResult = result as RedirectToRouteResult;
      result.Should().NotBeNull();
      redirectToRouteResult.RouteValues["controller"].Should().Be("AutoApproval");
      redirectToRouteResult.RouteValues["action"].Should().Be("Edit");
    }

    private void SetUpVariables()
    {
      _manCo1 = new ManCo { Id = 1 };
      _manCo2 = new ManCo { Id = 2 };
      _manCo3 = new ManCo { Id = 3 };

      this._manCos = new List<ManCo>();

      this._manCos.Add(_manCo1);
      this._manCos.Add(_manCo2);
      this._manCos.Add(_manCo3);

      _docType1 = new DocType { Id = 1, Code = "Code1" };
      _docType2 = new DocType { Id = 2, Code = "Code2" };
      _docType3 = new DocType { Id = 3, Code = "Code3" };

      this._docTypes = new List<DocType>();

      this._docTypes.Add(_docType1);
      this._docTypes.Add(_docType2);
      this._docTypes.Add(_docType3);

      this._subDocType1 = new SubDocType { Id = 1, DocType = _docType1, Code = "SubCode1"};
      this._subDocType2 = new SubDocType { Id = 2, DocType = _docType1, Code = "subCode2" };
      this._subDocType3 = new SubDocType { Id = 3, DocType = _docType1, Code = "SubCode3" };
      this._subDocType4 = new SubDocType { Id = 4, DocType = _docType2, Code = "SubCode4" };
      this._subDocType5 = new SubDocType { Id = 5, DocType = _docType2, Code = "SubCode5" };
      this._subDocType6 = new SubDocType { Id = 6, DocType = _docType3, Code = "SubCode6" };
      this._subDocType7 = new SubDocType { Id = 7, DocType = _docType3, Code = "SubCode7" };
      this._subDocType8 = new SubDocType { Id = 8, DocType = _docType3, Code = "SubCode8" };

      this._subDocTypes = new List<SubDocType>();

      this._subDocTypes.Add(_subDocType1);
      this._subDocTypes.Add(_subDocType2);
      this._subDocTypes.Add(_subDocType3);
      this._subDocTypes.Add(_subDocType4);
      this._subDocTypes.Add(_subDocType5);
      this._subDocTypes.Add(_subDocType6);
      this._subDocTypes.Add(_subDocType7);
      this._subDocTypes.Add(_subDocType8);

      this._approval1 = new AutoApproval { Id = 1, DocType = _docType1, SubDocType = _subDocType1, Manco = _manCo1 };
      this._approval2 = new AutoApproval { Id = 2, DocType = _docType1, SubDocType = _subDocType2, Manco = _manCo1 };
      this._approval3 = new AutoApproval { Id = 3, DocType = _docType1, SubDocType = _subDocType3, Manco = _manCo1 };

      this._approval4 = new AutoApproval { Id = 4, DocType = _docType2, SubDocType = _subDocType4, Manco = _manCo1 };
      this._approval5 = new AutoApproval { Id = 5, DocType = _docType3, SubDocType = _subDocType7, Manco = _manCo1 };
      this._approvals = new List<AutoApproval>();

      this._approvals.Add(_approval1);
      this._approvals.Add(_approval2);
      this._approvals.Add(_approval3);
      this._approvals.Add(_approval4);
      this._approvals.Add(_approval5);
    }

    private void SetUpServiceGetCalls()
    {
      this._autoApprovalService.Setup(x => x.GetAutoApprovals()).Returns(_approvals);
      this._docTypeService.Setup(x => x.GetDocTypes()).Returns(_docTypes);
      this._subDocTypeService.Setup(x => x.GetSubDocTypes()).Returns(_subDocTypes);
      this._manCoService.Setup(x => x.GetManCos()).Returns(_manCos);
      this._userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());
      _manCoService.Setup(m => m.GetManCosByUserId(It.IsAny<string>())).Returns(_manCos);
    }
  }
}
