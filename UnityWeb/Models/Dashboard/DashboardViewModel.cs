namespace UnityWeb.Models.Dashboard
{
  using System.Collections.Generic;
  using UnityWeb.File;

  public class DashboardViewModel
  {
    public DashboardViewModel()
    {
      DashboardProcessingViewModels = new List<DashboardProcessingViewModel>();
      DashboardProcessedViewModels = new List<DashboardProcessedViewModel>();
      DashboardExceptionViewModels = new List<DashboardExceptionViewModel>();
      DashboardUnapporvedGridsViewModels = new List<DashboardUnapporvedGridsViewModel>();
      DashboardAwaitingApprovalViewModels = new List<DashboardAwaitingApprovalViewModel>();
      DashboardRejectedViewModels = new List<DashboardRejectedViewModel>();
      DashboardHouseHeldViewModel = new List<DashboardHouseHeldViewModel>();
      DashboardGridsAwaitingHouseHoldingViewModel = new List<DashboardGridsAwaitingHouseHoldingViewModel>();
      ManCosToFilter = new List<int>();
    }

    public List<DashboardProcessingViewModel> DashboardProcessingViewModels { get; set; }

    public List<DashboardProcessedViewModel> DashboardProcessedViewModels { get; set; }

    public List<DashboardExceptionViewModel> DashboardExceptionViewModels { get; set; }

    public List<DashboardUnapporvedGridsViewModel> DashboardUnapporvedGridsViewModels { get; set; }

    public List<DashboardAwaitingApprovalViewModel> DashboardAwaitingApprovalViewModels { get; set; }

    public List<DashboardRejectedViewModel> DashboardRejectedViewModels { get; set; }

    public List<DashboardHouseHeldViewModel> DashboardHouseHeldViewModel { get; set; }

    public List<DashboardGridsAwaitingHouseHoldingViewModel> DashboardGridsAwaitingHouseHoldingViewModel { get; set; }

    public List<int> ManCosToFilter { get; set; }

    public void AddProcessing(IList<Entities.GridRun> gridRuns)
    {
      foreach (var gridRun in gridRuns)
      {
        if (gridRun.XmlFile != null)
        {
            var viewModel = new DashboardProcessingViewModel(
            gridRun.XmlFile.FileName, gridRun.XmlFile.BigZip, gridRun.XmlFile.DocType.Description, gridRun.XmlFile.ManCo, gridRun.StartDate.GetValueOrDefault(), gridRun.Grid);
            DashboardProcessingViewModels.Add(viewModel);      
        }
      }
    }

    public void AddRecent(IList<Entities.GridRun> grdRuns)
    {
      foreach (var gridRun in grdRuns)
      {
          if (gridRun.XmlFile != null)
          {
              var viewModel = new DashboardProcessedViewModel(
          gridRun.XmlFile.FileName, gridRun.XmlFile.BigZip, gridRun.XmlFile.DocType.Description, gridRun.XmlFile.ManCo, gridRun.StartDate.GetValueOrDefault(), gridRun.EndDate.Value, gridRun.Grid);
              DashboardProcessedViewModels.Add(viewModel);
          }
      }
    }

    public void AddExceptions(IList<Entities.GridRun> grdRuns)
    {
      foreach (var gridRun in grdRuns)
      {
          if (gridRun.XmlFile != null)
          {
              var viewModel = new DashboardExceptionViewModel(
          gridRun.XmlFile.FileName, gridRun.XmlFile.DocType.Description, gridRun.XmlFile.ManCo, gridRun.StartDate.GetValueOrDefault(), gridRun.Grid);
              DashboardExceptionViewModels.Add(viewModel);
          }
      }
    }

    public void AddUnapprovedGrids(IList<Entities.GridRun> grdRuns)
    {
      foreach (var gridRun in grdRuns)
      {
          if (gridRun.XmlFile != null && gridRun.EndDate != null)
          {
              var viewModel = new DashboardUnapporvedGridsViewModel(
           gridRun.XmlFile.FileName, gridRun.XmlFile.BigZip, gridRun.XmlFile.DocType.Description, gridRun.XmlFile.ManCo, gridRun.StartDate.GetValueOrDefault(), gridRun.EndDate.Value, gridRun.Grid);

              DashboardUnapporvedGridsViewModels.Add(viewModel);
          }
      }
    }

    public void AddRejectedGrids(IList<Entities.GridRun> gridRuns)
    {
      foreach (var gridRun in gridRuns)
      {
          if (gridRun.XmlFile != null)
          {
              var viewModel = new DashboardRejectedViewModel(
           gridRun.XmlFile.FileName, gridRun.XmlFile.BigZip, gridRun.XmlFile.DocType.Description, gridRun.XmlFile.ManCo, gridRun.StartDate.GetValueOrDefault(), gridRun.EndDate.Value, gridRun.Grid);

              DashboardRejectedViewModels.Add(viewModel);
          }
      }
    }

    public void AddAwaitingApproval(IList<Entities.Document> documents)
    {
      foreach (var document in documents)
      {
        var viewModel = new DashboardAwaitingApprovalViewModel(
           document.DocType.Description, document.SubDocType.Description, document.ManCo, document.DocumentId);
        DashboardAwaitingApprovalViewModels.Add(viewModel);
      }
    }

    public void AddAwaitingHouseHolding(IList<Entities.GridRun> gridRuns)
    {
      foreach (var gridRun in gridRuns)
      {
          if (gridRun.XmlFile != null)
          {
              var viewModel = new DashboardGridsAwaitingHouseHoldingViewModel(
           gridRun.XmlFile.FileName, gridRun.XmlFile.BigZip, gridRun.XmlFile.DocType.Description, gridRun.XmlFile.ManCo, gridRun.StartDate.GetValueOrDefault(), gridRun.EndDate.Value, gridRun.Grid);

              DashboardGridsAwaitingHouseHoldingViewModel.Add(viewModel);
          }
      }
    }

    public void AddHouseHeldGrids(IList<Entities.HouseHoldingRun> houseHoldingRuns)
    {
      foreach (var houseHoldingRun in houseHoldingRuns)
      {
        var viewModel = new DashboardHouseHeldViewModel(
          houseHoldingRun.Grid, houseHoldingRun.StartDate, houseHoldingRun.EndDate);
        DashboardHouseHeldViewModel.Add(viewModel);
      }
    }
  }
}