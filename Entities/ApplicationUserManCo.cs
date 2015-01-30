namespace Entities
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class ApplicationUserManCo
  {
    
    [Key, Column(Order = 0)]
    [ForeignKey("ApplicationUser")]
    public virtual string UserId { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("ManCo")]
    public virtual int ManCoId { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }

    public virtual ManCo ManCo { get; set; }
  }
}
