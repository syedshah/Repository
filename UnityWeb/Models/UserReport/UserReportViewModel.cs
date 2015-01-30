namespace UnityWeb.Models.UserReport
{
  using System;
  using System.Linq;

  public class UserReportViewModel
  {
    public UserReportViewModel(Entities.ApplicationUser user)
    {
      Id = user.Id;
      UserName = user.UserName;
      Roles = string.Join("/", (from r in user.Roles select r.Role.Name));
      IsLockedOut = user.IsLockedOut;
      IsApproved = user.IsApproved;
      LastLogin = user.LastLoginDate;
    }

    public string Id { get; set; }

    public string UserName { get; set; }

    public string Roles { get; set; }

    public bool IsLockedOut { get; set; }

    public bool IsApproved { get; set; }

    public DateTime? LastLogin { get; set; }
  }
}