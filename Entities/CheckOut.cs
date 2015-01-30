namespace Entities
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class CheckOut
  {
    [ForeignKey("Document")]
    [Key]
    public int DocumentId { get; set; }
    
    public virtual Document Document { get; set; }

    public string CheckOutBy { get; set; }

    public DateTime? CheckOutDate { get; set; }
  }
}
