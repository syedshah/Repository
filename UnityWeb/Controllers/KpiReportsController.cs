namespace UnityWeb.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Web.Mvc;
  using Entities;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using Logging;
  using UnityWeb.Models.KpiReport;

  [AuthorizeLoggedInUser]
  public class KpiReportsController : BaseController
  {
    private readonly IDocumentService _documentService;
    private readonly IManCoService _manCoService;
    private readonly IUserService _userService;

    public KpiReportsController(IDocumentService documentService, IManCoService manCoService, IUserService userService, ILogger logger)
      : base(logger)
    {
      _documentService = documentService;
      _manCoService = manCoService;
      _userService = userService;
    }

    public ActionResult Index()
    {
      var model = new KpiReportViewModel();
      PopulateKpiViewModel(model);
      return this.View(model);
    }

    public ActionResult Run(string submitButton, KpiReportViewModel kpiReportViewModel)
    {
      this.PopulateKpiViewModel(kpiReportViewModel);

      if (!this.ModelState.IsValid)
      {
        var errorList = this.ModelState.Values.SelectMany(x => x.Errors).ToList();
        this.TempData["kpierror"] = errorList[0].ErrorMessage;

        return this.View("Index", kpiReportViewModel);
      }

      var documents = this._documentService.GetDocuments(Convert.ToInt32(kpiReportViewModel.SelectedManCoId), kpiReportViewModel.DateFrom.Value, kpiReportViewModel.DateTo.Value);

      switch (submitButton)
      {
        case "Run":
          return Show(kpiReportViewModel, documents);
        case "Export":
          string manco = string.Format("{0} - {1}", documents.First().ManCo.Code, documents.First().ManCo.Description);
          return Export(documents, manco, kpiReportViewModel.DateFrom.Value.ToShortDateString(), kpiReportViewModel.DateTo.Value.ToShortDateString());
        default:
          return (View());
      }
    }

    private ActionResult Show(KpiReportViewModel kpiReportViewModel, IList<KpiReportData> documents)
    {
      if (documents.Count == 0)
      {
        TempData["noData"] = "There were no documents found for your search criteria.";
        return this.View(new KpiReportsDataViewModel(kpiReportViewModel));
      }

      var kpiReportsDataViewModel = new KpiReportsDataViewModel(documents, kpiReportViewModel);
      kpiReportsDataViewModel.AddKpiData(documents);

      return this.View(kpiReportsDataViewModel);
    }

    private ActionResult Export(IEnumerable<KpiReportData> documents, string manCo, string startDate, string endDate)
    {
      var builder = GetKpiReport(documents, manCo, startDate, endDate);

      return File(new UTF8Encoding().GetBytes(builder.ToString()), "text/csv", string.Format("KPIReport_{0}.csv", DateTime.Now.ToShortTimeString()));
    }

    private void PopulateKpiViewModel(KpiReportViewModel kpiReportViewModel)
    {
      var currentUser = this._userService.GetApplicationUser();
      var manCos = this._manCoService.GetManCosByUserId(currentUser.Id);
      kpiReportViewModel.AddMancos(manCos);
    }

    private StringBuilder GetKpiReport(IEnumerable<KpiReportData> documents, string manCo, string startDate, string endDate)
    {
      var csvBuilder = new StringBuilder();

      csvBuilder.Append("MAN CO: " + manCo);
      csvBuilder.Append(",");

      csvBuilder.Append("START DATE: " + startDate);
      csvBuilder.Append(",");

      csvBuilder.Append("END DATE: " + endDate);
      csvBuilder.Append(",");

      csvBuilder.Append("\n");
      csvBuilder.Append("\n");

      csvBuilder.Append("DOC TYPE");
      csvBuilder.Append(",");

      csvBuilder.Append("SUB DOC ROLE");
      csvBuilder.Append(",");

      csvBuilder.Append("DOCUMENTS");
      csvBuilder.Append(",");

      csvBuilder.Append("\n");

      foreach (var document in documents)
      {
        csvBuilder.Append(document.DocType);
        csvBuilder.Append(",");

        csvBuilder.Append(document.SubDocType);
        csvBuilder.Append(",");

        csvBuilder.Append(document.NumberOfDocs);
        csvBuilder.Append(",");

        csvBuilder.Append("\n");
      }

      return csvBuilder;
    }
  }
}
