namespace UnityWeb.Models.Dashboard
{
  using System;

  public class DashboardRejectedViewModel : BaseDashboardGridRunViewModel
  {
    public DashboardRejectedViewModel(string fileName, string bigZip, string docType, Entities.ManCo manCo, DateTime startDate, DateTime endDate, string grid) : base(manCo)
    {
      FileName = fileName;
      BigZip = bigZip;
      DocType = docType;
      StartDate = startDate;
      Grid = grid;
      EndDate = endDate;
      Duration = endDate - startDate;
    }

    public DateTime EndDate { get; set; }

    public TimeSpan Duration { get; set; }
  }
}