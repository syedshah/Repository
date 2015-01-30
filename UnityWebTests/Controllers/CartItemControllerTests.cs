namespace UnityWebTests.Controllers
{
  using System;
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
  using UnityWeb.Models.CartItem;

  [TestFixture]
  public class CartItemControllerTests : ControllerTestsBase
  {
    private Mock<ICartItemService> _cartItemService;
    private Mock<IExportFileService> _exportFileService;
    private Mock<IDocumentService> _documentService;
    private Mock<ILogger> _logger;
    private CartItemController _cartItemController;
    private AddCartItemsViewModel _addCartItemsViewModel;
    private IList<string> _documentIds;
    private IList<Document> _documents;

    [SetUp]
    public void SetUp()
    {
      _cartItemService = new Mock<ICartItemService>();
      _exportFileService = new Mock<IExportFileService>();
      _documentService = new Mock<IDocumentService>();
      _logger = new Mock<ILogger>();

      _cartItemController = new CartItemController(_cartItemService.Object, 
          _exportFileService.Object,
          _documentService.Object,
          _logger.Object);

      _addCartItemsViewModel = new AddCartItemsViewModel()
                                 {
                                   AddCartItemViewModel = new List<AddCartItemViewModel>()
                                                            {
                                                              new AddCartItemViewModel()
                                                                {
                                                                  DocumentId = "guid1",
                                                                  Selected = true
                                                                },
                                                                new AddCartItemViewModel()
                                                                {
                                                                  DocumentId = "guid2",
                                                                  Selected = true
                                                                }
                                                            }
                                 };

       _documentIds = new List<string>();

      _documents = new List<Document>()
                     {
                       new Document()
                         {
                           ManCo = new ManCo() { Code = "manCoCode" },
                           DocType = new DocType() { Code = "docTypeCode" },
                           SubDocType = new SubDocType() { Code = "subDocTypeCode" },
                           DocumentId = "guid1"
                         },
                       new Document()
                         {
                           ManCo = new ManCo() { Code = "manCoCode2" },
                           DocType = new DocType() { Code = "docTypeCode2" },
                           SubDocType = new SubDocType() { Code = "subDocTypeCode2" },
                           DocumentId = "guid2"
                         }
                     };
    }

    [Test]
    public void GivenALoggedInUser_WhenIAskForTheDocumentSummary_ThenIGetTheCorrectView()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(a => a.GetNumberOfItemsInCart("testUser")).Returns(1);

      var result = (PartialViewResult)_cartItemController.Summary();
      var model = result.Model as CartItemSummaryViewModel;

      model.Total.Should().Be("1");
      result.ViewName.Should().Be("_CartSummary");
    }

    [Test]
    public void GivenACartItemController_WhenIAddDocumentsToTheBasket_IGetTheCorrectViewReturned()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(a => a.AddItem("name", "docId", "cartId", "manCo", "docType", "subDocType"));
      _cartItemService.Setup(a => a.GetCartItems("testUser", 1, 10)).Returns(new PagedResult<CartItem>());

      var result = (PartialViewResult)_cartItemController.AddToCart(_addCartItemsViewModel);
      result.Model.Should().BeOfType<CartItemSummaryViewModel>();
      result.ViewName.Should().Be("_CartSummary");
    }

    [Test]
    public void GivenACartItemController_WhenIAddDocumentsToTheBasket_AndThereAreDocumentsCheckedOut_TempDataMessageIsReturned()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(a => a.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<DocumentCurrentlyCheckedOutException>();
      _cartItemService.Setup(a => a.GetCartItems("testUser", 1, 19)).Returns(new PagedResult<CartItem>());

      var result = (PartialViewResult)_cartItemController.AddToCart(new AddCartItemsViewModel());
      
      result.Model.Should().BeOfType<CartItemSummaryViewModel>();
      var model = (CartItemSummaryViewModel)result.Model;
      model.DocumentWarningsViewModel.DocumentsAlreadyCheckedOut.Should().BeNull();
      result.ViewName.Should().Be("_CartSummary");
    }

    [Test]
    public void GivenACartItemController_WhenIAddDocumentsToTheBasket_AndThereIsADocumentForAnotherManCoInThereAlready_TempDataMessageIsReturned()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(a => a.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<BasketContainsDifferentManCoException>();
      _cartItemService.Setup(a => a.GetCartItems("testUser", 1, 19)).Returns(new PagedResult<CartItem>());

      var result = (PartialViewResult)_cartItemController.AddToCart(_addCartItemsViewModel);

      result.Model.Should().BeOfType<CartItemSummaryViewModel>();
      var model = (CartItemSummaryViewModel)result.Model;
      model.DocumentWarningsViewModel.BasketContainsDocumentFromAnotherManCo.Should().Be("guid1,guid2");
      result.ViewName.Should().Be("_CartSummary");
    }

    [Test]
    public void GivenACartItemController_WhenNoDocumentsAreSelected_ThenIGetAnEmtpyViewModelsReturned()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(a => a.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<DocumentCurrentlyCheckedOutException>();
      _cartItemService.Setup(a => a.GetCartItems("testUser", 1, 19)).Returns(new PagedResult<CartItem>());

      var result = (PartialViewResult)_cartItemController.AddToCart(_addCartItemsViewModel);

      result.Model.Should().BeOfType<CartItemSummaryViewModel>();
      var model = (CartItemSummaryViewModel)result.Model;
      model.DocumentWarningsViewModel.DocumentsAlreadyCheckedOut.Should().Be("guid1,guid2");
      result.ViewName.Should().Be("_CartSummary");
    }

    [Test]
    public void GivenACartItemController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectNumberOfCartItems()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(c => c.GetCartItems("testUser", 1, 10)).Returns(new PagedResult<CartItem>()
                                                                             {
                                                                               Results = new List<CartItem>()
                                                                                           {
                                                                                             new CartItem()
                                                                                               {
                                                                                                 Document = new Document()
                                                                                                              {
                                                                                                                ManCo = new ManCo(),
                                                                                                                SubDocType = new SubDocType(),
                                                                                                                DocType = new DocType()
                                                                                                              }
                                                                                               },
                                                                                             new CartItem()
                                                                                               {
                                                                                                 Document = new Document()
                                                                                                              {
                                                                                                                ManCo = new ManCo(),
                                                                                                                SubDocType = new SubDocType(),
                                                                                                                DocType = new DocType()
                                                                                                              }

                                                                                               }
                                                                                           },
                                                                                           CurrentPage = 1,
                                                                                           EndRow = 10,
                                                                                           ItemsPerPage = 1,
                                                                                           StartRow = 1,
                                                                                           TotalItems = 30
                                                                             });

      var result = (ViewResult)_cartItemController.Index();
      var model = (CartItemsViewModel)result.Model;
      model.CartItems.Count.Should().Be(2);
    }

    [Test]
    public void GivenACartItemController_WhenICallItsIndexMethod__AndThereAreNoBasketItems_ThenItReturnsToTheDashBoardIndexControllerIndexAction()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(c => c.GetCartItems("testUser", 1, 10)).Returns(new PagedResult<CartItem>()
      {
        Results = new List<CartItem>(),
        CurrentPage = 1,
        EndRow = 10,
        ItemsPerPage = 1,
        StartRow = 1,
        TotalItems = 0
      });

      var result = _cartItemController.Index() as RedirectToRouteResult;
      result.Should().NotBeNull();
      result.RouteValues["Controller"].Should().Be("Dashboard");
      result.RouteValues["Action"].Should().Be("Index");
    }

    [Test]
    public void GivenACartItemController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(c => c.GetCartItems("testUser", 1, 10)).Returns(new PagedResult<CartItem>()
                                                                             {
                                                                               Results = new List<CartItem>()
                                                                                           {
                                                                                             new CartItem()
                                                                                               {
                                                                                                 Document = new Document()
                                                                                                              {
                                                                                                                ManCo = new ManCo(),
                                                                                                                SubDocType = new SubDocType(),
                                                                                                                DocType = new DocType()
                                                                                                              }
                                                                                               },
                                                                                             new CartItem()
                                                                                               {
                                                                                                 Document = new Document()
                                                                                                              {
                                                                                                                ManCo = new ManCo(),
                                                                                                                SubDocType = new SubDocType(),
                                                                                                                DocType = new DocType()
                                                                                                              }

                                                                                               }
                                                                                           },
                                                                               CurrentPage = 1,
                                                                               EndRow = 10,
                                                                               ItemsPerPage = 1,
                                                                               StartRow = 1,
                                                                               TotalItems = 30
                                                                             });

      var result = (ViewResult)_cartItemController.Index();
      var model = (CartItemsViewModel)result.Model;
      model.Should().BeOfType<CartItemsViewModel>();
    }

    [Test]
    public void GivenAValidViewModel_WhenITryToDeleteADocumentInTheBasket_ThenIGetTheCorrectView()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      List<RemoveCartItemViewModel> removeCartItemViewModel = new List<RemoveCartItemViewModel>()
                                                                {
                                                                  new RemoveCartItemViewModel()
                                                                    {
                                                                      DocumentId = "guid",
                                                                      Selected = true
                                                                    }
                                                                };
      _cartItemService.Setup(c => c.RemoveItem("guid", "testUser"));

      RemoveCartItemsViewModel removeCartItemsViewModel = new RemoveCartItemsViewModel
                                                            {
                                                              RemoveCartItemViewModel = removeCartItemViewModel
                                                            };

      var result = (PartialViewResult)_cartItemController.RemoveFromCart(removeCartItemsViewModel);
      result.Model.Should().BeOfType<CartItemSummaryViewModel>();
      result.ViewName.Should().Be("_CartSummary");
    }

    [Test]
    public void GivenAValidViewModel_WhenITryToEmptyTheBasket_ThenIGetTheCorrectView()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");
      
      _cartItemService.Setup(c => c.ClearCart("testUser"));

      var result = _cartItemController.ClearCart() as RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["Controller"].Should().Be("Dashboard");
      result.RouteValues["Action"].Should().Be("Index");
    }

    [Test]
    public void GivenAValidListOfDocumentIds_WhenITryToExportToZip_ThenIGetTheCorrectResult()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());

      var zipFilePath = "files/exportFiles/err6f87r1.zip";
      _exportFileService.Setup(e => e.ExportToZip(It.IsAny<List<string>>())).Returns(zipFilePath);

      this._cartItemService.Setup(x => x.RemoveItem(It.IsAny<string>(), "testUser"));

      var result = this._cartItemController.ExportDocumentsToZip(_documentIds.ToList());
      var jsonResult = result as JsonResult;

      result.Should().BeOfType<JsonResult>();
      var jsonFilevalue = jsonResult.Data.GetType().GetProperty("file").GetValue(jsonResult.Data, null);
      jsonFilevalue.ShouldBeEquivalentTo(zipFilePath);

      _exportFileService.Verify(e => e.ExportToZip(It.IsAny<List<string>>()), Times.AtLeastOnce);
      this._cartItemService.Verify(x => x.RemoveItem(It.IsAny<string>(), "testUser"), Times.AtMost(5));
    }

    [Test]
    public void GivenAValidCartId_WhenITryToExportToZip_ThenIGetTheCorrectResult()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      var zipFilePath = "files/exportFiles/err6f87r1.zip";
      _exportFileService.Setup(e => e.ExportToZip(It.IsAny<List<string>>())).Returns(zipFilePath);

      _cartItemService.Setup(e => e.GetCart("cartId")).Returns(new List<CartItem>() { new CartItem() { Document = new Document() { DocumentId = "docId"} } });

      this._cartItemService.Setup(x => x.RemoveItem(It.IsAny<string>(), "testUser"));

      var result = this._cartItemController.ExportBasketToZip("cartId");
      var jsonResult = result as JsonResult;

      result.Should().BeOfType<JsonResult>();
      var jsonFilevalue = jsonResult.Data.GetType().GetProperty("file").GetValue(jsonResult.Data, null);
      jsonFilevalue.ShouldBeEquivalentTo(zipFilePath);

      _exportFileService.Verify(e => e.ExportToZip(It.IsAny<List<string>>()), Times.AtLeastOnce);
      this._cartItemService.Verify(x => x.RemoveItem(It.IsAny<string>(), "testUser"), Times.AtMost(5));
    }

    [Test]
    public void GivenAValidZipFileLocation_WhenITryToDownloadTheZip_ThenIGetTheCorrectResult()
    {
      SetControllerContext(_cartItemController);
      var zipFilePath = "files/exportFiles/err6f87r1.zip";

      var result = this._cartItemController.Download(zipFilePath);
      var filePathResult = result as FilePathResult;

      result.Should().BeOfType<FilePathResult>();
      filePathResult.FileName.ShouldBeEquivalentTo(zipFilePath);
    }

    [Test]
    public void GivenACartItemController_WhenIAddAGRIDToTheBasket_IGetTheCorrectViewReturned()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(a => a.AddItem("name", "docId", "cartId", "manCo", "docType", "subDocType"));
      _documentService.Setup(d => d.GetDocuments("grid")).Returns(_documents);

      var result = (PartialViewResult)_cartItemController.AddGridToCart("grid");
      result.Model.Should().BeOfType<CartItemSummaryViewModel>();
      result.ViewName.Should().Be("_CartSummary");
    }

    [Test]
    public void GivenACartItemController_WhenIAddAGridToTheBasket_AndThereIsADocumentForAnotherManCoInThereAlready_TempDataMessageIsReturned()
    {
      SetControllerContext(_cartItemController);
      MockHttpContext.SetupGet(x => x.Session["CartId"]).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["testUser"]).Returns("testUser");

      _cartItemService.Setup(a => a.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<BasketContainsDifferentManCoException>();
      _documentService.Setup(d => d.GetDocuments("grid")).Returns(_documents);

      var result = (PartialViewResult)_cartItemController.AddGridToCart("grid");

      result.Model.Should().BeOfType<CartItemSummaryViewModel>();
      var model = (CartItemSummaryViewModel)result.Model;
      model.DocumentWarningsViewModel.BasketContainsDocumentFromAnotherManCo.Should().Be("guid1,guid2");
      result.ViewName.Should().Be("_CartSummary");
    }
  }
}
