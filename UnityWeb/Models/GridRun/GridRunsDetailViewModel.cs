namespace UnityWeb.Models.GridRun
{
  using System.Collections.Generic;

  using UnityWeb.Models.Helper;

  public class GridRunsDetailViewModel
  {
    public GridRunsDetailViewModel()
    {
      PagingInfo = new PagingInfoViewModel();
    }

    private List<GridRunDetailViewModel> _gridRuns = new List<GridRunDetailViewModel>();

    public List<GridRunDetailViewModel> GridRuns
    {
      get { return _gridRuns; }
      set { _gridRuns = value; }
    }

    public PagingInfoViewModel PagingInfo { get; set; }

    public void AddGridRuns(Entities.PagedResult<Entities.GridRun> gridRuns, bool houseHolding = false)
    {
      foreach (Entities.GridRun gridRun in gridRuns.Results)
      {
        var gvm = new GridRunDetailViewModel(gridRun, houseHolding);
        GridRuns.Add(gvm);
      }

      PagingInfo = new PagingInfoViewModel
      {
        CurrentPage = gridRuns.CurrentPage,
        TotalItems = gridRuns.TotalItems,
        ItemsPerPage = gridRuns.ItemsPerPage,
        TotalPages = gridRuns.TotalPages,
        StartRow = gridRuns.StartRow,
        EndRow = gridRuns.EndRow
      };
    }
  }
}