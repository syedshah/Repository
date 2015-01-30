namespace UnityWeb.Models.GridRun
{
  using System.Collections.Generic;

  public class GridRunSearchesViewModel
  {
    private List<GridRunSearchViewModel> _gridRuns = new List<GridRunSearchViewModel>();

    public List<GridRunSearchViewModel> GridRuns
    {
      get { return _gridRuns; }
      set { _gridRuns = value; }
    }

    public void AddGridRuns(IList<Entities.GridRun> gridRuns)
    {
      foreach (Entities.GridRun gridRun in gridRuns)
      {
        var gvm = new GridRunSearchViewModel(gridRun);
        GridRuns.Add(gvm);
      }
    }
  }
}