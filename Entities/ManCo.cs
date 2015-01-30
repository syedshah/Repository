namespace Entities
{
  using Microsoft.Build.Framework;
  using System.Collections.Generic;
  using Entities.File;

  public class ManCo
  {
    public ManCo()
    {
      XmlFiles = new List<XmlFile>();
      AutoApprovals = new List<AutoApproval>();
      Users = new List<ApplicationUserManCo>();
    }

    public ManCo(string code)
      : this()
    {
      Code = code;
    }

    public ManCo(string code, string description) : this()
    {
      Code = code;
      Description = description;
    }

    public int Id { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }

    public virtual ICollection<XmlFile> XmlFiles { get; set; }

    public virtual ICollection<AutoApproval> AutoApprovals { get; set; }

    public virtual IList<ApplicationUserManCo> Users { get; set; }

    public int? DomicileId { get; set; }

    public virtual Domicile Domicile { get; set; }

    public void UpdateManCo(string code, string description)
    {
      if (!string.IsNullOrEmpty(code))
        Code = code;
      if (!string.IsNullOrEmpty(description))
        Description = description;
    }
  }
}
