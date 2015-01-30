namespace UnityWeb.Controllers
{
  using System.Web.Mvc;
  using Entities;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.Index;

  [AuthorizeLoggedInUser]
  public class IndexController : BaseController
  {
    private readonly IApplicationService _applicationService;
    private readonly IIndexService _indexService;

    public IndexController(IApplicationService applicationService, IIndexService indexService, ILogger logger)
      : base(logger)
    {
      _applicationService = applicationService;
      _indexService = indexService;
    }

    [HttpGet]
    public virtual ActionResult Edit(int indexId)
    {
      IndexDefinition index = _indexService.GetIndex(indexId);
      return View(new EditIndexViewModel { IndexId = indexId, Name = index.Name, ArchiveName = index.ArchiveName , ArchiveColumn = index.ArchiveColumn });
    }

    [HttpPost]
    public virtual ActionResult Update(EditIndexViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Index", "Application");
      }
      return UpdateIndex(model);
    }

    [HttpPost]
    public virtual ActionResult Create(AddIndexViewModel indexViewModel)
    {
      if (ModelState.IsValid)
      {
        _applicationService.AddIndex(indexViewModel.ApplicaitonId, indexViewModel.Name, indexViewModel.ArchiveName, indexViewModel.ArchiveColumn);
      }
      else
      {
        TempData["comment"] = "Required files are missing";
      }
      return new RedirectResult(Request.Headers["Referer"]);
    }

    private ActionResult UpdateIndex(EditIndexViewModel model)
    {
      _indexService.Update(model.IndexId, model.Name,model.ArchiveName, model.ArchiveColumn);
      return RedirectToRoute(new { controller = "Application", action = "Index" });
    }
  }
}
