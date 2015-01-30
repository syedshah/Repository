namespace Entities
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class HouseHold
  {
    [ForeignKey("Document")]
    [Key]
    public int DocumentId { get; set; }

    public DateTime? HouseHoldDate { get; set; }

    public virtual Document Document { get; set; }
  }
}
