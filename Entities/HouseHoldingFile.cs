namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Microsoft.Build.Framework;

  public class HouseHoldingRunData
  {
    public HouseHoldingRunData()
    {
      DocumentRunData = new List<DocumentRunData>();
      ProcessingGridRun = new Collection<string>();
    }

    public HouseHoldingRunData(DateTime started, DateTime ended, string grid)
      : this()
    {
      Grid = grid;
      StartDate = started;
      EndDate = ended;
    }

    public int Id { get; set; }

    [Required]
    public string Grid { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public virtual ICollection<DocumentRunData> DocumentRunData { get; set; }

    public virtual ICollection<string> ProcessingGridRun { get; set; }
  }
}
