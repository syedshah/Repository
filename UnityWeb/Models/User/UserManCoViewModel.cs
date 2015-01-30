namespace UnityWeb.Models.User
{
  using System.Collections.Generic;
  using System.Web.Mvc;
  using UnityWeb.Models.Shared;

  public class UserManCoViewModel
  {
    public UserManCoViewModel()
    {
      AvailableItems = new List<CheckBoxModel>();

      SelectedItems = new List<CheckBoxModel>();
    }

    public string SelectedManCos { get; set; }

    public IList<CheckBoxModel> SelectedItems { get; set; }

    public IList<CheckBoxModel> AvailableItems { get; set; }

    public PostedCheckBox PostedCheckBox { get; set; }

    public void AddManCos(IList<Entities.ManCo> manCos)
    {
      foreach (var manco in manCos)
      {
        AvailableItems.Add(new CheckBoxModel { Value = manco.Id.ToString(), Text = manco.Code + '-' + manco.Description});
      }
    }
  }
}