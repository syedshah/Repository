namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using ClientProxies.ArchiveServiceReference;
  using Entities;

  public interface IDocumentService
  {
    PagedResult<IndexedDocumentData> GetDocuments(int startRow, int endRow, string grid, bool filterHouseHolding, bool filterUnapproved, bool filterApproved);

    PagedResult<IndexedDocumentData> GetDocuments(
      int pageNumber,
      int numberOfItems,
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
      string documentNumber);

    byte[] GetDocumentStream(string documentId);

    Document GetDocument(string documentId);

    void AddDocument(string documentId, int docTypeId, int subDocTypeId, int manCoId, int? gridRunId, string mailPrintFlag);
    
    void AddDocument(string documentId, string docType, string subDocType, string manCo, int? gridRunId);

    IList<Document> GetDocuments(string grid);

    IList<Document> GetDocumentsWithApprovalAndRejection(string grid);

    IList<Document> GetUnApprovedDocuments(string grid);

    IList<Document> GetApprovedDocuments(string grid);

    IList<KpiReportData> GetDocuments(int mancoId, DateTime startDate, DateTime endDate);

    IList<Document> GetApprovedAndNotExported(bool offShore);

    void Update(int documentId, int houseHoldingRunId);

    void FilterHouseHolding(DocumentSearchResultsData data);

    void FilterUnapproved(DocumentSearchResultsData data, string grid);

    void FilterApprovedAndRejected(DocumentSearchResultsData data, string grid);
  }
}
