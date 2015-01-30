namespace UnityWeb.Models.AutoApproval
{
  using System.Collections.Generic;

  public class ManCosApprovalViewModel
  {
    private List<ManCoApprovalViewModel> _namCos = new List<ManCoApprovalViewModel>();

    public int SelectedManCoId { get; set; }

    public List<ManCoApprovalViewModel> ManCos
    {
      get { return _namCos; }
      set { _namCos = value; }
    }

    public void AddMancos(IList<Entities.ManCo> manCos)
    {
      foreach (Entities.ManCo manCo in manCos)
      {
        var mvm = new ManCoApprovalViewModel(manCo);
        ManCos.Add(mvm);
      }
    }
  }

}