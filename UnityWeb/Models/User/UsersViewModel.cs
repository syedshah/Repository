namespace UnityWeb.Models.User
{
  using System.Collections.Generic;
  using Entities;
  using UnityWeb.Models.Helper;

  public class UsersViewModel
  {
    public UsersViewModel()
    {
      this.AddUserViewModel = new AddUserViewModel();

      PagingInfo = new PagingInfoViewModel();
    }
    
    public UsersViewModel(IList<Entities.ApplicationUser> users)
    {
      foreach (var user in users)
      {
          var x = new ApplicationUserViewModel(user);
          this._users.Add(x);
      }

      this.AddUserViewModel = new AddUserViewModel();

      PagingInfo = new PagingInfoViewModel();
    }

    public AddUserViewModel AddUserViewModel { get; set; }

    public PagingInfoViewModel PagingInfo { get; set; }

    public string CurrentPage { get; set; }

    private List<ApplicationUserViewModel> _users = new List<ApplicationUserViewModel>();

    public List<ApplicationUserViewModel> Users
    {
        get { return _users; }
        set { _users = value; }
    }

    public void AddUsers(PagedResult<ApplicationUser> users)
    {
      foreach (ApplicationUser user in users.Results)
      {
        var avm = new ApplicationUserViewModel(user);
        Users.Add(avm);
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

      CurrentPage = PagingInfo.CurrentPage.ToString();
    }
  }
}