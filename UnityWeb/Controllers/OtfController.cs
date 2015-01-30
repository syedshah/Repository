namespace UnityWeb.Controllers
{
  using System;
  using System.Web.Mvc;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Constants;
  using UnityWeb.Filters;
  using UnityWeb.Models.Otf;

  [AuthorizeLoggedInUser(Roles = Roles.Admin + "," + Roles.DstAdmin)]
  public class OtfController : BaseController
  {
    private readonly IAppManCoEmailService _appManCoEmailService;
    private readonly IApplicationService _applicationService;
    private readonly IManCoService _manCoService;
    private readonly IDocTypeService _docTypeService;
    private readonly IUserService _userService;
    public int PageSize = 10;

    public OtfController(
        IAppManCoEmailService appManCoEmailService, 
        IApplicationService applicationService,
        IManCoService manCoService,
        IDocTypeService docTypeService,
        IUserService userService,
        ILogger logger)
        : base(logger)
    {
      this._appManCoEmailService = appManCoEmailService;
      this._applicationService = applicationService;
      this._manCoService = manCoService;
      this._docTypeService = docTypeService;
      this._userService = userService;
    }

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public ActionResult Index(int page = 1)
    {
      var applications = this._applicationService.GetApplications();
      var manCos = this._manCoService.GetManCos();

      var model = new SelectAppManCoEmailsViewModel();

      model.Page = this.TempData.ContainsKey("Page") ? Convert.ToInt32(this.TempData["Page"].ToString()) : page;
     
      model.AddApplications(applications);
      model.AddManCos(manCos);

      return this.View(model);
    }

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public ActionResult Show(OtfParameterModel parameterModel)
    {
      if (parameterModel.IsAjaxCall)
      {
        var applications = this._applicationService.GetApplications();
        var manCos = this._manCoService.GetManCos();
        var doctypes = this._docTypeService.GetDocTypes();
      
        var appManCoEmails = this._appManCoEmailService.GetPagedAppManCoEmails(
          parameterModel.AccountNumber, parameterModel.AppId, parameterModel.ManCoId, parameterModel.Page, PageSize);

        var model = new OtfItemsViewModel(applications, manCos, doctypes);
        model.AddAppManCoEmails(appManCoEmails);

        if (model.AppManCoEmails.Count < 1)
        {
          TempData["comment"] = "No records for the selected criteria";
        }
     
        return this.PartialView("_PagedOtfResults", model);
      }

      TempData["Page"] = parameterModel.Page;

      return this.RedirectToAction("Index", "Otf");
    }

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public ActionResult Edit(int appManCoEmailId)
    {
      var appManCoEmail = this._appManCoEmailService.GetAppManCoEmail(appManCoEmailId);
      var applications = this._applicationService.GetApplications();
      var manCos = this._manCoService.GetManCos();
      var doctypes = this._docTypeService.GetDocTypes();
      var model = new EditAppManCoEmailViewModel(applications, manCos, doctypes, appManCoEmail);

      return this.View(model);
    }

    [HttpPost]
    public ActionResult Edit(EditAppManCoEmailViewModel editAppManCoEmailViewModel)
    {
      if (ModelState.IsValid)
      {
        var loggedInUser = _userService.GetApplicationUser();
        this._appManCoEmailService.UpdateAppManCoEmail(editAppManCoEmailViewModel.Id, editAppManCoEmailViewModel.ApplicationId, editAppManCoEmailViewModel.ManCoId, editAppManCoEmailViewModel.DocTypeId, editAppManCoEmailViewModel.OtfAccountNumber, editAppManCoEmailViewModel.OtfEmail, loggedInUser.UserName);
        return this.Json(new { Success = true });
      }
      else
      {
        TempData["comment"] = "Required fields are missing";
        return new RedirectResult(Request.Headers["Referer"]);
      }
    }

    [HttpPost]
    public ActionResult Create(AddAppManCoEmailViewModel model)
    {
      if (ModelState.IsValid)
      {
        this._appManCoEmailService.CreateAppManCoEmail(model.ApplicationId, model.ManCoId, model.DocTypeId, model.AccountNumber, model.Email);
        return RedirectToAction("Index");
      }
      else
      {
        TempData["comment"] = "Required files are missing";
        return new RedirectResult(Request.Headers["Referer"]);   
      }
    }

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public ActionResult Delete(int appManCoEmailId)
    {
      this._appManCoEmailService.DeleteAppManCoEmail(appManCoEmailId);
      return this.Json(new { Success = true });
    }
  }
}
