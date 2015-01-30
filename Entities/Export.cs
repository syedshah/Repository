namespace Entities
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Export
  {
    public int Id { get; set; }

    [ForeignKey("Document")]
    [Key]
    public int DocumentId { get; set; }

    public DateTime? ExportDate { get; set; }

    public virtual Document Document { get; set; }
  }
}
