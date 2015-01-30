namespace UnityWeb.Models.Document
{
  public class DocumentStatusViewModel
  {
    public DocumentStatusViewModel()
    {
    }

    public DocumentStatusViewModel(Entities.Document document)
    {
      if (document.GridRun != null)
      {
        Arrived = document.GridRun.XmlFile.Received.ToString("ddd d MMM yyyy HH:mm");
        Proccessed = document.GridRun.EndDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
      }

      if (document.Approval != null)
      {
        Approved = document.Approval.ApprovedDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
        ApprovedBy = document.Approval.ApprovedBy;
      }

      if (document.Rejection != null)
      {
        Rejected = document.Rejection.RejectionDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
        RejectedBy = document.Rejection.RejectedBy;
      }

      if (document.HouseHold != null)
      {
        HouseHeld = document.HouseHold.HouseHoldDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
      }
    }

    public string Arrived { get; set; }

    public string Proccessed { get; set; }

    public string Approved { get; set; }

    public string ApprovedBy { get; set; }

    public string Rejected { get; set; }

    public string RejectedBy { get; set; }

    public string Faxed { get; set; }

    public string Emailed { get; set; }

    public string HouseHeld { get; set; }
  }
}