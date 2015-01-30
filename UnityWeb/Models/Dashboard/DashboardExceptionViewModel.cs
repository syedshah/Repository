namespace UnityWeb.Models.Dashboard
{
  using System;

  public class DashboardExceptionViewModel : BaseDashboardGridRunViewModel
  {
    public DashboardExceptionViewModel(string fileName, string docType, Entities.ManCo manCo, DateTime startDate, string grid) : base(manCo)
    {
      FileName = fileName;
      DocType = docType;
      StartDate = startDate;
      Grid = grid;
    }
  }
}