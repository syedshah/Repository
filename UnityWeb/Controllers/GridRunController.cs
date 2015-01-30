
namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.Web.Mvc;
  using Entities;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.Dashboard;
  using UnityWeb.Models.GridRun;

  [AuthorizeLoggedInUser]
  public class GridRunController : BaseController
  {
    private readonly IGridRunService _gridRunService;
    private readonly IUserService _userService;
    public int _pageSize = 10;

    public GridRunController(
        IGridRunService gridRunService,
        IUserService userService,
        ILogger logger)
      : base(logger)
    {
      _gridRunService = gridRunService;
      _userService = userService;
    }

    public ActionResult HouseHolding(string houseHoldingGrid, int page = 1)
    {
      var manCoFilterViewModel = new ManCoFilterViewModel();
      var manCos = new List<int>();
      var gridRunsViewModel = new GridRunsDetailViewModel();
      var gridRuns = new PagedResult<GridRun>();

      if (Session["ManCoFilter"] != null)
      {
        manCoFilterViewModel = (ManCoFilterViewModel)Session["ManCoFilter"];
        manCos.Add(int.Parse(manCoFilterViewModel.SelectedManCoId));
        gridRuns = _gridRunService.GetGridRuns(page, _pageSize, houseHoldingGrid, manCos);

        gridRunsViewModel.AddGridRuns(gridRuns, true);
        return View(gridRunsViewModel);
      }

      gridRuns = _gridRunService.GetGridRuns(page, _pageSize, houseHoldingGrid);
      gridRunsViewModel.AddGridRuns(gridRuns, true);
      return View(gridRunsViewModel);
    }

    public ActionResult Unapproved(int page = 1)
    {
      var manCoFilterViewModel  = new ManCoFilterViewModel();
      var manCos = new List<int>();
      var gridRunsViewModel = new GridRunsDetailViewModel();
      var gridRuns = new PagedResult<GridRun>();

      if (Session["ManCoFilter"] != null)
      {
        manCoFilterViewModel = (ManCoFilterViewModel)Session["ManCoFilter"];
        manCos.Add(int.Parse(manCoFilterViewModel.SelectedManCoId));
        gridRuns = _gridRunService.GetUnapproved(page, _pageSize, manCos);

        gridRunsViewModel.AddGridRuns(gridRuns);
        return View(gridRunsViewModel);
      }

      gridRuns = _gridRunService.GetUnapproved(page, _pageSize);
      gridRunsViewModel.AddGridRuns(gridRuns);
      return View(gridRunsViewModel);
    }

    [OutputCache(CacheProfile = "medium", VaryByParam = "grid")]
    public ActionResult Search(string grid)
    {
      var gridSearchesViewModel = new GridRunSearchesViewModel();
      var gridRuns = _gridRunService.Search(grid, this._userService.GetUserManCoIds());
      gridSearchesViewModel.AddGridRuns(gridRuns);
      return View(gridSearchesViewModel);
    }

    public ActionResult Status(int gridRunId)
    {
      var gridRun = _gridRunService.GetGridRun(gridRunId);
      return PartialView("_GridRunStatus", new GridRunStatusViewModel(gridRun));
    }
  }
}
