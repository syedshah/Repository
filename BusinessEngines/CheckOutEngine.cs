namespace BusinessEngines
{
  using System;
  using BusinessEngineInterfaces;
  using Entities;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class CheckOutEngine : ICheckOutEngine
  {
    private readonly ICheckOutRepository _checkOutRepository;
    private readonly IDocumentService _documentService;

    public CheckOutEngine(ICheckOutRepository checkOutRepository, 
      IDocumentService documentService)
    {
      _checkOutRepository = checkOutRepository;
      _documentService = documentService;
    }

    public bool IsDocumentCheckedOut(string documentId)
    {
      bool checkedOut = false;

      var currentCheckOut = _checkOutRepository.GetCheckOut(documentId);

      if (currentCheckOut != null)
      {
        checkedOut = true;
      }

      return checkedOut;
    }

    public CheckOut CheckOutDocument(string userName, string documentId, string manCo, string docType, string subDocType)
    {
      CheckOut checkOut = null;

      var document = _documentService.GetDocument(documentId);

      if (document == null)
      {
        _documentService.AddDocument(documentId, docType, subDocType, manCo, null);
        document = _documentService.GetDocument(documentId);
      }

      checkOut = new CheckOut
                   {
                     CheckOutBy = userName, 
                     CheckOutDate = DateTime.Now,
                     DocumentId = document.Id
                   };

      _checkOutRepository.Create(checkOut);
      return checkOut;
    }
  }
}
