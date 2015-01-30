namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.Web.Mvc;
  using Entities;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.Dashboard;

  [AuthorizeLoggedInUser]
  public class DashboardController : BaseController
  {
    private readonly IGridRunService _gridRunService;
    private readonly ISyncService _syncService;
    private readonly IManCoService _manCoService;
    private readonly IUserService _userService;
    private readonly IHouseHoldingRunService _houseHoldingRunService;

    public DashboardController(
        IGridRunService gridRunService, 
        ISyncService syncService, 
        ILogger logger,
        IManCoService manCoService, 
        IUserService userService,
        IHouseHoldingRunService houseHoldingRunService)
      : base(logger)
    {
      _gridRunService = gridRunService;
      _syncService = syncService;
      _manCoService = manCoService;
      _userService = userService;
      _houseHoldingRunService = houseHoldingRunService;
    }

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] 
    public ActionResult Index(ManCoFilterViewModel manCoFilterViewModel)
    {
      var model = new DashboardViewModel();
      if (!string.IsNullOrEmpty(manCoFilterViewModel.SelectedManCoId))
      {
        if (manCoFilterViewModel.SelectedManCoId == "0")
        {
          Session["ManCoFilter"] = null;
        }
        else
        {
          Session["ManCoFilter"] = manCoFilterViewModel;
          model.ManCosToFilter.Add(int.Parse(manCoFilterViewModel.SelectedManCoId));
        }
      }
      else if (Session["ManCoFilter"] != null)
      {
        manCoFilterViewModel = (ManCoFilterViewModel)Session["ManCoFilter"];
        model.ManCosToFilter.Add(int.Parse(manCoFilterViewModel.SelectedManCoId));
      }

      _syncService.Synchronise();      
      
      GetProcessing(model);
      GetRecentlyProcessed(model);
      GetRecentExceptions(model);
      GetGridsUnapproved(model);
      GetGridsRejected(model);
      GetHouseHoldeldGrids(model);
      GetGridsWaitingHouseHolding(model);
      return View(model);
    }

    public ActionResult ManCoDropDown()
    {
      var manCoFilterViewModel = new ManCoFilterViewModel();
      if (Session != null &&  Session["ManCoFilter"] != null)
      {
        manCoFilterViewModel = (ManCoFilterViewModel)Session["ManCoFilter"];
        manCoFilterViewModel.ManCos.Clear();
      } 
      var currentUser = this._userService.GetApplicationUser();
      var manCos = this._manCoService.GetManCosByUserId(currentUser.Id);
      manCoFilterViewModel.AddMancos(manCos);
      return PartialView("_ManCoFilter", manCoFilterViewModel);
    }

    [OutputCache(CacheProfile = "short", VaryByParam = "dashBoardViewModel")]
    private void GetProcessing(DashboardViewModel dashBoardViewModel)
    {
      IList<GridRun> processingGridRuns;
      processingGridRuns = dashBoardViewModel.ManCosToFilter.Count > 0 ? this._gridRunService.GetProcessing(dashBoardViewModel.ManCosToFilter) : this._gridRunService.GetProcessing();
      dashBoardViewModel.AddProcessing(processingGridRuns);
    }

    [OutputCache(CacheProfile = "short", VaryByParam = "dashBoardViewModel")]
    private void GetRecentlyProcessed(DashboardViewModel dashBoardViewModel)
    {
      IList<GridRun> recentlyProcessed;
      if (dashBoardViewModel.ManCosToFilter.Count > 0)
      {
        recentlyProcessed = _gridRunService.GetTopFifteenSuccessfullyCompleted(dashBoardViewModel.ManCosToFilter);
      }
      else
      {
        recentlyProcessed = _gridRunService.GetTopFifteenSuccessfullyCompleted();
      }
      dashBoardViewModel.AddRecent(recentlyProcessed);
    }

    [OutputCache(CacheProfile = "short", VaryByParam = "dashBoardViewModel")]
    private void GetRecentExceptions(DashboardViewModel dashBoardViewModel)
    {
      IList<GridRun> recentExceptions ;
      if (dashBoardViewModel.ManCosToFilter.Count > 0)
      {
        recentExceptions = _gridRunService.GetTopFifteenRecentExceptions(dashBoardViewModel.ManCosToFilter);
      }
      else
      {
        recentExceptions = _gridRunService.GetTopFifteenRecentExceptions();
      } 
      dashBoardViewModel.AddExceptions(recentExceptions);
    }

    [OutputCache(CacheProfile = "short", VaryByParam = "dashBoardViewModel")]
    private void GetGridsUnapproved(DashboardViewModel dashBoardViewModel)
    {
      IList<GridRun> gridsUnapproved;

      gridsUnapproved = dashBoardViewModel.ManCosToFilter.Count > 0 ? this._gridRunService.GetTopFifteenRecentUnapprovedGrids(dashBoardViewModel.ManCosToFilter) : this._gridRunService.GetTopFifteenRecentUnapprovedGrids(); 

      dashBoardViewModel.AddUnapprovedGrids(gridsUnapproved);
    }

    [OutputCache(CacheProfile = "short", VaryByParam = "dashBoardViewModel")]
    private void GetGridsRejected(DashboardViewModel dashBoardViewModel)
    {
      IList<GridRun> gridsRejected;

      gridsRejected = dashBoardViewModel.ManCosToFilter.Count > 0 ? this._gridRunService.GetTopFifteenGridsWithRejectedDocuments(dashBoardViewModel.ManCosToFilter) : this._gridRunService.GetTopFifteenGridsWithRejectedDocuments();

      dashBoardViewModel.AddRejectedGrids(gridsRejected);
    }

    [OutputCache(CacheProfile = "short", VaryByParam = "dashBoardViewModel")]
    private void GetHouseHoldeldGrids(DashboardViewModel dashBoardViewModel)
    {
      IList<HouseHoldingRun> gridsHouseHeld;

      gridsHouseHeld = dashBoardViewModel.ManCosToFilter.Count > 0 ? this._houseHoldingRunService.GetTopFifteenRecentHouseHeldGrids(dashBoardViewModel.ManCosToFilter) : this._houseHoldingRunService.GetTopFifteenRecentHouseHeldGrids();

      dashBoardViewModel.AddHouseHeldGrids(gridsHouseHeld);
    }

    [OutputCache(CacheProfile = "short", VaryByParam = "dashBoardViewModel")]
    private void GetGridsWaitingHouseHolding(DashboardViewModel dashBoardViewModel)
    {
      IList<GridRun> gridsWaitingHouseHolding;

      gridsWaitingHouseHolding = dashBoardViewModel.ManCosToFilter.Count > 0 ? this._gridRunService.GetTopFifteenGridsAwaitingHouseHolding(dashBoardViewModel.ManCosToFilter) : this._gridRunService.GetTopFifteenGridsAwaitingHouseHolding();

      dashBoardViewModel.AddAwaitingHouseHolding(gridsWaitingHouseHolding);
    }
  }
}
