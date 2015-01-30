namespace BusinessEngines
{
  using System.Linq;
  using BusinessEngineInterfaces;
  using ClientProxies.ArchiveServiceReference;
  using Entities;

  public class SearchEngine : ISearchEngine
  {
    public void GetStartEndRow(int pageNumber, int pageSize, out int startRow, out int endRow)
    {
      startRow = ((pageNumber - 1) * pageSize) + 1;
      endRow = startRow + (pageSize - 1);
    }

    public PagedResult<IndexedDocumentData> GetPagedResults(int startRow, int endRow, int pageNumber, int itemsPerPage, DocumentSearchResultsData documentSearchResultsData, bool applyFilter = false)
    {
      var documents = (from d in documentSearchResultsData.DocumentList select d).ToList();
      
        //      Results = documentSearchResultsData.DocumentList.OrderBy(c => c.UserName)
        //.Skip((pageNumber - 1) * numberOfItems)
        //.Take(numberOfItems)
        //.ToList(),
      
      return new PagedResult<IndexedDocumentData>()
      {
        CurrentPage = pageNumber,
        ItemsPerPage = itemsPerPage,
        Results = applyFilter ? (from d in documentSearchResultsData.DocumentList select d).Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList() : (from d in documentSearchResultsData.DocumentList select d).ToList(),
        TotalItems = applyFilter ? documents.Count : documentSearchResultsData.TotalRecordsFound,
        StartRow = startRow,
        EndRow = documents.Count < 10 ? (startRow + documents.Count) - 1 : endRow
      };
    }

    public PagedResult<IndexedDocumentData> GetPagedResults(int startRow, int endRow, int pageNumber, int itemsPerPage, DocumentSearchResultsIndexedData documentSearchResultsData)
    {
      var documents = (from d in documentSearchResultsData.DocumentList select d).ToList();

      return new PagedResult<IndexedDocumentData>()
      {
        CurrentPage = pageNumber,
        ItemsPerPage = itemsPerPage,
        Results = (from d in documentSearchResultsData.DocumentList select d).ToList(),
        TotalItems = documentSearchResultsData.TotalRecordsFound,
        StartRow = startRow,
        EndRow = documents.Count < 10 ? (startRow + documents.Count) - 1 : endRow
      };
    }

  }
}
