namespace BusinessEngineInterfaces
{
  using ClientProxies.ArchiveServiceReference;
  using Entities;

  public interface ISearchEngine
  {
    void GetStartEndRow(int pageNumber, int pageSize, out int startRow, out int endRow);
    PagedResult<IndexedDocumentData> GetPagedResults(int startRow, int endRow, int pageNumber, int itemsPerPage, DocumentSearchResultsData documentSearchResultsData, bool filterHouseHeld);
    PagedResult<IndexedDocumentData> GetPagedResults(int startRow, int endRow, int pageNumber, int itemsPerPage, DocumentSearchResultsIndexedData documentSearchResultsData);
  }
}
