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
  using UnityWeb.Models.Rejection;
  using UnityWeb.Models.Shared;

  [TestFixture]
  public class RejectControllerTests : ControllerTestsBase
  {
    private Mock<IRejectionService> _rejectionService;
    private Mock<IDocumentService> _documentService;
    private Mock<ILogger> _logger; 
    private RejectController _rejectController;
    private RejectDocumentsViewModel _rejectDocumentsViewModel;

    [SetUp]
    public void SetUp()
    {
      _rejectionService = new Mock<IRejectionService>();
      _documentService = new Mock<IDocumentService>();
      _logger = new Mock<ILogger>();
      _rejectController = new RejectController(_rejectionService.Object, _documentService.Object, _logger.Object);

      _rejectDocumentsViewModel = new RejectDocumentsViewModel
      {
        RejectDocumentViewModel = new List<RejectDocumentViewModel>
                                                                    {
                                                                      new RejectDocumentViewModel
                                                                        {
                                                                          DocumentId = "guid1",
                                                                          Selected = true
                                                                        },
                                                                        new RejectDocumentViewModel
                                                                        {
                                                                          DocumentId = "guid2",
                                                                          Selected = false
                                                                        }
                                                                    },
                                                                    Grid = "grid",
                                                                    Page = "1"
      };

      SetControllerContext(_rejectController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectADocument_AndTheDocumentIsCheckedOutByAnotherUser_IGetTheCorrectViewReturned()
    {
      _rejectionService.Setup(
         a =>
         a.RejectDocument(
           It.IsAny<string>(),
           "guid1",
           It.IsAny<string>(),
           It.IsAny<string>(),
           It.IsAny<string>()));

      var result = (PartialViewResult)_rejectController.Document(_rejectDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(1);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectADocument_AndTheDocumentIsAlreadyRejected_IGetTheCorrectViewReturned()
    {
      _rejectionService.Setup(
        a =>
        a.RejectDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyRejectedException>();

      var result = (PartialViewResult)_rejectController.Document(_rejectDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyRejected.Should().Be("guid1");
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectADocument_AndTheDocumentIsAlreadyApproved_IGetTheCorrectViewReturned()
    {
      _rejectionService.Setup(
        a =>
        a.RejectDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyApprovedException>();

      var result = (PartialViewResult)_rejectController.Document(_rejectDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsAlreadyApproved.Should().Be("guid1");
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectADocument_IGetTheCorrectViewReturned()
    {
      _rejectionService.Setup(
        a =>
        a.RejectDocument(
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()));

      var result = (PartialViewResult)_rejectController.Document(_rejectDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(1);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectAGrid_AndTheGridHasDocumentsThatAreCheckedOutByAnotherUser_IGetTheCorrectViewReturned()
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

      _rejectionService.Setup(
         a =>
         a.RejectDocument(
           It.IsAny<string>(),
           "guid1",
           It.IsAny<string>(),
           It.IsAny<string>(),
           It.IsAny<string>()));

      var result = (PartialViewResult)_rejectController.Grid("grid");
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyCheckedOut.Should().Be(null);
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(1);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectAGrid_AndTheGridContaintsADocumentThatAreAlreadyRejected_IGetTheCorrectViewReturned()
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

      _rejectionService.Setup(
        a =>
        a.RejectDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyRejectedException>();

      var result = (PartialViewResult)_rejectController.Grid("grid");
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be("guid1");
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectAGrid_AndTheGridContaintsADocumentThatIsAlreadyApproved_IGetTheCorrectViewReturned()
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

      _rejectionService.Setup(
        a =>
        a.RejectDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyApprovedException>();

      var result = (PartialViewResult)_rejectController.Grid("grid");
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsAlreadyApproved.Should().Be("guid1");
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(0);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectAGrid_IGetTheCorrectViewReturned()
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

      _rejectionService.Setup(
        a =>
        a.RejectDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()));

      var result = (PartialViewResult)_rejectController.Grid("grid");
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(1);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectABasket_AndTheBasketHasDocumentsThatAreCheckedOutByAnotherUser_IGetTheCorrectViewReturned()
    {
      _rejectionService.Setup(
         a =>
         a.RejectDocument(
           It.IsAny<string>(),
           "guid1",
           It.IsAny<string>(),
           It.IsAny<string>(),
           It.IsAny<string>()));

      var result = (PartialViewResult)_rejectController.Basket(_rejectDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(2);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectABasket_AndTheGridContaintsADocumentThatAreAlreadyApproved_IGetTheCorrectViewReturned()
    {
      _rejectionService.Setup(
        a =>
        a.RejectDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyApprovedException>();

      var result = (PartialViewResult)_rejectController.Basket(_rejectDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be("guid1");
      model.DocumentsAlreadyRejected.Should().Be(string.Empty);
      model.DocumentsRejected.Should().Be(1);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectABasket_AndTheGridContaintsADocumentThatIsAlreadyRejected_IGetTheCorrectViewReturned()
    {
      _rejectionService.Setup(
        a =>
        a.RejectDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<DocumentAlreadyRejectedException>();

      var result = (PartialViewResult)_rejectController.Basket(_rejectDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsAlreadyRejected.Should().Be("guid1");
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(1);
      result.ViewName.Should().Be("_DocumentWarnings");
    }

    [Test]
    public void GivenARejectController_WhenITryToRejectABasket_IGetTheCorrectViewReturned()
    {
      _rejectionService.Setup(
        a =>
        a.RejectDocument(
          It.IsAny<string>(),
          "guid1",
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()));

      var result = (PartialViewResult)_rejectController.Basket(_rejectDocumentsViewModel);
      var model = (DocumentWarningsViewModel)result.Model;
      model.DocumentsCheckedOutByOtherUser.Should().Be(null);
      model.DocumentsAlreadyApproved.Should().Be(string.Empty);
      model.DocumentsApproved.Should().Be(0);
      model.DocumentsRejected.Should().Be(2);
      result.ViewName.Should().Be("_DocumentWarnings");
    }
  }
}
