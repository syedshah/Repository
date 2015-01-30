namespace UnityWeb.Filters
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using IdentityWrapper.Interfaces;
  using ServiceInterfaces;

  public class AuthorizeLoggedInUserAttribute : AuthorizeAttribute
  {
    public IUserService UserService { get; set; }
    public IUserManagerProvider UserManagerProvider { get; set; }

    public AuthorizeLoggedInUserAttribute()
    {
          
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
      filterContext.Result = new RedirectResult("~/session/new");
       
    }

    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        base.OnAuthorization(filterContext);

          if (!AuthorizeCore(filterContext.HttpContext))
          {
              HandleUnauthorizedRequest(filterContext);
              return;
          }

        var listRoles = SplitString(Roles);

        var user = filterContext.HttpContext.User;

       if (!this.CheckInRoles(listRoles, user.Identity.Name))
       {
           filterContext.Result = new RedirectResult("~/Dashboard/index");
       }

    }

    private IList<string> SplitString(string original)
    {
        if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

        return Roles.Split(',').ToList();
    }

    private bool CheckInRoles(IList<string> roles, string userName)
    {
        var user = this.UserManagerProvider.FindByName(userName);

        IList<string> userRoles = new List<string>();

        if (user.Roles != null || user.Roles.Count() >= 1)
        {
            user.Roles.ToList().ForEach(x => userRoles.Add(x.Role.Name)); 
        }

        if (roles.Count < 1)
        {
            return true;
        }
        else if (userRoles.Any(c => roles.Contains(c)))
        {
            return true;
        }
        else
        {
            return false;
        }

      return true;
    }
  }
}