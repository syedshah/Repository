namespace UnityWeb.Models.UserReport
{
  using System.Collections.Generic;

  using UnityWeb.Models.Helper;

  public class UserReportsViewModel
  {
    private List<UserReportViewModel> _users = new List<UserReportViewModel>();

    public UserReportsViewModel()
    {
      PagingInfo = new PagingInfoViewModel();
    }

    public UserReportsViewModel(IList<Entities.ApplicationUser> users) : this()
    {
      
    }

    public PagingInfoViewModel PagingInfo { get; set; }

    public List<UserReportViewModel> Users
    {
      get { return _users; }
      set { _users = value; }
    }

    public void AddUsers(Entities.PagedResult<Entities.ApplicationUser> users)
    {
      foreach (var user in users.Results)
      {
        var uvm = new UserReportViewModel(user);
        Users.Add(uvm);
      }

      PagingInfo = new PagingInfoViewModel
      {
        CurrentPage = users.CurrentPage,
        TotalItems = users.TotalItems,
        ItemsPerPage = users.ItemsPerPage,
        TotalPages = users.TotalPages,
        StartRow = users.StartRow,
        EndRow = users.EndRow
      };
    }
  }
}