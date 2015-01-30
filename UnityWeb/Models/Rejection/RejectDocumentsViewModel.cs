namespace UnityWeb.Models.Rejection
{
  using System.Collections.Generic;

  public class RejectDocumentsViewModel
  {
    public List<RejectDocumentViewModel> RejectDocumentViewModel { get; set; }

    public string Grid { get; set; }

    public string Page { get; set; }
  }
}