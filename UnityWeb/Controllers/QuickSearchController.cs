namespace UnityWeb.Controllers
{
  using System.Linq;
  using System.Web.Mvc;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.QuickSearch;

  [AuthorizeLoggedInUser]
  public class QuickSearchController : BaseController
  {
    private readonly IGridRunService _gridRunService;
    private readonly IXmlFileService _xmlFileService;
    private readonly IZipFileService _zipFileService;
    private readonly IUserService _userService;

    public QuickSearchController(
        IGridRunService gridRunService, 
        IXmlFileService xmlFileService,
        IZipFileService zipFileService,
        IUserService userService,
        ILogger logger)
      : base(logger)
    {
      _gridRunService = gridRunService;
      _xmlFileService = xmlFileService;
      this._zipFileService = zipFileService;
      this._userService = userService;
    }

    [ChildActionOnly]
    [OutputCache(Duration = 180)]
    public ActionResult Index()
    {
      return PartialView("_Index", new QuickSearchViewModel());
    }

    [OutputCache(CacheProfile = "long", VaryByParam = "term")]
    public ActionResult Grid(string term)
    {
      ////TODO Move this out of here
        var gridRuns = _gridRunService.Search(term, this._userService.GetUserManCoIds())
                          .Take(10)
                          .OrderBy(g => g.Grid)
                          .Select(r => new { label = r.Grid });

      return Json(gridRuns, JsonRequestBehavior.AllowGet);
    }

    [OutputCache(CacheProfile = "long", VaryByParam = "term")]
    public ActionResult File(string term)
    {
      ////TODO Move this out of here
        var gridRuns = _xmlFileService.Search(term, this._userService.GetUserManCoIds())
                          .Take(10)
                          .OrderBy(f => f.FileName)
                          .Select(r => new { label = r.FileName });

      return Json(gridRuns, JsonRequestBehavior.AllowGet);
    }

    [OutputCache(CacheProfile = "long", VaryByParam = "term")]
    public ActionResult BigZip(string term)
    {
      ////TODO Move this out of here
        var gridRuns = _zipFileService.SearchBigZip(term, this._userService.GetUserManCoIds())
                            .Take(10)
                            .OrderBy(f => f.FileName)
                            .Select(r => new { label = r.FileName });

      return Json(gridRuns, JsonRequestBehavior.AllowGet);
    }
  }
}
