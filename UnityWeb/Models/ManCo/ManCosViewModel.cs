namespace UnityWeb.Models.ManCo
{
  using System.Collections.Generic;

  public class ManCosViewModel
  {
    private List<ManCoViewModel> _namCos = new List<ManCoViewModel>();

    public List<ManCoViewModel> ManCos
    {
      get { return _namCos; }
      set { _namCos = value; }
    }

    public void AddMancos(IList<Entities.ManCo> manCos)
    {
      foreach (Entities.ManCo manCo in manCos)
      {
        var mvm = new ManCoViewModel(manCo);
        mvm.AddDisplayName(manCos);
        ManCos.Add(mvm);
      }
    }
  }
}