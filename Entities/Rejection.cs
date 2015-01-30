namespace Entities
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Rejection
  {
    [ForeignKey("Document")]
    [Key]
    public int DocumentId { get; set; }

    public string RejectedBy { get; set; }

    public DateTime? RejectionDate { get; set; }

    public virtual Document Document { get; set; }
  }
}
