namespace UnityWeb.File
{
  using System;
  using UnityWeb.Models.Dashboard;

  public class DashboardProcessingViewModel : BaseDashboardGridRunViewModel
  {
    public DashboardProcessingViewModel(string fileName, string bigZip, string docType, Entities.ManCo manCo, DateTime startDate, string grid) : base(manCo)
    {
      FileName = fileName;
      BigZip = bigZip;
      DocType = docType;
      StartDate = startDate;
      Grid = grid;
    }
  }
}