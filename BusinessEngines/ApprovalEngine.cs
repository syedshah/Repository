namespace BusinessEngines
{
  using System;
  using BusinessEngineInterfaces;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class ApprovalEngine : IApprovalEngine
  {
    private readonly IDocumentService _documentService;
    private readonly IApprovalRepository _approvalRepository;

    public ApprovalEngine(IDocumentService documentService, IApprovalRepository approvalRepository)
    {
      _documentService = documentService;
      _approvalRepository = approvalRepository;
    }

    public void AutoApproveDocument(string documentId)
    {
      var document = _documentService.GetDocument(documentId);

      var approval = new Approval
                       {
                         ApprovedBy = "auto approval",
                         ApprovedDate = DateTime.Now,
                         DocumentId = document.Id,
                       };

      _approvalRepository.Create(approval);
    }

    public void ApproveDocument(string userName, string documentId, string manCo, string docType, string subDocType)
    {
      var document = _documentService.GetDocument(documentId);

      if (document == null)
      {
        _documentService.AddDocument(documentId, docType, subDocType, manCo, null);
        document = _documentService.GetDocument(documentId);
      }

      if (document.Approval != null)
      {
        throw new DocumentAlreadyApprovedException("Document is already approved");
      }

      if (document.Rejection != null)
      {
        throw new DocumentAlreadyRejectedException("Document is already rejected");
      }

      var approval = new Approval
                       {
                         ApprovedBy = userName,
                         ApprovedDate = DateTime.Now,
                         DocumentId = document.Id
                       };

      _approvalRepository.Create(approval);
    }
  }
}
