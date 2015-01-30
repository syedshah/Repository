namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Web.Mvc;
  using ClientProxies.ArchiveServiceReference;
  using Entities;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.Document;
  using UnityWeb.Models.Search;

  [AuthorizeLoggedInUser]
  public class DocumentController : BaseController
  {
    private readonly IDocumentService _documentService;
    private readonly IUserService _userService;
    private readonly IManCoService _manCoService;
    private readonly int _pageSize = 10;

    public DocumentController(
        IDocumentService documentService, 
        IUserService userService,
        IManCoService manCoService,
        ILogger logger)
      : base(logger)
    {
      _documentService = documentService;
      _userService = userService;
      _manCoService = manCoService;
    }

    [HttpGet]
    public ActionResult Index()
    {
      var model = new DocumentsViewModel();

      TempData["valid"] = "Show filter";
      return View("Search", model);
    }
    
    public ActionResult SearchGrid(string grid, int page = 1, bool isAjaxCall = false, bool filterHouseHolding = false, bool filterUnapproved = false, bool filterApproved = false)
    {
      var documentsViewModel = new DocumentsViewModel(grid, filterHouseHolding);
      PagedResult<IndexedDocumentData> documents = _documentService.GetDocuments(page, _pageSize, grid, filterHouseHolding, filterUnapproved, filterApproved);
      documentsViewModel.AddDocuments(documents);
      ProcessDocumentResults(documentsViewModel);

      documentsViewModel.AddTotalDocs(_documentService.GetDocuments(grid).Count());
      documentsViewModel.AddDocsAwaitingApproval(_documentService.GetUnApprovedDocuments(grid).Count());
      documentsViewModel.AddDocsApproved(_documentService.GetApprovedDocuments(grid).Count());

      if (isAjaxCall)
      {
        return PartialView("_PagedDocumentResults", documentsViewModel);
      }

      return View("Search", documentsViewModel);
    }

    public ActionResult Show(string documentId)
    {
      byte[] document = _documentService.GetDocumentStream(documentId);
      var memoryStream = ConvertToStream(document);
      return File(memoryStream, "application/pdf");
    }
    
    public ActionResult Search(SearchViewModel SearchViewModel, int page = 1, bool isAjaxCall = false)
    {
      Session["SearchViewModel"] = null;

      if (!ModelState.IsValid)
      {
        var errorList = ModelState.Values.SelectMany(x => x.Errors).ToList();
        TempData["error"] = errorList[0].ErrorMessage;
        return new RedirectResult(Request.Headers["Referer"]);
      }

      PopulateManCoTextList(SearchViewModel);

      AddSearchViewModelToSession(SearchViewModel);

      var documentsViewModel = new DocumentsViewModel();

      PagedResult<IndexedDocumentData> documents = _documentService.GetDocuments(
        page,
        _pageSize,
        SearchViewModel.SelectedDocText,
        SearchViewModel.SelectedSubDocText,
        SearchViewModel.AddresseeSubType,
        SearchViewModel.AccountNumber,
        SearchViewModel.MailingName,
        SearchViewModel.ManCoTexts,
        SearchViewModel.InvestorReference,
        SearchViewModel.ProcessingDateFrom,
        SearchViewModel.ProcessingDateTo,
        SearchViewModel.PrimaryHolder,
        SearchViewModel.AgentReference,
        SearchViewModel.AddresseePostCode,
        SearchViewModel.EmailAddress,
        SearchViewModel.FaxNumber,
        SearchViewModel.ContractDate,
        SearchViewModel.PaymentDate,
        SearchViewModel.DocumentNumber);

      documentsViewModel.AddDocuments(documents);
      
      ProcessDocumentResults(documentsViewModel);

      TempData["valid"] = "Show filter";

      if (documents.Results.Count == 0 && documents.TotalItems > 0)
      {
        TempData["tooManyDocs"] = "There were too many documents returned from your search. Please refine your search criteria";
      }
      else
      {
        TempData["tooManyDocs"] = string.Empty;
      }

      if (isAjaxCall)
      {
        return PartialView("_PagedDocumentResults", documentsViewModel);
      }
      
      return View(documentsViewModel);
    }
    
    public ActionResult PdfContainer(string documentId)
    {
      return PartialView("_PdfContainer", documentId);
    }

    public ActionResult Status(string documentId)
    {
      var document = _documentService.GetDocument(documentId);

      if (document != null)
      {
        return PartialView("_DocumentStatus", new DocumentStatusViewModel(document));  
      }
      else
      {
        return PartialView("_DocumentStatus", new DocumentStatusViewModel());
      }
    }

    [OutputCache(CacheProfile = "medium", VaryByParam = "document")]
    private MemoryStream ConvertToStream(byte[] document)
    {
      MemoryStream memoryStream = new MemoryStream();
      memoryStream.Write(document, 0, document.Length);
      memoryStream.Position = 0;

      HttpContext.Response.AddHeader("content-disposition", "inline; filename=myPDF.pdf");
      return memoryStream;
    }

    private void AddSearchViewModelToSession(SearchViewModel searchViewModel)
    {
      this.HttpContext.Session["SearchViewModel"] = searchViewModel;
    }

    private void PopulateManCoTextList(SearchViewModel searchViewModel)
    {
      if (searchViewModel.SelectedManCoText == null)
      {
        searchViewModel.ManCoTexts = this.GenerateMancoTextList().ToList();
      }
      else
      {
        searchViewModel.ManCoTexts.Add(searchViewModel.SelectedManCoText.Trim());
      }  
    }

    private IList<string> GenerateMancoTextList()
    {
      var manCoTextList = new List<string>();

      var currentUser = this._userService.GetApplicationUser();
      var manCos = this._manCoService.GetManCosByUserId(currentUser.Id);

      manCos.ToList().ForEach(x => manCoTextList.Add(x.Code));

      return manCoTextList;
    }

    private void ProcessDocumentResults(DocumentsViewModel dvm)
    {
      foreach (DocumentViewModel documentViewModel in dvm.Documents)
      {
        Document document = _documentService.GetDocument(documentViewModel.DocumentId);
        documentViewModel.AddCheckoutDetails(document);
        documentViewModel.AddApprovalDetails(document);
        documentViewModel.AddRejectionDetails(document);
        documentViewModel.AddGridRunDetails(document);
        documentViewModel.AddManCoDetails(document);
        documentViewModel.AddDoNotPrintFlag(document);
      }
    }
  }
}
