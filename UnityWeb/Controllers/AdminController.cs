namespace UnityWeb.Controllers
{
  using System.Web.Mvc;
  using Logging;
  using UnityWeb.Constants;
  using UnityWeb.Filters;

  [AuthorizeLoggedInUser(Roles = Roles.Admin + "," + Roles.DstAdmin)]
  public class AdminController : BaseController
  {

    public AdminController(ILogger logger)
      : base(logger)
    {
    }

    public ActionResult Index()
    {
      return View();
    }
  }
}
