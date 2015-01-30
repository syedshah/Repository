namespace Entities.File
{
  using System;
  using System.Data.Linq.Mapping;
  using Microsoft.Build.Framework;

  public abstract class InputFile
  {
    public int Id { get; set; }

    [Required, Column(Name = "document_set_id")]
    public string DocumentSetId { get; set; }

    [Required, Column(Name = "file_name")]
    public string FileName { get; set; }

    public string ParentFileName { get; set; }

    [Required]
    public DateTime Received { get; set; }

    [Required]
    public string AlloctorGrid { get; set; }
  }
}
