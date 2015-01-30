namespace UnityWeb.Controllers
{
  using System.Web.Mvc;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Constants;
  using UnityWeb.Filters;
  using UnityWeb.Models.Applicaiton;

  [AuthorizeLoggedInUser(Roles = Roles.Admin + "," + Roles.DstAdmin)]
  public class ApplicationController : BaseController
  {
    private readonly IApplicationService _applicationService;

    public ApplicationController(IApplicationService applicationService, ILogger logger) : base(logger)
    {
      _applicationService = applicationService;
    }

    public ActionResult Index()
    {
      var applicationsViewModel = new ApplicationsViewModel();
      var applications = _applicationService.GetApplications();
      applicationsViewModel.AddApplications(applications);
      return View(applicationsViewModel);
    }
  }
}
