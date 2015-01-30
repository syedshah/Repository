namespace ArchiveWCFRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using AbstractConfigurationManager;
  using ArchiveServiceFactory.ArchiveService;
  using ArchiveServiceFactory.Interfaces;
  using ArchiveWCFRepository.Interface;
  using Entities;

  public class ArchiveRepository : IArchiveRepository
  {
    public ArchiveRepository(IDocumentServiceFactory documentServiceFactory, IConfigurationManager configurationManager)
    {
      _documentServiceFactory = documentServiceFactory;
      _configurationManager = configurationManager;
      _authenticationData = new AuthenticationData()
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

    private readonly IConfigurationManager _configurationManager;

    private readonly IDocumentServiceFactory _documentServiceFactory;

    private AuthenticationData _authenticationData;

    public PagedResult<IndexedDocumentData> GetDocuments(int pageNumber, int numberOfItems, string grid)
    {
      using (var clientChannel = _documentServiceFactory.CreateChannel())
      {
        var proxy = (IDocument)clientChannel;
        var documents = proxy.DocumentSearch(
          _authenticationData,
          1,
          10,
          "",
          grid,
          1,
          "*",
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty);

        return GetPagedResultForQuery(pageNumber, numberOfItems, documents);
      }
    }

    public PagedResult<IndexedDocumentData> GetDocuments(
      int pageNumber, int numberOfItems, List<IndexNameCriteriaData> searchCriteria)
    {
      using (var clientChannel = _documentServiceFactory.CreateChannel())
      {
        var startDate = new DateTime(2000, 1, 1);
        var proxy = (IDocument)clientChannel;
        int endRow;
        var startRow = CalculateStartAndEndRowRumbers(pageNumber, numberOfItems, out endRow);

        var documents = proxy.DocumentSearchMapVerbose(
          _authenticationData, startRow, endRow, 1, startDate, DateTime.Now, searchCriteria);
        return GetPagedResultForQuery(pageNumber, numberOfItems, documents);
      }
    }

    public byte[] GetDocument(string documentId)
    {
      using (var clientChannel = _documentServiceFactory.CreateChannel())
      {
        var proxy = (IDocument)clientChannel;
        return proxy.GetDocument(_authenticationData, documentId);
      }
    }

    private static int CalculateStartAndEndRowRumbers(int pageNumber, int numberOfItems, out int endRow)
    {
      int startRow = (pageNumber - 1) * numberOfItems;
      endRow = startRow + numberOfItems;
      return startRow;
    }

    private static PagedResult<IndexedDocumentData> GetPagedResultForQuery(
      int pageNumber, int numberOfItems, DocumentSearchResultsIndexedData documents)
    {
      return new PagedResult<IndexedDocumentData>
               {
                 CurrentPage = pageNumber,
                 ItemsPerPage = numberOfItems,
                 TotalItems =
                   documents.RecordsFound > 0
                     ? documents.TotalRecordsFound
                     : documents.DocumentsFoundOverLimit.Value,
                 Results = documents.DocumentList
               };
    }

    private static PagedResult<IndexedDocumentData> GetPagedResultForQuery(
      int pageNumber, int numberOfItems, DocumentSearchResultsData documents)
    {
      return new PagedResult<IndexedDocumentData>
               {
                 CurrentPage = pageNumber,
                 ItemsPerPage = numberOfItems,
                 TotalItems =
                   documents.RecordsFound > 0
                     ? documents.TotalRecordsFound
                     : documents.DocumentsFoundOverLimit.Value,
                 Results = documents.DocumentList
               };
    }
  }
}
