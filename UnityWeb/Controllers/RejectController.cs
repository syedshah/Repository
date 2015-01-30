namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Exceptions;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.Rejection;
  using UnityWeb.Models.Shared;

  [AuthorizeLoggedInUser]
  public class RejectController : BaseController
  {
    private IRejectionService _rejectionService;
    private IDocumentService _documentService;

    public RejectController(IRejectionService rejectionService, IDocumentService documentService, ILogger logger)
      : base(logger)
    {
      _rejectionService = rejectionService;
      _documentService = documentService;
    }

    [HttpPost]
    public ActionResult Document(RejectDocumentsViewModel rejectDocumentsViewModel)
    {
      var documentsAlreadyApproved = new List<string>();
      var documentsRejected = new List<string>();
      var documentsAlreadyRejcted = new List<string>();

      foreach (var document in rejectDocumentsViewModel.RejectDocumentViewModel.Where(d => d.Selected))
      {
        RejectDocument(document.DocumentId, document.ManCo, document.DocType, document.SubDocType, documentsAlreadyApproved, documentsRejected, documentsAlreadyRejcted);
      }

      var documentWarningsViewModel = new DocumentWarningsViewModel
                                        {
                                          DocumentsAlreadyApproved = string.Join(",", documentsAlreadyApproved.ToArray()),
                                          DocumentsRejected = documentsRejected.Count,
                                          DocumentsAlreadyRejected = string.Join(",", documentsAlreadyRejcted.ToArray())
                                        };

      return PartialView("_DocumentWarnings", documentWarningsViewModel);
    }

    [HttpPost]
    public ActionResult Grid(string grid)
    {
      var documentsAlreadyApproved = new List<string>();
      var documentsRejected = new List<string>();
      var documents = _documentService.GetDocuments(grid);
      var documentsAlreadyRejcted = new List<string>();

      foreach (var document in documents)
      {
        RejectDocument(document.DocumentId, document.ManCo.Code, document.DocType.Code, document.SubDocType.Code, documentsAlreadyApproved, documentsRejected, documentsAlreadyRejcted);
      }

      var documentWarningsViewModel = new DocumentWarningsViewModel
      {
        DocumentsAlreadyApproved = string.Join(",", documentsAlreadyApproved.ToArray()),
        DocumentsRejected = documentsRejected.Count,
        DocumentsAlreadyRejected = string.Join(",", documentsAlreadyRejcted.ToArray())
      };

      return PartialView("_DocumentWarnings", documentWarningsViewModel);
    }

    [HttpPost]
    public ActionResult Basket(RejectDocumentsViewModel rejectDocumentsViewModel)
    {
      var documentsAlreadyApproved = new List<string>();
      var documentsRejected = new List<string>();
      var documentsAlreadyRejcted = new List<string>();

      foreach (var document in rejectDocumentsViewModel.RejectDocumentViewModel)
      {
        RejectDocument(document.DocumentId, document.ManCo, document.DocType, document.SubDocType, documentsAlreadyApproved, documentsRejected, documentsAlreadyRejcted);
      }

      var documentWarningsViewModel = new DocumentWarningsViewModel
      {
        DocumentsAlreadyApproved = string.Join(",", documentsAlreadyApproved.ToArray()),
        DocumentsAlreadyRejected = string.Join(",", documentsAlreadyRejcted.ToArray()),
        DocumentsRejected = documentsRejected.Count
      };

      return PartialView("_DocumentWarnings", documentWarningsViewModel);
    }

    private void RejectDocument(string documentId, string manCo, string docType, string subDocType, List<string> documentsAlreadyApproved, List<string> documentsRejected, List<string> documentsAlreadyRejcted)
    {
      try
      {
        _rejectionService.RejectDocument(this.HttpContext.User.Identity.Name, documentId, manCo, docType, subDocType);

        documentsRejected.Add(documentId);
      }
      catch (DocumentAlreadyApprovedException e)
      {
        documentsAlreadyApproved.Add(documentId);
      }
      catch (DocumentAlreadyRejectedException e)
      {
        documentsAlreadyRejcted.Add(documentId);
      }
    }
  }
}

