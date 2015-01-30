namespace UnityWeb.Models.Dashboard
{
  using System;

  public class BaseDashboardGridRunViewModel
  {
    public BaseDashboardGridRunViewModel(Entities.ManCo manCo)
    {
      ManCo = string.Format("{0} - {1}", manCo.Code, manCo.Description);
    }

    public string FileName { get; set; }

    public string Grid { get; set; }

    public string BigZip { get; set; }

    public string DocType { get; set; }

    public string SubDocType { get; set; }

    public string ManCo { get; set; }

    public string DocumentId { get; set; }

    public DateTime StartDate { get; set; }
  }
}