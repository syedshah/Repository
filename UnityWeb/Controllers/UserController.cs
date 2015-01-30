namespace UnityWeb.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Logging;
  using Microsoft.AspNet.Identity.EntityFramework;
  using ServiceInterfaces;
  using UnityWeb.Constants;
  using UnityWeb.Filters;
  using UnityWeb.Models.Shared;
  using UnityWeb.Models.User;

  [AuthorizeLoggedInUser(Roles = Roles.Admin + "," + Roles.DstAdmin)]
  public class UserController : BaseController
  {
    private readonly IUserService _userService;

    private readonly IManCoService _manCoService;

    private readonly IIdentityRoleService _identityRoleService;

    public int PageSize = 10;

    public UserController(
      IUserService userService,
      IManCoService manCoService,
      IIdentityRoleService identityRoleService,
      ILogger logger)
      : base(logger)
    {
      this._userService = userService;
      this._manCoService = manCoService;
      this._identityRoleService = identityRoleService;
    }

    public ActionResult Index(int page = 1, bool isAjaxCall = false)
    {
      var domiciles = this._userService.GetApplicationUser().Domiciles;

      var listDomicileIds = new List<int>();

      domiciles.ToList().ForEach(x => listDomicileIds.Add(x.DomicileId));

      var users = this._userService.GetUsersByDomicile(listDomicileIds, page, PageSize);

      var usersViewModel = new UsersViewModel();

      usersViewModel.AddUsers(users);

      if (isAjaxCall)
      {
        return PartialView("_PagedUserResults", usersViewModel);
      }

      return View(usersViewModel);
    }

    [HttpGet]
    public virtual ActionResult Edit(string userName)
    {
      var loggedInUser = _userService.GetApplicationUser();

      var user = this._userService.GetApplicationUser(userName);
      var domicileId = Convert.ToInt32(this._userService.GetApplicationUser().Domiciles.FirstOrDefault().DomicileId);
      var manCos = this._manCoService.GetManCos(domicileId);

      var userRoles = this._userService.GetRoles(loggedInUser.Id).ToList();

      var editModel = new EditUserViewModel(user, loggedInUser.UserName);
      editModel.DomicileId = domicileId;

      editModel.UserManCoViewModel.SelectedItems = this.GenerateManCoCheckedList(user.ManCos);
      editModel.UserManCoViewModel.AddManCos(manCos.ToList());
      editModel.AddDomiciles(this._userService.GetUserDomiciles().ToList());

      var identityRoles = this._identityRoleService.GetRoles();

      //editModel.AddIdentityRoles(identityRoles);
      editModel.AddIdentityRoles(this._identityRoleService.GetRoles(), userRoles);

      editModel.SelectedRoleItems = this.GenerateRolesCheckedList(
        identityRoles, this._userService.GetRoles(user.Id).ToList());

      return View(editModel);
    }

    [HttpPost]
    public virtual ActionResult Edit(string submitButton, EditUserViewModel editModel)
    {
      if (!this.ModelState.IsValid)
      {
        var errorList = this.ModelState.Values.SelectMany(x => x.Errors).ToList();

        if (errorList.Count > 0)
        {
          this.TempData["comment"] = errorList[0].ErrorMessage;
        }
        else
        {
          this.TempData["comment"] = "Required fields are empty";
        }

        var user = this._userService.GetApplicationUser(editModel.UserName);

        editModel.AddDomiciles(this._userService.GetUserDomiciles().ToList());

        var identityRoles = this._identityRoleService.GetRoles();

        editModel.AddIdentityRoles(identityRoles);

        var manCos = this._manCoService.GetManCos(editModel.DomicileId).ToList();

        editModel.UserManCoViewModel.SelectedItems = this.GenerateManCoCheckedList(user.ManCos);

        editModel.UserManCoViewModel.AddManCos(manCos);

        if (editModel.UserManCoViewModel.PostedCheckBox != null)
        {
          editModel.UserManCoViewModel.SelectedItems = this.GenerateManCoCheckedList(
            manCos, editModel.UserManCoViewModel.PostedCheckBox.CheckBoxValues.ToList());
        }

        editModel.SelectedRoleItems = this.GenerateRolesCheckedListByRoleIds(
          identityRoles, editModel.PostedRolesCheckBox.CheckBoxValues);

        return this.View(editModel);
      }

      switch (submitButton)
      {
        case "Save":
          return this.SubmitUserChanges(editModel);
        case "Unlock User":
          return this.UnlockUser(editModel.UserId);
        default:
          return (View());
      }
    }
    [HttpGet]
    public virtual ActionResult Create()
    {
      var addModel = new AddUserViewModel();

      addModel.AddDomiciles(this._userService.GetUserDomiciles().ToList());

      var user = _userService.GetApplicationUser();

      var userRoles = this._userService.GetRoles(user.Id).ToList();

      addModel.AddIdentityRoles(this._identityRoleService.GetRoles(), userRoles);

      var domicileId = Convert.ToInt32(this._userService.GetApplicationUser().Domiciles.FirstOrDefault().DomicileId);
      var manCos = this._manCoService.GetManCos(domicileId);
      var userManCoModel = new UserManCoViewModel();

      addModel.UserManCoViewModel.SelectedItems = new List<CheckBoxModel>();

      userManCoModel.AddManCos(manCos.ToList());

      addModel.UserManCoViewModel = userManCoModel;

      return View(addModel);
    }

    [HttpPost]
    public virtual ActionResult Create(AddUserViewModel user)
    {
      if (ModelState.IsValid)
      {
        var selected = new List<int>();

        if (user.PostedCheckBox != null)
        {
          user.PostedCheckBox.CheckBoxValues.ToList().ForEach(x => selected.Add(Convert.ToInt32(x)));
        }

        var selectedRoles = this.GetCheckedRoles(user.PostedRolesCheckBox);

        this._userService.CreateUser(
          user.UserName,
          user.Password,
          user.FirstName,
          user.LastName,
          user.Email,
          selected,
          user.DomicileId,
          selectedRoles);
        return RedirectToAction("Index");
      }
      else
      {
        var errorList = this.ModelState.Values.SelectMany(x => x.Errors).ToList();

        if (errorList.Count > 0)
        {
          TempData["comment"] = errorList[0].ErrorMessage; 
        }
        else
        {
          TempData["comment"] = "Required fields are empty";  
        }

        user.AddDomiciles(this._userService.GetUserDomiciles().ToList());
        
        var domicileId = Convert.ToInt32(this._userService.GetApplicationUser().Domiciles.FirstOrDefault().DomicileId);
        var manCos = this._manCoService.GetManCos(domicileId);
        var userManCoModel = new UserManCoViewModel();

        userManCoModel.AddManCos(manCos.ToList());

        var identityRoles = this._identityRoleService.GetRoles();

        user.AddIdentityRoles(identityRoles);

        if (user.PostedRolesCheckBox != null)
        {
          user.SelectedRoleItems = GenerateRolesCheckedListByRoleIds(
            identityRoles, user.PostedRolesCheckBox.CheckBoxValues);
        }

        user.UserManCoViewModel = userManCoModel;

        if (user.PostedCheckBox != null)
        {
          user.UserManCoViewModel.SelectedItems = GenerateManCoCheckedList(
            manCos.ToList(), user.PostedCheckBox.CheckBoxValues);
        }

        return this.View(user);
      }
    }

    [HttpGet]
    public ActionResult RetrieveManCoes(string domicileId, string userName)
    {
      try
      {
        var user = this._userService.GetApplicationUser(userName);

        var userManCoViewModel = new UserManCoViewModel();

        var manCos = this._manCoService.GetManCos(Convert.ToInt32(domicileId));

        if (!string.IsNullOrEmpty(userName))
        {
          userManCoViewModel.SelectedItems = this.GenerateManCoCheckedList(user.ManCos);  
        }

        userManCoViewModel.AddManCos(manCos.ToList());

        return this.PartialView("_UserManCo", userManCoViewModel);
      }
      catch (Exception e)
      {
        return Json(new { status = "Error", message = e.Message });
      }
    }

    private IList<CheckBoxModel> GenerateManCoCheckedList(IList<Entities.ApplicationUserManCo> selectedMancos)
    {
      IList<CheckBoxModel> checkedList = new List<CheckBoxModel>();

      selectedMancos.ToList().ForEach(x => checkedList.Add(new CheckBoxModel(x.ManCo)
                                                             {
                                                               Id = x.ManCoId
                                                             }));

      return checkedList;
    }

    private IList<CheckBoxModel> GenerateManCoCheckedList(IList<Entities.ManCo> mancos, IList<string> selected)
    {
      IList<CheckBoxModel> checkedList = new List<CheckBoxModel>();

      foreach (var manco in mancos)
      {
        if (selected.Contains(manco.Id.ToString()))
        {
          checkedList.Add(
            new CheckBoxModel
              {
                IsSelected = true,
                Text = manco.Code + '-' + manco.Description,
                Value = manco.Id.ToString()
              });
        }
      }

      return checkedList;
    }

    private IList<string> GetCheckedRoles(PostedCheckBox postedCheckBox)
    {
      var checkedItems = new List<string>();

      if (postedCheckBox != null)
      {
        checkedItems = postedCheckBox.CheckBoxValues.ToList();
      }

      return checkedItems;
    }

    private IList<CheckBoxModel> GenerateRolesCheckedListByRoleIds(IEnumerable<IdentityRole> listRoles, IList<string> checkedRoleIds)
    {
      return (from identityRole in listRoles where checkedRoleIds.Contains(identityRole.Id) select new CheckBoxModel { IsSelected = true, Text = identityRole.Name, Value = identityRole.Id }).ToList();
    }

    private IList<CheckBoxModel> GenerateRolesCheckedList(IEnumerable<IdentityRole> listRoles, ICollection<string> userRoles)
    {
      return (from identityRole in listRoles where userRoles.Contains(identityRole.Name) select new CheckBoxModel { IsSelected = true, Text = identityRole.Name, Value = identityRole.Id }).ToList();
    }

    public ActionResult Delete(string userName)
    {
      this._userService.DeactivateUser(userName);

      return RedirectToAction("Index", "User");
    }

    private ActionResult UnlockUser(string userId)
    {
      this._userService.UnlockUser(userId);

      return RedirectToAction("Index", "User");
    }

    private ActionResult SubmitUserChanges(EditUserViewModel editModel)
    {
      var selected = new List<int>();

      if (editModel.PostedCheckBox != null)
      {
        editModel.PostedCheckBox.CheckBoxValues.ToList().ForEach(x => selected.Add(Convert.ToInt32(x)));
      }

      var selectedRoles = this.GetCheckedRoles(editModel.PostedRolesCheckBox);

      this._userService.Updateuser(
        editModel.UserName,
        editModel.Password,
        editModel.FirstName,
        editModel.LastName,
        editModel.Email,
        selected,
        selectedRoles,
        editModel.IsApproved);
      return RedirectToAction("Index");
    }
  }
}
