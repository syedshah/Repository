namespace BusinessEngines
{
  using System;
  using BusinessEngineInterfaces;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class RejectionEngine : IRejectionEngine
  {
    private readonly IDocumentService _documentService;
    private readonly IRejectionRepository _rejectionRepository;

    public RejectionEngine(IDocumentService documentService, IRejectionRepository rejectionRepository)
    {
      _documentService = documentService;
      _rejectionRepository = rejectionRepository;
    }

    public void RejectDocument(string userName, string documentId, string manCo, string docType, string subDocType)
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

      var rejection = new Rejection
      {
        RejectedBy = userName,
        RejectionDate = DateTime.Now,
        DocumentId = document.Id
      };

      _rejectionRepository.Create(rejection);
    }
  }
}
