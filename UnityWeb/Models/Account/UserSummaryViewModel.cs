namespace UnityWeb.Models.Account
{
  using System.Web;
  using Entities;

  public class UserSummaryViewModel
  {
    public UserSummaryViewModel()
    {
    }

    public UserSummaryViewModel(ApplicationUser user, HttpContextBase context)
    {
      FirstName = user.FirstName;

      if (context.Session["LastLoggedInDate"] == null)
      {
        LastLoggedInDate = user.LastLoginDate.ToString();
      }
      else
      {
        LastLoggedInDate = context.Session["LastLoggedInDate"].ToString();
      }
    }

    public string FirstName { get; set; }

    public string LastLoggedInDate { get; set; }
  }
}