namespace UnityWebTests.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using ClientProxies.ArchiveServiceReference;
  using Entities;
  using Entities.File;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.Document;
  using UnityWeb.Models.Search;
  using UnityWebTests.Helpers;

  [TestFixture]
  public class DocumentControllerTests : ControllerTestsBase
  {
    private Mock<IDocumentService> _documentService;
    private Mock<IUserService> _userService;
    private Mock<IManCoService> _manCoService;
    private Mock<ILogger> _logger;
    private DocumentController _controller;
    private const string ExpectedRefererUrl = "value";

    [SetUp]
    public void SetUp()
    {
      var headers = new FormCollection { { "Referer", ExpectedRefererUrl } };
      MockRequest.Setup(r => r.Headers).Returns(headers);

      _documentService = new Mock<IDocumentService>();
      _userService = new Mock<IUserService>();
      _manCoService = new Mock<IManCoService>();
      _logger = new Mock<ILogger>();

      _documentService.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(new Document());
    }

    private readonly PagedResult<IndexedDocumentData> _documentsIndexed = new PagedResult<IndexedDocumentData>
                                                                           {
                                                                             CurrentPage = 1,
                                                                             ItemsPerPage = 10,
                                                                             TotalItems = 50,
                                                                             Results = new[]
                                                                                 {
                                                                                   new IndexedDocumentData()
                                                                                     {
                                                                                       MappedIndexes = new List<IndexMapped>{}
                                                                                     },
                                                                                   new IndexedDocumentData()
                                                                                     {
                                                                                       MappedIndexes = new List<IndexMapped>{}
                                                                                     },
                                                                                   new IndexedDocumentData()
                                                                                     {
                                                                                       MappedIndexes = new List<IndexMapped>{}
                                                                                     }
                                                                                 }
                                                                           };

    private readonly PagedResult<IndexedDocumentData> _tooManyDocuments = new PagedResult<IndexedDocumentData>
                                                                            {
                                                                              CurrentPage = 1,
                                                                              ItemsPerPage = 10,
                                                                              TotalItems = 6000,
                                                                              Results = new List<IndexedDocumentData> {}
                                                                            };


    [Test]
    public void GivenADocumentController_WhenICallItsSearchGridMethodForPageTwo_ThenItRetrunsTheCorrectNumberOfDocuments()
    {
      PagedResult<IndexedDocumentData> documentsPageTwo = new PagedResult<IndexedDocumentData>
                                                                           {
                                                                             CurrentPage = 2,
                                                                             ItemsPerPage = 10,
                                                                             TotalItems = 50,
                                                                             Results = new[]
                                                                                 {
                                                                                   new IndexedDocumentData()
                                                                                     {
                                                                                       MappedIndexes = new List<IndexMapped>{}
                                                                                     },
                                                                                   new IndexedDocumentData()
                                                                                     {
                                                                                       MappedIndexes = new List<IndexMapped>{}
                                                                                     },
                                                                                   new IndexedDocumentData()
                                                                                     {
                                                                                       MappedIndexes = new List<IndexMapped>{}
                                                                                     }
                                                                                 }
                                                                           };

      var controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocuments(2, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(documentsPageTwo);
      _documentService.Setup(d => d.GetDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetUnApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });

      var result = (ViewResult)controller.SearchGrid(It.IsAny<string>(), 2);
      var model = (DocumentsViewModel)result.Model;

      int documentCount = (from s in documentsPageTwo.Results select s).Count();
      model.Documents.Should().HaveCount(documentCount);
      model.PagingInfo.CurrentPage = 2;
    }
    
    [Test]
    public void GivenADocumentController_WhenICallItsSearchGridMethod_ThenItRetrunsTheCorrectNumberOfDocuments()
    {
      var controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocuments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(_documentsIndexed);
      _documentService.Setup(d => d.GetDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetUnApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });

      var result = (ViewResult)controller.SearchGrid(It.IsAny<string>());
      var model = (DocumentsViewModel)result.Model;

      int documentCount = (from s in _documentsIndexed.Results select s).Count();
      model.Documents.Should().HaveCount(documentCount);
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchGridMethod_AndTheDocumentsAreCheckedOut_ThenTheDocumentsAreCheckedOut()
    {
      var controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocuments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(_documentsIndexed);
      _documentService.Setup(d => d.GetDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetUnApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });

      DateTime checkOutDate = DateTime.Now;

      _documentService.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(new Document
                                                                               {
                                                                                 CheckOut = new CheckOut
                                                                                              {
                                                                                                CheckOutBy = "user",
                                                                                                CheckOutDate = checkOutDate
                                                                                              }
                                                                               });

      var result = (ViewResult)controller.SearchGrid(It.IsAny<string>());
      var model = (DocumentsViewModel)result.Model;

      int documentCount = (from s in _documentsIndexed.Results select s).Count();
      model.Documents.Should().HaveCount(documentCount);
      model.Documents.Should()
           .Contain(d => d.CheckedOut == true && d.CheckedOutBy == "user" && d.CheckedOutDate == checkOutDate)
           .And.Contain(d => d.ApprovalStatus == "Unapproved");
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchGridMethod_AndTheDocumentsAreApproved_ThenTheDocumentsAreApproved()
    {
      var controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocuments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(_documentsIndexed);

      _documentService.Setup(d => d.GetDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetUnApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });


      DateTime approvalDate = DateTime.Now;

      _documentService.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(new Document
      {
        Approval = new Approval()
        {
          ApprovedBy = "user",
          ApprovedDate = approvalDate
        }
      });

      var result = (ViewResult)controller.SearchGrid(It.IsAny<string>());
      var model = (DocumentsViewModel)result.Model;

      int documentCount = (from s in _documentsIndexed.Results select s).Count();
      model.Documents.Should().HaveCount(documentCount);
      model.Documents.Should()
           .Contain(d => d.ApprovedBy == "user" && d.ApprovedDate == approvalDate && d.ApprovalStatus == "Approved");
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchGridMethod_AndTheDocumentsAreNotInUnity_ThenTheCorrectNumberOfDocumentsAreReturned()
    {
      var controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocuments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(_documentsIndexed);
      _documentService.Setup(d => d.GetDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetUnApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });

      DateTime approvalDate = DateTime.Now;
      Document document = new Document{ ManCo = new ManCo("code", "decsription")};

      _documentService.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(document);
      

      var result = (ViewResult)controller.SearchGrid(It.IsAny<string>());
      var model = (DocumentsViewModel)result.Model;

      int documentCount = (from s in _documentsIndexed.Results select s).Count();
      model.Documents.Should().HaveCount(documentCount);
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchGridMethod_WithAFullPostBackCall_ThenIGetTHeCorrectViewReturned()
    {
      var controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocuments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(_documentsIndexed);
      _documentService.Setup(d => d.GetDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetUnApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });

      var result = (ViewResult)controller.SearchGrid(It.IsAny<string>());
      result.Model.Should().BeOfType<DocumentsViewModel>();
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchGridMethod_WithAnAjaxRequest_ThenIGetTHeCorrectViewReturned()
    {
      var controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocuments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(_documentsIndexed);
      _documentService.Setup(d => d.GetDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetUnApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });
      _documentService.Setup(d => d.GetApprovedDocuments(It.IsAny<String>())).Returns(new List<Document>() { new Document() });

      var result = (PartialViewResult)controller.SearchGrid(It.IsAny<string>(), It.IsAny<int>(), true);
      
      result.Model.Should().BeOfType<DocumentsViewModel>();
      result.ViewName.Should().Be("_PagedDocumentResults");
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchCriteriaMethod_ThenItRetrunsTheCorrectNumberOfDocuments()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      _manCoService.Setup(x => x.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo>());

      _documentService.Setup(
        d =>
        d.GetDocuments(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<List<string>>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>())
          ).Returns(_documentsIndexed);

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (ViewResult)_controller.Search(new SearchViewModel());

      _userService.Verify(x => x.GetApplicationUser(), Times.Once);

      _manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once);

      var model = (DocumentsViewModel)result.Model;

      int documentCount = (from s in _documentsIndexed.Results select s).Count();
      model.Documents.Should().HaveCount(documentCount);
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchCriteriaMethodForPageTwo_ThenItRetrunsTheCorrectNumberOfDocuments()
    {

      var documentsPageTwo = new PagedResult<IndexedDocumentData>
      {
        CurrentPage = 2,
        ItemsPerPage = 10,
        TotalItems = 50,
        Results = new[]
          {
            new IndexedDocumentData()
              {
                MappedIndexes = new List<IndexMapped>{}
              },
            new IndexedDocumentData()
              {
                MappedIndexes = new List<IndexMapped>{}
              },
            new IndexedDocumentData()
              {
                MappedIndexes = new List<IndexMapped>{}
              }
          }
      };

      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      _manCoService.Setup(x => x.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo>());

      _documentService.Setup(
        d =>
        d.GetDocuments(
          2,
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<List<string>>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>())).Returns(documentsPageTwo);

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (ViewResult)_controller.Search(new SearchViewModel(), 2);

      _userService.Verify(x => x.GetApplicationUser(), Times.Once);

      _manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once);

      var model = (DocumentsViewModel)result.Model;

      int documentCount = (from s in _documentsIndexed.Results select s).Count();
      model.Documents.Should().HaveCount(documentCount);
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchCriteraMethod_WithAnAjaxRequest_ThenItReturnsTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      _manCoService.Setup(x => x.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo>());

      _documentService.Setup(
        d =>
        d.GetDocuments(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<List<string>>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>())).Returns(_documentsIndexed);

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (PartialViewResult)_controller.Search(new SearchViewModel(), It.IsAny<int>(), true);

      _userService.Verify(x => x.GetApplicationUser(), Times.Once);

      _manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once);

      result.Model.Should().BeOfType<DocumentsViewModel>();
      result.ViewName.Should().Be("_PagedDocumentResults");

      _controller.TempData.Should().NotBeNull();
      _controller.TempData["valid"].Should().Be("Show filter");
      _controller.TempData["tooManyDocs"].Should().Be(string.Empty);
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchCriteraMethod_AndThereAreTooManyDocumentsReurned_ThenIGetTheCorrectErrorMessage()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      _manCoService.Setup(x => x.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo>());

      _documentService.Setup(
        d =>
        d.GetDocuments(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<List<string>>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>())).Returns(_tooManyDocuments);

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (ViewResult)_controller.Search(new SearchViewModel());

      _userService.Verify(x => x.GetApplicationUser(), Times.Once);

      _manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once);

      var model = (DocumentsViewModel)result.Model;
      model.Should().BeOfType<DocumentsViewModel>();

      _controller.TempData.Should().NotBeNull();
      _controller.TempData["valid"].Should().Be("Show filter");
      _controller.TempData["tooManyDocs"].Should().Be("There were too many documents returned from your search. Please refine your search criteria");
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchCriteraMethod_ThenItReturnsTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());
      _manCoService.Setup(x => x.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo>());
      _documentService.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(new Document
                                                                               {
                                                                                 GridRun = new GridRun
                                                                                             {
                                                                                               Grid = "grid"
                                                                                             }
                                                                               });

      _documentService.Setup(
        d =>
        d.GetDocuments(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<List<string>>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>())).Returns(_documentsIndexed);

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (ViewResult)_controller.Search(new SearchViewModel());

      _userService.Verify(x => x.GetApplicationUser(), Times.Once);

      _manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once);

      result.Model.Should().BeOfType<DocumentsViewModel>();
      var model = (DocumentsViewModel)result.Model;

      model.Documents.Should().OnlyContain(d => d.Grid == "grid");

      _controller.TempData.Should().NotBeNull();
      _controller.TempData["valid"].Should().Be("Show filter");
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchCriteraMethod_AndThereAreDocumentsThatHaveBeenApproved_ThenItReturnsTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);


      DateTime approvalDate = DateTime.Now;

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());
      _manCoService.Setup(x => x.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo>());
      _documentService.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(new Document()
                                                                               {
                                                                                 Approval = new Approval
                                                                                              {
                                                                                                ApprovedBy = "user",
                                                                                                ApprovedDate = approvalDate
                                                                                              }
                                                                               });

      _documentService.Setup(
        d =>
        d.GetDocuments(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<List<string>>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>())).Returns(_documentsIndexed);

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (ViewResult)_controller.Search(new SearchViewModel());

      _userService.Verify(x => x.GetApplicationUser(), Times.Once);

      _manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once);

      result.Model.Should().BeOfType<DocumentsViewModel>();
      var model = (DocumentsViewModel)result.Model;

      model.Documents.Should().OnlyContain(d => d.ApprovedBy == "user");
      model.Documents.Should().OnlyContain(d => d.ApprovedDate == approvalDate);

      _controller.TempData.Should().NotBeNull();
      _controller.TempData["valid"].Should().Be("Show filter");
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchCriteraMethod_AndThereAreDocumentsThatHaveBeenCheckedOut_ThenItReturnsTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);


      DateTime checkOutDate = DateTime.Now;

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());
      _manCoService.Setup(x => x.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo>());
      _documentService.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(new Document()
      {
       CheckOut = new CheckOut
                    {
                      CheckOutBy = "user",
                      CheckOutDate = checkOutDate
                    }
      });

      _documentService.Setup(
        d =>
        d.GetDocuments(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<List<string>>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>())).Returns(_documentsIndexed);

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (ViewResult)_controller.Search(new SearchViewModel());

      _userService.Verify(x => x.GetApplicationUser(), Times.Once);

      _manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once);

      result.Model.Should().BeOfType<DocumentsViewModel>();
      var model = (DocumentsViewModel)result.Model;
      
      model.Documents.Should().OnlyContain(d => d.CheckedOutBy == "user");
      model.Documents.Should().OnlyContain(d => d.CheckedOutDate == checkOutDate);

      _controller.TempData.Should().NotBeNull();
      _controller.TempData["valid"].Should().Be("Show filter");
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchCriteraMethod_AndThereAreDocumentsThatHaveBeenRejected_ThenItReturnsTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);


      DateTime rejectedDate = DateTime.Now;

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());
      _manCoService.Setup(x => x.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo>());
      _documentService.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(new Document()
      {
        Rejection = new Rejection
        {
          RejectedBy = "user",
          RejectionDate = rejectedDate
        }
      });

      _documentService.Setup(
        d =>
        d.GetDocuments(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<List<string>>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<string>())).Returns(_documentsIndexed);

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (ViewResult)_controller.Search(new SearchViewModel());

      _userService.Verify(x => x.GetApplicationUser(), Times.Once);

      _manCoService.Verify(x => x.GetManCosByUserId(It.IsAny<string>()), Times.Once);

      result.Model.Should().BeOfType<DocumentsViewModel>();
      var model = (DocumentsViewModel)result.Model;
      
      model.Documents.Should().OnlyContain(d => d.RejectedBy == "user");
      model.Documents.Should().OnlyContain(d => d.RejectionDate == rejectedDate);

      _controller.TempData.Should().NotBeNull();
      _controller.TempData["valid"].Should().Be("Show filter");
    }

    [Test]
    public void GivenADocumentController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      var result = (ViewResult)_controller.Index();
      result.Model.Should().BeOfType<DocumentsViewModel>();
      _controller.TempData.Should().NotBeNull();
      _controller.TempData["valid"].Should().Be("Show filter");
    }

    [Test]
    public void GivenADocumentController_WhenICallItsSearchCriteraMethod_AndThereIsAModelError_TheUserIsNotified()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _controller.ModelState.AddModelError("error", "message");

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (RedirectResult)_controller.Search(new SearchViewModel());

      _controller.TempData.Should().NotBeNull();
      var errorList = _controller.ModelState.Values.SelectMany(x => x.Errors).ToList();
      _controller.TempData["error"].Should().Be(errorList[0].ErrorMessage);
    }

    [Test]
    public void GivenValidData_WhenIAskForPdfContainer_ThenIGetTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      var result = (PartialViewResult)_controller.PdfContainer("1");
      result.ViewName.Should().Be("_PdfContainer");
    }

    [Test]
    public void GivenValidData_WhenIAskForStatus_AndThedocumentHasBeenApproved_ThenIGetTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);
      DateTime approved = DateTime.Now.AddMinutes(-1);

      _documentService.Setup(d => d.GetDocument("1")).Returns(new Document()
                                                                {
                                                                  GridRun = new GridRun
                                                                              {
                                                                                XmlFile = new XmlFile
                                                                                            {
                                                                                              Received = recieved
                                                                                            },
                                                                                EndDate = endDate,
                                                                              },
                                                                  Approval = new Approval
                                                                               {
                                                                                 ApprovedDate = approved,
                                                                                 ApprovedBy = "a"
                                                                               }
                                                                });

      var result = (PartialViewResult)_controller.Status("1");
      var model = (DocumentStatusViewModel)result.Model;

      model.Arrived.Should().Be(recieved.ToString("ddd d MMM yyyy HH:mm"));
      model.Proccessed.Should().Be(endDate.ToString("ddd d MMM yyyy HH:mm"));
      model.Approved.Should().Be(approved.ToString("ddd d MMM yyyy HH:mm"));
      model.ApprovedBy.Should().Be("a");
      result.ViewName.Should().Be("_DocumentStatus");
    }

    [Test]
    public void GivenValidData_WhenIAskForStatus_AndThedocumentHasBeenRejected_ThenIGetTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);
      DateTime rejected = DateTime.Now.AddMinutes(-1);

      _documentService.Setup(d => d.GetDocument("1")).Returns(new Document()
      {
        GridRun = new GridRun
        {
          XmlFile = new XmlFile
          {
            Received = recieved
          },
          EndDate = endDate,
        },
        Rejection = new Rejection()
        {
          RejectionDate = rejected,
          RejectedBy = "a"
        }
      });

      var result = (PartialViewResult)_controller.Status("1");
      var model = (DocumentStatusViewModel)result.Model;

      model.Arrived.Should().Be(recieved.ToString("ddd d MMM yyyy HH:mm"));
      model.Proccessed.Should().Be(endDate.ToString("ddd d MMM yyyy HH:mm"));
      model.Rejected.Should().Be(rejected.ToString("ddd d MMM yyyy HH:mm"));
      model.RejectedBy.Should().Be("a");
      result.ViewName.Should().Be("_DocumentStatus");
    }

    [Test]
    public void GivenValidData_WhenIAskForStatus_AndThedocumentHasBeenHouseHeld_ThenIGetTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);
      DateTime approved = DateTime.Now.AddMinutes(-1);
      DateTime houseHeld = DateTime.Now.AddMinutes(-1);

      _documentService.Setup(d => d.GetDocument("1")).Returns(new Document()
      {
        GridRun = new GridRun
        {
          XmlFile = new XmlFile
          {
            Received = recieved
          },
          EndDate = endDate,
        },
        Approval = new Approval
        {
          ApprovedDate = approved,
          ApprovedBy = "a"
        },
        HouseHold = new HouseHold
                      {
                        HouseHoldDate = houseHeld
                      }
      });

      var result = (PartialViewResult)_controller.Status("1");
      var model = (DocumentStatusViewModel)result.Model;

      model.Arrived.Should().Be(recieved.ToString("ddd d MMM yyyy HH:mm"));
      model.Proccessed.Should().Be(endDate.ToString("ddd d MMM yyyy HH:mm"));
      model.Approved.Should().Be(approved.ToString("ddd d MMM yyyy HH:mm"));
      model.HouseHeld.Should().Be(houseHeld.ToString("ddd d MMM yyyy HH:mm"));
      model.ApprovedBy.Should().Be("a");
      result.ViewName.Should().Be("_DocumentStatus");
    }

    [Test]
    public void GivenNoDocumen_WhenIAskForStatus_ThenIGetTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      Document document = null;

      _documentService.Setup(d => d.GetDocument("1")).Returns(document);

      var result = (PartialViewResult)_controller.Status("1");
      var model = (DocumentStatusViewModel)result.Model;

      model.Approved.Should().BeNull();
      model.Arrived.Should().BeNull();
      model.Proccessed.Should().BeNull();
      result.ViewName.Should().Be("_DocumentStatus");
    }

    [Test]
    public void GivenADocumentThatHasNotBeenApproved_WhenIAskForStatus_ThenIGetTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocument("1")).Returns(new Document()
                                                                {
                                                                  GridRun = new GridRun
                                                                              {
                                                                                XmlFile = new XmlFile
                                                                                            {
                                                                                              Received = DateTime.Now.AddDays(-1)
                                                                                            },
                                                                                EndDate = DateTime.Now.AddHours(-1)
                                                                              }
                                                                });

      var result = (PartialViewResult)_controller.Status("1");
      var model = (DocumentStatusViewModel)result.Model;

      model.Approved.Should().BeNull();
      result.ViewName.Should().Be("_DocumentStatus");
    }

    [Test]
    public void GivenADocumentThatHasNotBeenRecievedOrProcessed_WhenIAskForStatus_ThenIGetTheCorrectView()
    {
      _controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocument("1")).Returns(new Document()
      {
        Approval = new Approval
        {
          ApprovedDate = DateTime.Now.AddMinutes(-1)
        }
      });

      var result = (PartialViewResult)_controller.Status("1");
      var model = (DocumentStatusViewModel)result.Model;

      model.Arrived.Should().BeNull();
      model.Proccessed.Should().BeNull();
      result.ViewName.Should().Be("_DocumentStatus");
    }

    [Test]
    public void GivenAValidDocumentId_WhenIAskForShow_ThenIGetAFileStreamResult()
    {
      var controller = new DocumentController(
          _documentService.Object,
          _userService.Object,
          _manCoService.Object,
          _logger.Object);

      _documentService.Setup(d => d.GetDocumentStream("documentId")).Returns(new byte[] { 1, 2, 3 });
      SetControllerContext(controller);
      MockHttpContext.Setup(h => h.Response).Returns(new FakeResponse());

      var result = controller.Show("documentId") as FileStreamResult;

      result.Should().NotBeNull();
      result.Should().BeOfType<FileStreamResult>();
    }
  }
}
