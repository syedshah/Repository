namespace UnityWeb.Models.Approval
{
  using System.Collections.Generic;

  public class ApproveDocumentsViewModel
  {
    public List<ApproveDocumentViewModel> ApproveDocumentViewModel { get; set; }

    public string Grid { get; set; }

    public string Page { get; set; }
  }
}