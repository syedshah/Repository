namespace UnityWeb.Models.KpiReport
{
  public class KpiReportDataViewModel
  {
    public KpiReportDataViewModel(Entities.KpiReportData kpiData)
    {
      this.DocType = kpiData.DocType;
      this.SubDocType = kpiData.SubDocType;
      this.NumberOfDocs = kpiData.NumberOfDocs;
    }

    public string Id { get; set; }

    public string DocType { get; set; }

    public string SubDocType { get; set; }

    public int NumberOfDocs { get; set; }
  }
}