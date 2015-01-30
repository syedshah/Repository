namespace UnityWebTests.Controllers
{
  using System.Collections.Generic;
  using System.Web.Mvc;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.Approval;
  using UnityWeb.Models.Shared;

  [TestFixture]
  public class ApprovalControllerTests : ControllerTestsBase
  {
    private Mock<IApprovalService> _approvalService;
    private Mock<IDocumentService> _documentService;
    private Mock<ILogger> _logger; 
    private ApprovalController _approvalController;
    private ApproveDocumentsViewModel _approveDocumentsViewModel;

    [SetUp]
    public void SetUp()
    {
      _approvalService = new Mock<IApprovalService>();
      _documentService = new Mock<IDocumentService>();
      _logger = new Mock<ILogger>();
      _approvalController = new ApprovalController(_approvalService.Object, _documentService.Object, _logger.Object);

      _approveDocumentsViewModel = new ApproveDocumentsViewModel
      {
        ApproveDocumentViewModel = new List<ApproveDocumentViewModel>
                                                                    {
                                                                      new ApproveDocumentViewModel
                                                                        {
                                                                          DocumentId = "guid1",
                                                                          Selected = true
                                                                        },
                                                                        new ApproveDocumentViewModel
                                                                        {
                                                                          DocumentId = "guid2",
                                                                          Selected = false
                                                                        }
                                                                    },
                                                                    Grid = "grid",
                                                                    Page = "1"
      };

      SetControllerContext(_approvalController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveADocument_AndTheDocumentIsCheckedOutByAnotherUser_IGetTheCorrectViewReturned()
    {
      _approvalService.Setup(
        a =>
        a.ApproveDocument(
        It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()));

      var result = (PartialViewResult)_approvalController.Document(_approveDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(1);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveADocument_AndTheDocumentIsAlreadyApproved_IGetTheCorrectViewReturned()
    {
      _approvalService.Setup(
        a =>
        a.ApproveDocument(
        It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyApprovedException>();

      var result = (PartialViewResult)_approvalController.Document(_approveDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be("guid1");
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveADocument_AndTheDocumentIsAlreadyRejected_IGetTheCorrectViewReturned()
    {
      _approvalService.Setup(
        a =>
        a.ApproveDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyRejectedException>();

      var result = (PartialViewResult)_approvalController.Document(_approveDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be("guid1");
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveADocument_IGetTheCorrectViewReturned()
    {
      _approvalService.Setup(
        a =>
        a.ApproveDocument(
        It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()));

      var result = (PartialViewResult)_approvalController.Document(_approveDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(1);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveAGrid_AndTheGridHasDocumentsThatAreCheckedOutByAnotherUser_IGetTheCorrectViewReturned()
    {
      _documentService.Setup(d => d.GetDocuments("grid")).Returns(new List<Document>()
                                                                    {
                                                                      new Document()
                                                                        {
                                                                          DocType = new DocType(),
                                                                          SubDocType = new SubDocType(),
                                                                          ManCo = new ManCo(),
                                                                          DocumentId = "guid1"
                                                                        }
                                                                    });

      _approvalService.Setup(
        a =>
        a.ApproveDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()));

      var result = (PartialViewResult)_approvalController.Grid("grid");
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyCheckedOut.Should().Be(null);
      model.DocumentsApproved.Should().Be(1);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveAGrid_AndTheGridContaintsADocumentThatIsAlreadyApproved_IGetTheCorrectViewReturned()
    {
      _documentService.Setup(d => d.GetDocuments("grid")).Returns(new List<Document>()
                                                                    {
                                                                      new Document()
                                                                        {
                                                                          DocType = new DocType(),
                                                                          SubDocType = new SubDocType(),
                                                                          ManCo = new ManCo(),
                                                                          DocumentId = "guid1"
                                                                        }
                                                                    });

      _approvalService.Setup(
        a =>
        a.ApproveDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyApprovedException>();

      var result = (PartialViewResult)_approvalController.Grid("grid");
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsAlreadyApproved.Should().Be("guid1");
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveAGrid_AndTheGridContaintsADocumentThatIsAlreadyRejected_IGetTheCorrectViewReturned()
    {
      _documentService.Setup(d => d.GetDocuments("grid")).Returns(new List<Document>()
                                                                    {
                                                                      new Document()
                                                                        {
                                                                          DocType = new DocType(),
                                                                          SubDocType = new SubDocType(),
                                                                          ManCo = new ManCo(),
                                                                          DocumentId = "guid1"
                                                                        }
                                                                    });

      _approvalService.Setup(
        a =>
        a.ApproveDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyRejectedException>();

      var result = (PartialViewResult)_approvalController.Grid("grid");
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be("guid1");
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveAGrid_IGetTheCorrectViewReturned()
    {
      _documentService.Setup(d => d.GetDocuments("grid")).Returns(new List<Document>()
                                                                    {
                                                                      new Document()
                                                                        {
                                                                          DocType = new DocType(),
                                                                          SubDocType = new SubDocType(),
                                                                          ManCo = new ManCo(),
                                                                          DocumentId = "guid1"
                                                                        }
                                                                    });

      _approvalService.Setup(
        a =>
        a.ApproveDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()));

      var result = (PartialViewResult)_approvalController.Grid("grid");
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(1);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveABasket_AndTheBasketHasDocumentsThatAreCheckedOutByAnotherUser_IGetTheCorrectViewReturned()
    {
      _approvalService.Setup(
        a =>
        a.ApproveDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()));

      var result = (PartialViewResult)_approvalController.Basket(_approveDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(2);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveABasket_AndTheGridContaintsADocumentThatIsAlreadyApproved_IGetTheCorrectViewReturned()
    {
      _approvalService.Setup(
        a =>
        a.ApproveDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyApprovedException>();

      var result = (PartialViewResult)_approvalController.Basket(_approveDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsAlreadyApproved.Should().Be("guid1");
      model.DocumentsApproved.Should().Be(1);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveABasket_AndTheGridContaintsADocumentThatIsAlreadyRejected_IGetTheCorrectViewReturned()
    {
      _approvalService.Setup(
        a =>
        a.ApproveDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyRejectedException>();

      var result = (PartialViewResult)_approvalController.Basket(_approveDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be("guid1");
      model.DocumentsApproved.Should().Be(1);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenAnApprovalcontroller_WhenITryToApproveABasket_IGetTheCorrectViewReturned()
    {
      _approvalService.Setup(
        a =>
        a.ApproveDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()));

      var result = (PartialViewResult)_approvalController.Basket(_approveDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(2);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }
  }
}
