namespace UnityWeb.Controllers
{
  using System.Web.Mvc;
  using Entities;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Constants;
  using UnityWeb.Filters;
  using UnityWeb.Models.DocType;
  using UnityWeb.Models.ManCo;

  [AuthorizeLoggedInUser(Roles = Roles.DstAdmin)]
  public class ManCoController : BaseController
  {
    private readonly IManCoService _manCoService;

    public ManCoController(IManCoService manCoService, ILogger logger)
      : base(logger)
    {
      _manCoService = manCoService;
    }

    public ActionResult Index()
    {
      var manCosViewModel = new ManCosViewModel();
      var manCos = _manCoService.GetManCos();
      manCosViewModel.AddMancos(manCos);
      return View(manCosViewModel);
    }

    [HttpGet]
    public virtual ActionResult Edit(int manCoId)
    {
      ManCo manCo = _manCoService.GetManCo(manCoId);
      return View(new EditManCoViewModel { ManCoId = manCoId, Code = manCo.Code, Description = manCo.Description });
    }

    [HttpPost]
    public virtual ActionResult Update(EditManCoViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Index", "ManCo");
      }
      return UpdateManCo(model);
    }

    private ActionResult UpdateManCo(EditManCoViewModel model)
    {
      _manCoService.Update(model.ManCoId, model.Code, model.Description);
      return RedirectToRoute(new { controller = "ManCo", action = "Index" });
    }
  }
}
