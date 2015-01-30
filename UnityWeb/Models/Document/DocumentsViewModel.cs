namespace UnityWeb.Models.Document
{
  using System.Collections.Generic;
  using ClientProxies.ArchiveServiceReference;
  using UnityWeb.Models.Helper;

  public class DocumentsViewModel
  {
    public DocumentsViewModel()
    {
      PagingInfo = new PagingInfoViewModel();
    }

    public DocumentsViewModel(string gridSearch, bool filterHouseHolding)
      : this()
    {
      GridSearch = gridSearch;
      FilterHouseHolding = filterHouseHolding;
    }

    public string GridSearch { get; set; }

    public int DocumentsInGrid { get; set; }

    public int DocsAwaitingApproval { get; set; }

    public int DocsApproved { get; set; }

    private List<DocumentViewModel> _documents = new List<DocumentViewModel>();

    public List<DocumentViewModel> Documents
    {
      get { return _documents; }
      set { _documents = value; }
    }

    public bool FilterHouseHolding { get; set; }

    public PagingInfoViewModel PagingInfo { get; set; }

    public void AddDocuments(Entities.PagedResult<IndexedDocumentData> documents)
    {
      foreach (var document in documents.Results)
      {
        var dvm = new DocumentViewModel(document);

        Documents.Add(dvm);
      }

      PagingInfo = new PagingInfoViewModel
      {
        CurrentPage = documents.CurrentPage,
        TotalItems = documents.TotalItems,
        ItemsPerPage = documents.ItemsPerPage,
        TotalPages = documents.TotalPages,
        StartRow = documents.StartRow,
        EndRow = documents.EndRow
      };
    }

    public void AddTotalDocs(int documentsInGrid)
    {
      DocumentsInGrid = documentsInGrid;
    }

    public void AddDocsAwaitingApproval(int docsAwaitingApproval)
    {
      DocsAwaitingApproval = docsAwaitingApproval;
    }

    public void AddDocsApproved(int docsApproved)
    {
      DocsApproved = docsApproved;
    }
  }
}