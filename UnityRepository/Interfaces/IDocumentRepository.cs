namespace UnityRepository.Interfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IDocumentRepository : IRepository<Document>
  {
    Document GetDocument(int id);

    Document GetDocument(string documentId);

    IList<Document> GetDocuments();

    IList<Document> GetDocuments(string grid);

    IList<Document> GetDocumentsWithApprovalAndRejection(string grid);

    IList<Document> GetUnApprovedDocuments(string grid);

    IList<Document> GetApprovedDocuments(string grid);

    IList<Document> GetApprovedAndNotExported(bool offShore);

    IList<KpiReportData> GetDocuments(int mancoId, DateTime startDate, DateTime endDate);

    void Update(int documentId, int houseHoldingRunId);
  }
}
