namespace UnityWeb.Models.GridRun
{
  public class GridRunDetailViewModel : BaseGridRunViewModel
  {
    public GridRunDetailViewModel(Entities.GridRun gridRun, bool houseHolding = false)
      : base(houseHolding)
    {
      FileName = gridRun.XmlFile.FileName;
      Grid = gridRun.Grid;
      BigZip = gridRun.XmlFile.BigZip;
      DocType = gridRun.XmlFile.DocType.Code;
      ManCo = gridRun.XmlFile.ManCo.Code;
      StartDate = gridRun.StartDate.GetValueOrDefault();
    }

    public string FileName { get; set; }

    public string DocType { get; set; }

    public string ManCo { get; set; }
  }
}