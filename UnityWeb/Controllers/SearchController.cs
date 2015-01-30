namespace UnityWeb.Controllers
{
  using System;
  using System.Web.Mvc;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.Search;
  using UnityWeb.Models.Dashboard;

  [AuthorizeLoggedInUser]
  public class SearchController : BaseController
  {
    private readonly IDocTypeService _docTypeService;
    private readonly ISubDocTypeService _subDocTypeService;
    private readonly IManCoService _manCoService;
    private readonly IUserService _userService;

    public SearchController(
        IDocTypeService docTypeService, 
        ISubDocTypeService subDocTypeService, 
        IManCoService manCoService, 
        IUserService userService,
        ILogger logger)
      : base(logger)
    {
      _docTypeService = docTypeService;
      _subDocTypeService = subDocTypeService;
      _manCoService = manCoService;
      _userService = userService;
    }
    
    public ActionResult Index()
    {
      var model = new SearchViewModel(); 

      if (Session["SearchViewModel"] != null)
      {
        model = (SearchViewModel)Session["SearchViewModel"];
        model.DocTypes.Clear();
        model.SubDocTypes.Clear();
        model.ManCos.Clear();
        model.AddSubDocTypes(_subDocTypeService.GetSubDocTypes(Convert.ToInt32(model.SelectedDocId)));
      }

      var docTypes = _docTypeService.GetDocTypes();
      var currentUser = this._userService.GetApplicationUser();
      var manCos = this._manCoService.GetManCosByUserId(currentUser.Id);
        
      model.AddDocTypes(docTypes);
      model.AddMancos(manCos);
      if (Session["ManCoFilter"] != null)
      {
        ManCoFilterViewModel   mModel = (ManCoFilterViewModel)Session["ManCoFilter"];
        if (mModel.SelectedManCoId!=null)
        {
          model.SelectedManCoId = mModel.SelectedManCoId;
        } 
     }

      return PartialView("_Index", model);
    }
    
    public ActionResult SubDocType(int docTypeId)
    {
      var subDocTypes = _subDocTypeService.GetSubDocTypes(docTypeId);

      var model = new SubDocTypeJsonResponse();
      model.AddSubDocTypes(subDocTypes);

      return Json(model.SubDocTypes, JsonRequestBehavior.AllowGet);
    }

    public ActionResult Reset()
    {
      Session["SearchViewModel"] = null;
      return RedirectToAction("Index", "Document");
    }
  }
}
