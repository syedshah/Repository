namespace UnityWeb.Models.Shared
{
  using System.Collections.Generic;

  public class CheckBoxViewModel
  {
    public CheckBoxViewModel()
    {
      AvailableItems = new List<CheckBoxModel>();

      SelectedItems = new List<CheckBoxModel>();   
    }

    public IList<CheckBoxModel> SelectedItems { get; set; }

    public IList<CheckBoxModel> AvailableItems { get; set; }

    public PostedCheckBox PostedCheckBox { get; set; }
  }
}