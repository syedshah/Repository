namespace Entities
{
  using System;
  using System.Data.Linq.Mapping;
  using Microsoft.Build.Framework;

  public class FileSync
  {
    public FileSync()
    {
      SyncDate = DateTime.Now;
    }

    public FileSync(int gridRunId) : this()
    {
      GridRunId = gridRunId;
    }

    public int Id { get; set; }

    [Required, Column(Name = "synchronisation_date")]
    public DateTime SyncDate { get; set; }

    [Required]
    public int GridRunId { get; set; }
  }
}
