namespace UnityWeb.Models.GridRun
{
  using System.Linq;

  public class GridRunStatusViewModel
  {
    public GridRunStatusViewModel(Entities.GridRun gridRun)
    {
      Grid = gridRun.Grid;

      if (gridRun.XmlFile != null)
      {
        Arrived = gridRun.XmlFile.Received.ToString("ddd d MMM yyyy HH:mm");
        Proccessed = gridRun.EndDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
      }

      if (gridRun.Documents != null && gridRun.Documents.Count > 0)
      {
        if (gridRun.Documents.All(d => d.Approval == null) && gridRun.Documents.All(d => d.Rejection == null))
        {
          ApprovalStatus = (int)ApprovalStatuses.Unapproved;
        }
        else if (gridRun.Documents.All(d => d.Approval != null))
        {
          ApprovalStatus = (int)ApprovalStatuses.FullyApproved;

          var document = (from g in gridRun.Documents
                          where g.Approval != null
                          orderby g.Approval.ApprovedDate descending 
                          select g).First();

          ApprovedBy = document.Approval.ApprovedBy;
          ApprovalDate = document.Approval.ApprovedDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
        }
        else if (gridRun.Documents.Any(d => d.Approval != null))
        {
          ApprovalStatus = (int)ApprovalStatuses.PartiallyApproved;

          var document = (from g in gridRun.Documents
                          where g.Approval != null
                          orderby g.Approval.ApprovedDate descending
                          select g).First();

          ApprovedBy = document.Approval.ApprovedBy;
          ApprovalDate = document.Approval.ApprovedDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
        }
        else if (gridRun.Documents.All(d => d.Rejection != null))
        {
          ApprovalStatus = (int)ApprovalStatuses.FullyRejected;

          var document = (from g in gridRun.Documents
                          where g.Rejection != null
                          orderby g.Rejection.RejectionDate descending
                          select g).First();

          RejectedBy = document.Rejection.RejectedBy;
          RejectedDate = document.Rejection.RejectionDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
        }

        if (gridRun.Documents.All(d => d.HouseHold != null))
        {
          HouseHoldStatus = (int)HouseHoldingStatuses.FullyHouseHeld;
          HouseHoldingDate = gridRun.Documents.Last().HouseHold.HouseHoldDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
        }
        else if (gridRun.Documents.Any(d => d.HouseHold != null))
        {
          HouseHoldStatus = (int)HouseHoldingStatuses.PartiallyHouseHeld;
          HouseHoldingDate = (from d in gridRun.Documents
                             where d.HouseHold != null
                             orderby d.HouseHold.HouseHoldDate descending 
                             select d).First().HouseHold.HouseHoldDate.GetValueOrDefault().ToString("ddd d MMM yyyy HH:mm");
        }
      }
    }

    public enum ApprovalStatuses
    {
      Unapproved,
      FullyApproved,
      PartiallyApproved,
      FullyRejected
    };

    public enum HouseHoldingStatuses
    {
      NonHouseHeld,
      PartiallyHouseHeld,
      FullyHouseHeld
    };

    public string Grid { get; set; }

    public string Arrived { get; set; }

    public int ApprovalStatus { get; set; }

    public string ApprovedBy { get; set; }

    public string ApprovalDate { get; set; }

    public string RejectedBy { get; set; }

    public string RejectedDate { get; set; }

    public int HouseHoldStatus { get; set; }

    public string HouseHoldingDate { get; set; }

    public string Proccessed { get; set; }

    public string HouseHeld { get; set; }
  }
}