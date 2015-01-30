namespace UnityWeb.Models.KpiReport
{
  using System.Collections.Generic;
  using System.Linq;

  public class KpiReportsDataViewModel
  {
    public string ManCo { get; set; }

    public KpiReportViewModel KpiReportViewModel { get; set; }

    private List<KpiReportDataViewModel> _kpiReport = new List<KpiReportDataViewModel>();

    public KpiReportsDataViewModel(KpiReportViewModel kpiReportViewModel)
    {
      KpiReportViewModel = kpiReportViewModel;
    }

    public KpiReportsDataViewModel(IList<Entities.KpiReportData> kpiReportData, KpiReportViewModel kpiReportViewModel)
      : this(kpiReportViewModel)
    {
      if (kpiReportData.Count > 0)
      {
        ManCo = string.Format("{0} - {1}", kpiReportData.First().ManCo.Code, kpiReportData.First().ManCo.Description);
      }
    }

    public List<KpiReportDataViewModel> KpiReport
    {
      get { return this._kpiReport; }
      set { this._kpiReport = value; }
    }

    public void AddKpiData(IEnumerable<Entities.KpiReportData> kpiReportDatas)
    {
      foreach (var kpiReportData in kpiReportDatas)
      {
        var uvm = new KpiReportDataViewModel(kpiReportData);
        this.KpiReport.Add(uvm);
      }
    }
  }
}