namespace UnityWeb.Models.GridRun
{
  using System;

  public class GridRunSearchViewModel : BaseGridRunViewModel
  {
    public GridRunSearchViewModel(Entities.GridRun gridRun, bool houseHolding = false)
      : base(houseHolding)
    {
      GridRunId = gridRun.Id.ToString();
      Grid = gridRun.Grid;
      Code = gridRun.Application.Code;
      Desc = gridRun.Application.Description;
      StartDate = gridRun.StartDate.GetValueOrDefault();
      Duration = gridRun.EndDate != null ? gridRun.EndDate.Value - gridRun.StartDate.GetValueOrDefault() : new TimeSpan();
      FileName = gridRun.XmlFile.FileName;
    }

    public string GridRunId { get; set; }

    public string Code { get; set; }

    public string Desc { get; set; }

    public TimeSpan Duration { get; set; }

    public int Status { get; set; }

    public string FileName { get; set; }
  }
}