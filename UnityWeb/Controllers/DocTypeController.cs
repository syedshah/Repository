namespace UnityWeb.Controllers
{
  using Entities;
  using Logging;
  using System.Web.Mvc;
  using ServiceInterfaces;
  using UnityWeb.Constants;
  using UnityWeb.Filters;
  using UnityWeb.Models.DocType;

 [AuthorizeLoggedInUser(Roles = Roles.DstAdmin)]
  public class DocTypeController : BaseController
  {
    private readonly IDocTypeService _docTypeService;

    public DocTypeController(IDocTypeService docTypeService, ILogger logger)
      : base(logger)
    {
      _docTypeService = docTypeService;
    }

    public ActionResult Index()
    {
      var docTypesViewModel = new DocTypesViewModel();
      var docTypes = _docTypeService.GetDocTypes();
      docTypesViewModel.AddDocTypes(docTypes);
      return View(docTypesViewModel);
    }

    [HttpGet]
    public virtual ActionResult Edit(int docTypeId)
    {
      DocType docType = _docTypeService.GetDocType(docTypeId);
      return View(new EditDocTypeViewModel { DocTypeId = docTypeId, Code = docType.Code, Description = docType.Description });
    }

    [HttpPost]
    public virtual ActionResult Update(EditDocTypeViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Index", "DocType");
      }
      return UpdateDocType(model);
    }

    private ActionResult UpdateDocType(EditDocTypeViewModel model)
    {
      _docTypeService.Update(model.DocTypeId, model.Code, model.Description);
      return RedirectToRoute(new { controller = "DocType", action = "Index" });
    }
  }
}
