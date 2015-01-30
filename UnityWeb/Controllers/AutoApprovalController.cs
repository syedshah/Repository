namespace UnityWeb.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Entities;
  using Exceptions;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Constants;
  using UnityWeb.Filters;
  using UnityWeb.Models.AutoApproval;
  using UnityWeb.Models.DocType;

  [AuthorizeLoggedInUser(Roles = Roles.Admin + "," + Roles.DstAdmin)]
  public class AutoApprovalController : BaseController
  {
    private readonly IAutoApprovalService _autoApprovalService;
    private readonly IDocTypeService _docTypeService;
    private readonly ISubDocTypeService _subDocTypeService;
    private readonly IManCoService _manCoService;
    private readonly IUserService _userService;

    public AutoApprovalController(
      IAutoApprovalService autoApprovalService,
      IDocTypeService docTypeService,
      ISubDocTypeService subDocTypeService,
      IManCoService manCoService,
      IUserService userService,
      ILogger logger)
      : base(logger)
    {
      this._autoApprovalService = autoApprovalService;
      this._docTypeService = docTypeService;
      this._subDocTypeService = subDocTypeService;
      this._manCoService = manCoService;
      this._userService = userService;
    }

    public ActionResult Index()
    {
      var manCosApprovalViewModel = new ManCosApprovalViewModel();

      var currentUser = this._userService.GetApplicationUser();
      var manCos = this._manCoService.GetManCosByUserId(currentUser.Id);
      manCosApprovalViewModel.AddMancos(manCos);

        if (TempData["SelectedManCoId"] != null)
        {
          manCosApprovalViewModel.SelectedManCoId = Convert.ToInt32(TempData["SelectedManCoId"].ToString());    
        }
  
      return this.View("AutoApprovals", manCosApprovalViewModel);
    }

    public ActionResult AutoApprovals(int manCoId)
    {
      var autoApprovals = this._autoApprovalService.GetAutoApprovals(manCoId);

      var docTypes = _docTypeService.GetDocTypes();

      var subDocTypes = _subDocTypeService.GetSubDocTypes();

      var manCos = _manCoService.GetManCos();
      var autoApprovalsViewModel = new AutoApprovalsViewModel(docTypes, subDocTypes, manCos);

      autoApprovalsViewModel.AddAutoApprovals(autoApprovals, docTypes, subDocTypes);

      return this.PartialView("_ShowAutoApprovals", autoApprovalsViewModel);
    }

    [HttpPost]
    public virtual ActionResult Create(AddAutoApprovalViewModel addAutoApprovalViewModel)
    {
      if (ModelState.IsValid)
      {
        if (addAutoApprovalViewModel.SubDocTypeId == -1)
        {
          this.AutoApproveAllSubTypes(addAutoApprovalViewModel.DocTypeId, addAutoApprovalViewModel.ManCoId);
        }
        else
        {
          this.AutoApproveSingleDocType(addAutoApprovalViewModel.ManCoId, addAutoApprovalViewModel.DocTypeId, addAutoApprovalViewModel.SubDocTypeId);
        }
      }
      else
      {
        TempData["comment"] = "Required files are missing";
      }
      TempData["SelectedManCoId"] = addAutoApprovalViewModel.ManCoId;
      return new RedirectResult(Request.Headers["Referer"]);
    }

    public virtual ActionResult Delete(int autoApprovalId, string docTypeCode, int mancoId)
    {
      if (autoApprovalId > 0)
      {
        _autoApprovalService.Delete(autoApprovalId);
      }
      else
      {
        _autoApprovalService.Delete(docTypeCode);
      }

      TempData["SelectedManCoId"] = mancoId;
      return RedirectToAction("Index", "AutoApproval");
    }

    public virtual ActionResult Edit(int autoApprovalId, string docTypeCode)
    {
      AutoApproval autoApproval = new AutoApproval();
      IList<AutoApproval> autoApprovals = new List<AutoApproval>();

      if (autoApprovalId > 0)
      {
        autoApproval = this._autoApprovalService.GetAutoApproval(autoApprovalId);
      }
      else
      {
        autoApprovals = this._autoApprovalService.GetAutoApprovals(docTypeCode);   
      }

      var docTypes = _docTypeService.GetDocTypes();
      var subDocTypes = this._subDocTypeService.GetSubDocTypes();
      var manCos = _manCoService.GetManCos();

      var model = new EditAutoApprovalViewModel();

      if (autoApprovalId > 0)
      {
        model.ManCoId = autoApproval.ManCoId;
        model.DocTypeId = autoApproval.DocTypeId;
        model.SubDocTypeId = autoApproval.SubDocTypeId;
      }
      else
      {
        model.ManCoId = autoApprovals.First().ManCoId;
        model.DocTypeId = autoApprovals.First().DocTypeId;
        model.SubDocTypeId = -1;
      }

      model.DocTypeCode = docTypeCode;
      model.AddManCos(manCos);
      model.AddDocTypes(docTypes);
      model.AddSubDocTypes(subDocTypes);

      model.SubDocTypes = model.SubDocTypes.FindAll(x => x.DocTypeViewModel.Id == model.DocTypeId);

      model.SubDocTypes.Insert(0, new SubDocTypeViewModel()
                                    {
                                      Code = "All",
                                      Id = -1
                                    });

      return this.View(model);
    }

    [HttpPost]
    public virtual ActionResult Update(EditAutoApprovalViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          if (model.AutoApprovalId == -1)
          {
            _autoApprovalService.Delete(model.DocTypeCode);
            this.AutoApproveSingleDocType(model.ManCoId, model.DocTypeId, model.SubDocTypeId);
            TempData["SelectedManCoId"] = model.ManCoId;
            return RedirectToAction("Index", "AutoApproval");
          }
          else
          {
            if (model.SubDocTypeId == -1)
            {
              this.AutoApproveAllSubTypes(model.DocTypeId, model.ManCoId);
            }
            else
            {
              this._autoApprovalService.Update(model.AutoApprovalId, model.ManCoId, model.DocTypeId, model.SubDocTypeId);
            }
            
            TempData["SelectedManCoId"] = model.ManCoId;
            return RedirectToAction("Index", "AutoApproval");
          }
        }
        catch (UnityAutoApprovalAlreadyExistsException)
        {
          this.TempData["comment"] = "Auto approval already exists";
          return RedirectToAction("Edit", "AutoApproval", new { autoApprovalId = model.AutoApprovalId, docTypeCode = model.DocTypeCode } ); 
        }
      }
      else
      {
        TempData["comment"] = "Please correct the errors and try again";
        return RedirectToAction("Edit", "AutoApproval", new { autoApprovalId = model.AutoApprovalId, docTypeCode = model.DocTypeCode }); 
      }
    }

    private void AutoApproveSingleDocType(int mancoId, int docTypeId, int subDocType)
    {
      try
      {
        this._autoApprovalService.AddAutoApproval(mancoId, docTypeId, subDocType);
      }
      catch (UnityAutoApprovalAlreadyExistsException)
      {
        this.TempData["comment"] = "Auto approval already exists";
      }
    }

    private void AutoApproveAllSubTypes(int docTypeId, int mancoId)
    {
      var subDocTypes = this._subDocTypeService.GetSubDocTypes(docTypeId);

      foreach (var subDocType in subDocTypes)
      {
        try
        {
          this._autoApprovalService.AddAutoApproval(mancoId, docTypeId, subDocType.Id);
        }
        catch (UnityException)
        {
        }
      }
    }
  }
}
