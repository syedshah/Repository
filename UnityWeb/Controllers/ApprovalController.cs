namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Exceptions;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Constants;
  using UnityWeb.Filters;
  using UnityWeb.Models.Approval;
  using UnityWeb.Models.Shared;

  [AuthorizeLoggedInUser]
  public class ApprovalController : BaseController
  {
    private IApprovalService _approvalService;
    private IDocumentService _documentService;

    public ApprovalController(IApprovalService approvalService, IDocumentService documentService, ILogger logger)
      : base(logger)
    {
      _approvalService = approvalService;
      _documentService = documentService;
    }

    [HttpPost]
    public ActionResult Document(ApproveDocumentsViewModel approveDocumentsViewModel)
    {
      var documentsAlreadyApproved = new List<string>();
      var documentsApproved = new List<string>();
      var documentsAlreadyRejcted = new List<string>();

      foreach (var document in approveDocumentsViewModel.ApproveDocumentViewModel.Where(d => d.Selected))
      {
        ApproveDocument(document.DocumentId, document.ManCo, document.DocType, document.SubDocType, documentsAlreadyApproved, documentsApproved, documentsAlreadyRejcted);
      }

      var documentWarningsViewModel = new DocumentWarningsViewModel
                                        {
                                          DocumentsAlreadyApproved = string.Join(",", documentsAlreadyApproved.ToArray()),
                                          DocumentsApproved = documentsApproved.Count,
                                          DocumentsAlreadyRejected = string.Join(",", documentsAlreadyRejcted.ToArray())
                                        };

      return PartialView("_DocumentWarnings", documentWarningsViewModel);
    }

    [HttpPost]
    public ActionResult Grid(string grid)
    {
      var documentsAlreadyApproved = new List<string>();
      var documentsApproved = new List<string>();
      var documents = _documentService.GetDocuments(grid);
      var documentsAlreadyRejcted = new List<string>();

      foreach (var document in documents)
      {
        ApproveDocument(document.DocumentId, document.ManCo.Code, document.DocType.Code, document.SubDocType.Code, documentsAlreadyApproved, documentsApproved, documentsAlreadyRejcted);
      }

      var documentWarningsViewModel = new DocumentWarningsViewModel
      {
        DocumentsAlreadyApproved = string.Join(",", documentsAlreadyApproved.ToArray()),
        DocumentsApproved = documentsApproved.Count,
        DocumentsAlreadyRejected = string.Join(",", documentsAlreadyRejcted.ToArray())
      };

      return PartialView("_DocumentWarnings", documentWarningsViewModel);
    }

    [HttpPost]
    public ActionResult Basket(ApproveDocumentsViewModel approveDocumentsViewModel)
    {
      var documentsAlreadyApproved = new List<string>();
      var documentsApproved = new List<string>();
      var documentsAlreadyRejcted = new List<string>();

      foreach (var document in approveDocumentsViewModel.ApproveDocumentViewModel)
      {
        ApproveDocument(document.DocumentId, document.ManCo, document.DocType, document.SubDocType, documentsAlreadyApproved, documentsApproved, documentsAlreadyRejcted);
      }

      var documentWarningsViewModel = new DocumentWarningsViewModel
      {
        DocumentsAlreadyApproved = string.Join(",", documentsAlreadyApproved.ToArray()),
        DocumentsAlreadyRejected = string.Join(",", documentsAlreadyRejcted.ToArray()),
        DocumentsApproved = documentsApproved.Count
      };

      return PartialView("_DocumentWarnings", documentWarningsViewModel);
    }

    private void ApproveDocument(string documentId, string manCo, string docType, string subDocType, List<string> documentsAlreadyApproved, List<string> documentsApproved, List<string> documentsAlreadyRejcted)
    {
      try
      {
        _approvalService.ApproveDocument(this.HttpContext.User.Identity.Name, documentId, manCo, docType, subDocType);

        documentsApproved.Add(documentId);
      }
      catch (DocumentAlreadyApprovedException)
      {
        documentsAlreadyApproved.Add(documentId);
      }
      catch (DocumentAlreadyRejectedException)
      {
        documentsAlreadyRejcted.Add(documentId);
      }
    }
  }
}
