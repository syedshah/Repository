namespace UnityWeb.Controllers
{
  using Entities;
  using Logging;
  using System.Web.Mvc;
  using ServiceInterfaces;
  using UnityWeb.Constants;
  using UnityWeb.Filters;
  using UnityWeb.Models.DocType;
  using UnityWeb.Models.SubDocType;

  [AuthorizeLoggedInUser(Roles = Roles.DstAdmin)]
  public class SubDocTypeController : BaseController
  {
    private readonly ISubDocTypeService _subDocTypeService;
    private readonly IDocTypeService _docTypeService;

    public SubDocTypeController(ISubDocTypeService subDocTypeService, IDocTypeService docTypeService, ILogger logger)
      : base(logger)
    {
      _subDocTypeService = subDocTypeService;
      _docTypeService = docTypeService;
    }

    public ActionResult Index()
    {
      var subDocTypes = _subDocTypeService.GetSubDocTypes();
      var docTypes = _docTypeService.GetDocTypes();

      var subDocTypesViewModel = new SubDocTypesViewModel(docTypes);

      subDocTypesViewModel.AddSubDocTypes(subDocTypes);

      return View(subDocTypesViewModel);
    }

    [HttpPost]
    public virtual ActionResult Create(AddSubDocTypeViewModel addSubDocTypeViewModel)
    {
      if (ModelState.IsValid)
      {
        _docTypeService.AddSubDocType(addSubDocTypeViewModel.DocTypeId, addSubDocTypeViewModel.Code, addSubDocTypeViewModel.Description);
      }
      else
      {
        TempData["comment"] = "Required files are missing";
      }
      return new RedirectResult(Request.Headers["Referer"]);
    }

   [HttpGet]
    public virtual ActionResult Edit(int subDocTypeId)
    {
      SubDocType subDocType = _subDocTypeService.GetSubDocType(subDocTypeId);
      var docTypes = _docTypeService.GetDocTypes();

     var model = new EditSubDocTypeViewModel()
                   {
                     Code = subDocType.Code,
                     Description = subDocType.Description,
                     SubDocTypeId = subDocType.Id
                   };

     model.AddDocTypes(docTypes);

      return View(model);
    }

    [HttpPost]
    public virtual ActionResult Update(EditSubDocTypeViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Index", "SubDocType");
      }
      return UpdateSubDocType(model);
    }

    private ActionResult UpdateSubDocType(EditSubDocTypeViewModel model)
    {
      _subDocTypeService.Update(model.SubDocTypeId, model.Code, model.Description);
      return RedirectToRoute(new { controller = "SubDocType", action = "Index" });
    }
  }
}
