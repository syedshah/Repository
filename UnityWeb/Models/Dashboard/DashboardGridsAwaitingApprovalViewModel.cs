namespace UnityWeb.Models.Dashboard
{
  public class DashboardAwaitingApprovalViewModel : BaseDashboardGridRunViewModel
  {
    public DashboardAwaitingApprovalViewModel(string docType, string subDocType, Entities.ManCo manCo, string documentId) : base(manCo)
    {
      DocType = docType;
      SubDocType = subDocType;
      DocumentId = documentId;
    }
  }
}