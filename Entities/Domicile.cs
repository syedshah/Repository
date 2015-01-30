namespace Entities
{
  using System.Collections.Generic;
  using Entities.File;
  using Microsoft.Build.Framework;

  public class Domicile
  {
    public Domicile()
    {
      XmlFiles = new List<XmlFile>();
      Users = new List<ApplicationUserDomicile>();
    }

    public Domicile(string code, string description) : this()
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

    public virtual ICollection<ManCo> ManCos { get; set; }

    public virtual IList<ApplicationUserDomicile> Users { get; set; }

  }
}
