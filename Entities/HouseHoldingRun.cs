namespace Entities
{
  using System;
  using System.Collections.Generic;
  using Microsoft.Build.Framework;

  public class HouseHoldingRun
  {
    public HouseHoldingRun()
    {
      Documents = new List<Document>();
      GridRuns = new List<GridRun>();
    }

    public HouseHoldingRun(DateTime started, DateTime ended, string grid)
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

    public virtual ICollection<Document> Documents { get; set; }

    public virtual ICollection<GridRun> GridRuns { get; set; }
  }
}
