namespace UnityWeb.Controllers
{
  using System.Web.Mvc;
  using AbstractConfigurationManager;
  using Logging;
  
  public class EnvironmentController : BaseController
  {
    private readonly IConfigurationManager _configurationManager;

    public EnvironmentController(IConfigurationManager configurationManager, ILogger logger)
      : base(logger)
    {
      _configurationManager = configurationManager;
    }

    [ChildActionOnly]
    [OutputCache(Duration = 180)]
    public ActionResult Index()
    {
      var environment = _configurationManager.AppSetting("environment");

      switch (environment.ToLower())
      {
        case "debug":
          return PartialView("_debug");
        case "test":
          return PartialView("_test");
        case "uat":
          return PartialView("_uat");
        case "prod":
          return PartialView("_prod");
      }

      return PartialView("_prod");
    }
  }
}
