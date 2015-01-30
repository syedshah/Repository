namespace ArchiveWCFRepository.Interface
{
  using System.Collections.Generic;
  using ArchiveServiceFactory.ArchiveService;
  using Entities;

  public interface IArchiveRepository
  {
    PagedResult<IndexedDocumentData> GetDocuments(int pageNumber, int numberOfItems, string grid);
    PagedResult<IndexedDocumentData> GetDocuments(int pageNumber, int numberOfItems, List<IndexNameCriteriaData> searchCriteria);
    byte[] GetDocument(string documentId);
  }
}
