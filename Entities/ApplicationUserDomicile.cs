namespace Entities
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class ApplicationUserDomicile
  {
    [Key, Column(Order = 0)]
    [ForeignKey("ApplicationUser")]
    public virtual string UserId { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("Domicile")]
    public virtual int DomicileId { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }

    public virtual Domicile Domicile { get; set; }
  }
}
