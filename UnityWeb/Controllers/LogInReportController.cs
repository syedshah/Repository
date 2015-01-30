namespace UnityWeb.Controllers
{
  using System;
  using System.Linq;
  using System.Text;
  using System.Web.Mvc;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.LogInReport;

  [AuthorizeLoggedInUser]
  public class LogInReportController : BaseController
  {
    private readonly ISessionService _sessionService;
    private readonly IUserService _userService;

    public LogInReportController(ISessionService sessionService, IUserService userService, ILogger logger)//IUserReportService userReportService, IUserService userService, ILogger logger)
      : base(logger)
    {
      _sessionService = sessionService;
      _userService = userService;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Run()
    {
      var domiciles = this._userService.GetApplicationUser().Domiciles;

      var sessions = _sessionService.GetSessionsByGovReadOnlyAdmin(DateTime.Now.Date.AddDays(-30), domiciles.First().DomicileId);

      var model = new LoginReportViewModel();
      model.CreateReport(sessions);

      var builder = GetLogInReport(model);

      return File(new UTF8Encoding().GetBytes(builder.ToString()), "text/csv", string.Format("LogInReport{0}.csv", DateTime.Now.ToShortTimeString()));
    }

    private StringBuilder GetLogInReport(LoginReportViewModel loginReportViewModel)
    {
      var csvBuilder = new StringBuilder();

      csvBuilder.Append("USER NAME");
      csvBuilder.Append(",");

      csvBuilder.Append("DATE");
      csvBuilder.Append(",");

      csvBuilder.Append("LOGGED IN");
      csvBuilder.Append(",");

      csvBuilder.Append("LOGGED OUT");
      csvBuilder.Append(",");

      csvBuilder.Append("\n");

      string userName = string.Empty; 
      foreach (var userLogIn in loginReportViewModel.LogInReport)
      {
        if (userLogIn.Key == userName)
        {
          csvBuilder.Append("");
          csvBuilder.Append(",");
        }
        else
        {
          csvBuilder.Append(userLogIn.Key);
          csvBuilder.Append(",");  
        }

        string date = string.Empty;
        foreach (var session in userLogIn.Value)
        {
          if (date != session.Date)
          {
            if (date != string.Empty)
            {
              csvBuilder.Append("");
              csvBuilder.Append(",");  
            }

            csvBuilder.Append(session.Date);
            csvBuilder.Append(",");
          }
          else
          {
            csvBuilder.Append("");
            csvBuilder.Append(",");

            csvBuilder.Append("");
            csvBuilder.Append(",");
          }

          csvBuilder.Append(session.LogIn);
          csvBuilder.Append(",");

          csvBuilder.Append(session.LogOut);
          csvBuilder.Append(",");
          csvBuilder.Append("\n");

          date = session.Date;
          userName = userLogIn.Key;
        }
      }

      return csvBuilder;
    }
  }
}
