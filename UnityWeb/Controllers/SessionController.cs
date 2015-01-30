namespace UnityWeb.Controllers
{
  using System;
  using System.Web.Mvc;
  using Entities;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Models.User;

  public class SessionController : BaseController
  {
    private readonly IUserService _userService;
    private readonly ISecurityAnswerService _securityAnswerService;
    private readonly ISessionService _sessionService;
    private GlobalSetting _globalSetting;

    public SessionController(IUserService userService, ISecurityAnswerService securityAnswerService, ISessionService sessionService, ILogger logger)
      : base(logger)
    {
      _userService = userService;
      _sessionService = sessionService;
      this._securityAnswerService = securityAnswerService;
    }

    [HttpGet]
    public ActionResult New()
    {
      if (!HttpContext.User.Identity.IsAuthenticated)
      {
        this.HttpContext.Session["LastLoggedInDate"] = null;
        return View();
      }
      return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    public ActionResult Create(LoginUserViewModel userViewModel, String returnUrl)
    {
      if (ModelState.IsValid)
      {
        var user = this._userService.GetApplicationUser(userViewModel.Username.Trim(), userViewModel.Password.Trim());
        if (user != null)
        {
          if (user.IsLockedOut)
          {
            return RedirectToAction("LockedOut", "Session", new { userName = userViewModel.Username });
          }

          if (!user.IsApproved)
          {
            return RedirectToAction("InActive", "Session", new { userName = userViewModel.Username });
          }

          AddLastLoggedInToSession(user);

          this._userService.UpdateUserLastLogindate(user.Id, (String)this.HttpContext.Session["SessionGuid"]);

          if (_userService.CheckForPassRenewal(user.LastPasswordChangedDate, user.LastLoginDate.GetValueOrDefault()))
          {
            return RedirectToAction("ChangePassword", "Account", new { userName = userViewModel.Username });
          }

          this._userService.SignIn(user, false);

          if (!this._securityAnswerService.HasSecurityAnswers(user.Id))
          {
            return RedirectToAction("AddAnswers", "Security");
          }

          return RedirectToLocal(returnUrl);
        }
        else
        {
          user = _userService.GetApplicationUser(userViewModel.Username);

          if (user != null)
          {
            _userService.UpdateUserFailedLogin(user.Id);

            if (_userService.IsLockedOut(user.Id))
            {
              return RedirectToAction("LockedOut", "Session", new { userName = userViewModel.Username });
            }  
          }

          Logger.Info(string.Format("User: {0} failed to log on", userViewModel.Username));
          TempData["message"] = "Login was unsuccessful. Please correct the errors and try again.";

          return View("New");
        }
      }

      return View("New");
    }

    public ActionResult Expired()
    {
      this.UpdateSession();
      this.SignOut();
      return View();
    }

    public void SessionReset()
    {
    }

    private ActionResult RedirectToLocal(String returnUrl)
    {
      if (!String.IsNullOrEmpty(returnUrl))
      {
        return Redirect(returnUrl);
      }
      else
      {
        return RedirectToAction("Index", "Dashboard");
      }
    }

    public ActionResult Remove()
    {
      this.UpdateSession();
      this._userService.SignOut();
      Session.Clear();
      return View();
    }

    private void UpdateSession()
    {
      var guid = (string)this.HttpContext.Session["SessionGuid"];
      if (guid != null)
      {
        this._sessionService.Update(guid, DateTime.Now); 
      }
    }

    public ActionResult LockedOut()
    {
      return View();
    }

    public ActionResult InActive()
    {
      return View();
    }

    private void AddLastLoggedInToSession(ApplicationUser user)
    {
      this.HttpContext.Session["LastLoggedInDate"] = user.LastLoginDate;
      this.HttpContext.Session["SessionGuid"] = Guid.NewGuid().ToString();
    }

    private void SignOut()
    {
      this._userService.SignOut();
      this.Session.Clear();
    }
  }
}
