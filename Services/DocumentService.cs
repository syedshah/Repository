namespace Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using BusinessEngineInterfaces;
  using ClientProxies.ArchiveServiceReference;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class DocumentService : IDocumentService
  {
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocTypeRepository _docTypeRepositry;
    private readonly ISubDocTypeRepository _subDocTypeRepositry;
    private readonly IManCoRepository _manCoRepository;
    private readonly IIndexNameCriteraService _indexNameCriteraService;
    private readonly IDocument _archiveService;
    private readonly ISearchEngine _documentEngine;
    private AuthenticationData _authenticationData;

    //Constuctor required for NTGEN94
    public DocumentService(
      IDocumentRepository documentRepository,
      IIndexNameCriteraService indexNameCriteraService,
      IDocTypeRepository docTypeRepositry,
      ISubDocTypeRepository subDocTypeRepository,
      IManCoRepository manCoRepository)
    {
      _documentRepository = documentRepository;
      _indexNameCriteraService = indexNameCriteraService;
      _docTypeRepositry = docTypeRepositry;
      _subDocTypeRepositry = subDocTypeRepository;
      _manCoRepository = manCoRepository;
    }

    //Constuctor required for NTGEN99
    public DocumentService(
      IDocumentRepository documentRepository,
      IIndexNameCriteraService indexNameCriteraService,
      ISearchEngine documentEngine,
      IDocTypeRepository docTypeRepositry,
      ISubDocTypeRepository subDocTypeRepository,
      IManCoRepository manCoRepository)
    {
      _documentRepository = documentRepository;
      _indexNameCriteraService = indexNameCriteraService;
      _documentEngine = documentEngine;
      _docTypeRepositry = docTypeRepositry;
      _subDocTypeRepositry = subDocTypeRepository;
      _manCoRepository = manCoRepository;
    }

    public DocumentService(
      IDocument archiveService,
      IDocumentRepository documentRepository,
      IIndexNameCriteraService indexNameCriteraService,
      ISearchEngine documentEngine,
      IDocTypeRepository docTypeRepositry,
      ISubDocTypeRepository subDocTypeRepository,
      IManCoRepository manCoRepository)
    {
      _archiveService = archiveService;
      _documentRepository = documentRepository;
      _indexNameCriteraService = indexNameCriteraService;
      _documentEngine = documentEngine;
      _docTypeRepositry = docTypeRepositry;
      _subDocTypeRepositry = subDocTypeRepository;
      _manCoRepository = manCoRepository;
    }

    public PagedResult<IndexedDocumentData> GetDocuments(int pageNumber, int itemsPerPage, string grid, bool filterHouseHeld = false, bool filterUnapproved = false, bool filterApproved = false)
    {
      try
      {
        int startRow;
        int endRow;
        _documentEngine.GetStartEndRow(pageNumber, itemsPerPage, out startRow, out endRow);

        GetAuthData();

        DocumentSearchResultsData data;

        if (filterHouseHeld || filterUnapproved || filterApproved)
        {
          data = _archiveService.DocumentSearch(_authenticationData, 1, 1000, "", grid, 1, "*", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);  
        }
        else
        {
          data = _archiveService.DocumentSearch(_authenticationData, startRow, endRow, "", grid, 1, "*", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);  
        }

        if (filterHouseHeld)
        {
          FilterHouseHolding(data);
        }

        if (filterUnapproved)
        {
          FilterUnapproved(data, grid);
        }

        if (filterApproved)
        {
          FilterApprovedAndRejected(data, grid);
        }

        return _documentEngine.GetPagedResults(startRow, endRow, pageNumber, itemsPerPage, data, (filterHouseHeld || filterUnapproved || filterApproved));
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to search for documents", e);
      }
    }

    public void FilterHouseHolding(DocumentSearchResultsData data)
    {
      for (int i = (data.DocumentList.Count - 1); i >= 0; i--)
      {
        Document document = this._documentRepository.GetDocument((data.DocumentList[i].Id).ToString());

        if (document.HouseHold == null)
        {
          data.DocumentList.RemoveAt(i);
        }
      }
    }

    public void FilterUnapproved(DocumentSearchResultsData data, string grid)
    {
      var approvedDocs = this.GetDocumentsWithApprovalAndRejection(grid);

      for (int i = (data.DocumentList.Count - 1); i >= 0; i--)
      {
        Document document = approvedDocs.FirstOrDefault(d => d.DocumentId == data.DocumentList[i].Id.ToString());

        if (document.Approval == null)
        {
          data.DocumentList.RemoveAt(i);
        }
      }
    }

    public void FilterApprovedAndRejected(DocumentSearchResultsData data, string grid)
    {
      var approvedDocs = this.GetDocumentsWithApprovalAndRejection(grid);

      for (int i = (data.DocumentList.Count - 1); i >= 0; i--)
      {
        Document document = approvedDocs.FirstOrDefault(d => d.DocumentId == data.DocumentList[i].Id.ToString());

        if (document.Approval != null || document.Rejection != null)
        {
          data.DocumentList.RemoveAt(i);
        }
      }
    }

    public PagedResult<IndexedDocumentData> GetDocuments(
      int pageNumber,
      int itemsPerPage,
      string docType,
      string subDocType,
      string addresseeSubType,
      string accountNumber,
      string mailingName,
      IList<string> managementCompanies,
      string investorReference,
      DateTime? processingDateFrom,
      DateTime? processingDateTo,
      string primaryHolder,
      string agentReference,
      string addresseePostCode,
      string emailAddress,
      string faxNumber,
      DateTime? contractDate,
      DateTime? paymentDate,
      string documentNumber)
    {
      try
      {
        int startRow;
        int endRow;
        _documentEngine.GetStartEndRow(pageNumber, itemsPerPage, out startRow, out endRow);

        GetAuthData();

        var searchCriteria = _indexNameCriteraService.BuildSearchCriteria(
          docType,
          subDocType,
          addresseeSubType,
          accountNumber,
          mailingName,
          managementCompanies,
          investorReference,
          processingDateFrom,
          processingDateTo,
          primaryHolder,
          agentReference,
          addresseePostCode,
          emailAddress,
          faxNumber,
          contractDate,
          paymentDate,
          documentNumber);

          var fromDate = processingDateFrom == null ? new DateTime(2000, 1, 1) : processingDateFrom;
          var toDate = processingDateTo == null ? DateTime.Now : processingDateTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

          var data = _archiveService.DocumentSearchMapVerbose(_authenticationData, startRow, endRow, 1, fromDate, toDate, searchCriteria);
        
        return _documentEngine.GetPagedResults(startRow, endRow, pageNumber, itemsPerPage, data);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to search for documents", e);
      }
    }

    public byte[] GetDocumentStream(string documentId)
    {
      try
      {
        GetAuthData();
        return _archiveService.GetDocument(_authenticationData, documentId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve document", e);
      }
    }

    public Document GetDocument(string documentId)
    {
      try
      {
        return _documentRepository.GetDocument(documentId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve document", e);
      }
    }

    public void AddDocument(string documentId, int docTypeId, int subDocTypeId, int manCoId, int? gridRunId, string mailPrintFlag)
    {
      try
      {
        var document = new Document(mailPrintFlag)
                         {
                           DocumentId = documentId,
                           DocTypeId = docTypeId,
                           SubDocTypeId = subDocTypeId,
                           ManCoId = manCoId,
                           GridRunId = gridRunId
                         };

        _documentRepository.Create(document);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to add document", e);
      }
    }

    public void AddDocument(string documentId, string docTypeCode, string subDocTypeCode, string manCoCode, int? gridRunId)
    {
      try
      {
        var docType = _docTypeRepositry.GetDocType(docTypeCode);
        var subDocType = _subDocTypeRepositry.GetSubDocType(subDocTypeCode);
        var manCo = _manCoRepository.GetManCo(manCoCode);
        AddDocument(documentId, docType.Id, subDocType.Id, manCo.Id, gridRunId, null);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to add document", e);
      }
    }

    public IList<Document> GetDocuments(string grid)
    {
      try
      {
        return _documentRepository.GetDocuments(grid);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve documents", e);
      }
    }

    public IList<Document> GetDocumentsWithApprovalAndRejection(string grid)
    {
      try
      {
        return _documentRepository.GetDocumentsWithApprovalAndRejection(grid);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve unapproved documents", e);
      }
    }

    public IList<Document> GetUnApprovedDocuments(string grid)
    {
      try
      {
        return _documentRepository.GetUnApprovedDocuments(grid);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve unapproved documents", e);
      }
    }

    public IList<Document> GetApprovedDocuments(string grid)
    {
      try
      {
        return _documentRepository.GetApprovedDocuments(grid);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve unapproved documents", e);
      }
    }

    public IList<KpiReportData> GetDocuments(int mancoId, DateTime startDate, DateTime endDate)
    {
      try
      {
        return _documentRepository.GetDocuments(mancoId, startDate, endDate);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieved documents", e);
      }
    }

    public IList<Document> GetApprovedAndNotExported(bool offShore)
    {
      try
      {
        return _documentRepository.GetApprovedAndNotExported(offShore);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve approved and not exported documents", e);
      }
    }

    public void Update(int documentId, int houseHoldingRunId)
    {
      try
      {
        _documentRepository.Update(documentId, houseHoldingRunId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update document", e);
      }
    }

    private void GetAuthData()
    {
      this._authenticationData = new AuthenticationData()
      {
        Source = "Unity",
        Destination = "NT-Archive",
        UserName = "paul",
        PassToken = "9F33A7C798AF6FD6ABB28049D9C1B3EDFA2FD24A",
        RequestId = "1",
        CurrentDateTime = DateTime.Now,
        OriginatingUser = "unity"
      };
    }
  }
}
