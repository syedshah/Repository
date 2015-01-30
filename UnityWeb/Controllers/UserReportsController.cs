namespace UnityWeb.Controllers
{
  using System;
  using System.Linq;
  using System.Text;
  using System.Web.Mvc;
  using Entities;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using Logging;
  using UnityWeb.Models.UserReport;

  [AuthorizeLoggedInUser]
  public class UserReportsController : BaseController
  {
    private readonly IUserReportService _userReportService;
    private readonly IUserService _userService;

    public UserReportsController(IUserReportService userReportService, IUserService userService, ILogger logger)
      : base(logger)
    {
      _userReportService = userReportService;
      _userService = userService;
    }

    public ActionResult Index(int page = 1)
    {
      var userReportsViewModel = new UserReportsViewModel();
      var users = this.GetReportData(page, 10);
      userReportsViewModel.AddUsers(users);
      return View(userReportsViewModel);
    }

    public ActionResult Run(int page = 1)
    {
      var users = this.GetReportData(page, 100);

      var builder = GetUserReport(users);

      return File(new UTF8Encoding().GetBytes(builder.ToString()), "text/csv", string.Format("UserReport_{0}.csv", DateTime.Now.ToShortTimeString()));
    }

    private PagedResult<ApplicationUser> GetReportData(int page, int pageSize)
    {
      Domicile docmile = this._userService.GetUserDomiciles().First();

      PagedResult<ApplicationUser> users = this._userReportService.GetUserReport(docmile.Id, page, pageSize);
      return users;
    }

    private StringBuilder GetUserReport(PagedResult<ApplicationUser> users)
    {
      var csvBuilder = new StringBuilder();

      csvBuilder.Append("USER NAME");
      csvBuilder.Append(",");

      csvBuilder.Append("USER ROLE");
      csvBuilder.Append(",");

      csvBuilder.Append("LOCKED");
      csvBuilder.Append(",");

      csvBuilder.Append("ACTIVE");
      csvBuilder.Append(",");

      csvBuilder.Append("LAST LOGIN DATE");
      csvBuilder.Append(",");

      csvBuilder.Append("\n");

      foreach (var applicationUser in users.Results)
      {
        var isLockedOut = applicationUser.IsLockedOut;
        var isApproved = applicationUser.IsApproved;
        var userName = applicationUser.UserName;
        var lastLogIn = applicationUser.LastLoginDate;

        foreach (var userByRole in applicationUser.Roles)
        {
          csvBuilder.Append(userName);
          csvBuilder.Append(",");

          csvBuilder.Append(userByRole.Role.Name);
          csvBuilder.Append(",");

          csvBuilder.Append(isLockedOut);
          csvBuilder.Append(",");

          csvBuilder.Append(isApproved);
          csvBuilder.Append(",");

          csvBuilder.Append(lastLogIn);
          csvBuilder.Append(",");

          csvBuilder.Append("\n");
        }
      }

      return csvBuilder;
    }
  }
}
