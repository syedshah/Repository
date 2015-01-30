namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.Web.Mvc;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.Account;

  //[AuthorizeLoggedInUser]
  public class AccountController : BaseController
  {
    private readonly IUserService _userService;
    private readonly IFormsAuthenticationService _formsAuthenticationService;

    public AccountController(IUserService userService, IFormsAuthenticationService formsAuthenticationService, ILogger logger)
      : base(logger)
    {
      _userService = userService;
      _formsAuthenticationService = formsAuthenticationService;
    }

    [ChildActionOnly]
    [AuthorizeLoggedInUser]
    public ActionResult Summary()
    {
      var user = this._userService.GetApplicationUser();
       
      var modelOut = new UserSummaryViewModel(user, this.HttpContext);

      return PartialView("_Summary", modelOut);
    }

    [HttpGet]
    public ActionResult ChangePassword(string userName)
    {
      if (string.IsNullOrEmpty(userName))
      {
        return RedirectToAction("New", "Session");
      }
      else
      {
         TempData["username"] = userName;
        return RedirectToAction("ChangeCurrent", "Password", userName);
      }
     
     }

   /* [HttpPost]
    public ActionResult UpdatePassword(ChangePasswordModel model)
    {
      List<ErrorDetails> errorDetails;
      if (!ModelState.IsValid)
      {
        return View("ChangePassword");
      }
      
      errorDetails = _userService.ChangePassword(model.UserName, model.OldPassword, model.NewPassword);
      if (errorDetails.Count != 0)
      {
        foreach (ErrorDetails errorDetail in errorDetails)
        {
          ModelState.AddModelError(string.Empty, errorDetail.Message);
        }
        return View("ChangePassword");
      }
      _formsAuthenticationService.SignIn(model.UserName, true);
      return RedirectToAction("Index", "Dashboard");
    }*/
  }
}