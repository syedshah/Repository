namespace Entities
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Approval
  {
    [ForeignKey("Document")]
    [Key]
    public int DocumentId { get; set; }

    public string ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public virtual Document Document { get; set; }
  }
}
