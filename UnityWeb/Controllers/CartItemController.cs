namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Entities;
  using Exceptions;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.CartItem;

  [AuthorizeLoggedInUser]
  public class CartItemController : BaseController
  {
    private ICartItemService _cartItemService;
    private IExportFileService _exportFileService;
    private IDocumentService _documentService;

    public int PageSize = 10;

    public CartItemController(
        ICartItemService cartItemService,
        IExportFileService exportFileService,
        IDocumentService documentService,
        ILogger logger)
      : base(logger)
    {
      _cartItemService = cartItemService;
      _exportFileService = exportFileService;
      _documentService = documentService;
    }

    [AuthorizeLoggedInUser(Roles = "Admin,dstadmin,Governor")]
    public ActionResult Index(int page = 1, bool isAjaxCall = false)
    {
      var cartItemsViewModel = new CartItemsViewModel();
      CartItem cartItem = new CartItem(this.HttpContext);

      var cartItems = _cartItemService.GetCartItems(cartItem.CartId, page, PageSize);

      cartItemsViewModel.AddCartItems(cartItems);

      if (isAjaxCall)
      {
        return PartialView("_PagedCartItemResults", cartItemsViewModel);
      }

      if (cartItemsViewModel.CartTotal == 0)
      {
        return RedirectToAction("Index", "Dashboard");
      }

      return View(cartItemsViewModel);
    }

    public ActionResult AddGridToCart(string grid)
    {
      var checkedOutDoc = new List<string>();
      var couldNotAddDoc = new List<string>();
      var anotherManCoAlredy = new List<string>();

      var documents = _documentService.GetDocuments(grid);

      foreach (var document in documents)
      {
        this.AddDocumentToCart(document.DocumentId, document.ManCo.Code, document.DocType.Code, document.SubDocType.Code, anotherManCoAlredy, checkedOutDoc, couldNotAddDoc);
      }

      var cartItemSummaryViewModel = GetCartSummary();
      cartItemSummaryViewModel.DocumentWarningsViewModel.DocumentsAlreadyCheckedOut = string.Join(",", checkedOutDoc.ToArray());
      cartItemSummaryViewModel.DocumentWarningsViewModel.DocumentAddToBasketErrors = string.Join(",", couldNotAddDoc.ToArray());
      cartItemSummaryViewModel.DocumentWarningsViewModel.BasketContainsDocumentFromAnotherManCo = string.Join(",", anotherManCoAlredy.ToArray());

      return PartialView("_CartSummary", cartItemSummaryViewModel);
    }


    [HttpPost]
    public ActionResult AddToCart(AddCartItemsViewModel addCartItemsViewModel)
    {
      var checkedOutDoc = new List<string>();
      var couldNotAddDoc = new List<string>();
      var anotherManCoAlredy = new List<string>();

      if (addCartItemsViewModel.AddCartItemViewModel == null)
        return PartialView("_CartSummary", new CartItemSummaryViewModel());

      foreach (var document in addCartItemsViewModel.AddCartItemViewModel.Where(d => d.Selected))
      {
        this.AddDocumentToCart(document.DocumentId, document.ManCo, document.DocType, document.SubDocType, anotherManCoAlredy, checkedOutDoc, couldNotAddDoc);
      }

      var cartItemSummaryViewModel = GetCartSummary();
      cartItemSummaryViewModel.DocumentWarningsViewModel.DocumentsAlreadyCheckedOut = string.Join(",", checkedOutDoc.ToArray());
      cartItemSummaryViewModel.DocumentWarningsViewModel.DocumentAddToBasketErrors = string.Join(",", couldNotAddDoc.ToArray());
      cartItemSummaryViewModel.DocumentWarningsViewModel.BasketContainsDocumentFromAnotherManCo = string.Join(",", anotherManCoAlredy.ToArray());

      return PartialView("_CartSummary", cartItemSummaryViewModel);
    }

    private void AddDocumentToCart(
      string documentId, string manCo, string docType, string subDocType, List<string> anotherManCoAlredy, List<string> checkedOutDoc, List<string> couldNotAddDoc)
    {
      CartItem cartItem = new CartItem(this.HttpContext);

      try
      {
        this._cartItemService.AddItem(
          this.HttpContext.User.Identity.Name,
          documentId,
          cartItem.CartId,
          manCo,
          docType,
          subDocType);
      }
      catch (BasketContainsDifferentManCoException e)
      {
        anotherManCoAlredy.Add(documentId);
      }
      catch (DocumentCurrentlyCheckedOutException e)
      {
        checkedOutDoc.Add(documentId);
      }
      catch (UnityException e)
      {
        couldNotAddDoc.Add(documentId);
      }
    }

    [HttpPost]
    public ActionResult RemoveFromCart(RemoveCartItemsViewModel removeCartItemsViewModel)
    {
      foreach (var document in removeCartItemsViewModel.RemoveCartItemViewModel.Where(d => d.Selected))
      {
        CartItem cartItem = new CartItem(HttpContext);

        _cartItemService.RemoveItem(document.DocumentId, cartItem.CartId);
      }

      var cartItemSummaryViewModel = GetCartSummary();

      return PartialView("_CartSummary", cartItemSummaryViewModel);
    }

    public ActionResult ClearCart()
    {
      CartItem cartItem = new CartItem(HttpContext);
      _cartItemService.ClearCart(cartItem.CartId);

      return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    public ActionResult ExportBasketToZip(string cartId)
    {
      var cartItems = _cartItemService.GetCart(cartId);

      var documentIds = (from c in cartItems select c.Document.DocumentId).ToList();

      var zipFilePath = this._exportFileService.ExportToZip(documentIds);

      documentIds.ForEach(x => this._cartItemService.RemoveItem(x, cartId));

      return this.Json(new { file = zipFilePath });
    }

    [HttpPost]
    public ActionResult ExportDocumentsToZip(List<string> documentIds)
    {
      var zipFilePath = this._exportFileService.ExportToZip(documentIds);

      CartItem cartItem = new CartItem(HttpContext);

      documentIds.ForEach(x => this._cartItemService.RemoveItem(x, cartItem.CartId));

      return this.Json(new { file = zipFilePath });
    }

    public ActionResult Download(string file)
    {
      return this.File(file, System.Net.Mime.MediaTypeNames.Application.Zip, "ZippedDocuments.zip");
    }

    [ChildActionOnly]
    public ActionResult Summary()
    {
      var cartItemSummaryViewModel = GetCartSummary();
      return PartialView("_CartSummary", cartItemSummaryViewModel);
    }

    private CartItemSummaryViewModel GetCartSummary()
    {
      string cartId = new CartItem(HttpContext).CartId;
      int cartItems = _cartItemService.GetNumberOfItemsInCart(cartId);

      CartItemSummaryViewModel cartItemSummaryViewModel = new CartItemSummaryViewModel(cartItems);
      return cartItemSummaryViewModel;
    }
  }
}
