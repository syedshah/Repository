namespace UnityWeb.Models.Dashboard
{
  using System.Collections.Generic;

  using Entities;

  using UnityWeb.Models.ManCo;

  public class ManCoFilterViewModel
  {
    public string SelectedManCoId { get; set; }
    public List<string> ManCoTexts { get; set; }

    public List<ManCoViewModel> ManCos
    {
      get
      {
        return _namCos;
      }
      set
      {
        _namCos = value;
      }
    }

    private List<ManCoViewModel> _namCos = new List<ManCoViewModel>();

    public void AddMancos(IList<Entities.ManCo> manCos)
    {
      foreach (Entities.ManCo manCo in manCos)
      {
        var mvm = new ManCoViewModel(manCo);
        mvm.AddDisplayName(manCos);
        ManCos.Add(mvm);
      }

      ManCos.Insert(0, new ManCoViewModel(new ManCo("All", "All")));
    }
  }
}