namespace Entities
{
  using System.Collections.Generic;
  using Entities.File;
  using Microsoft.Build.Framework;

  public class DocType
  {
    public DocType()
    {
      XmlFiles = new List<XmlFile>();
      SubDocTypes = new List<SubDocType>();
      AutoApprovals = new List<AutoApproval>();
    }

    public DocType(string code): this()
    {
      Code = code;
    }

    public DocType(string code, string description) : this()
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

    public virtual ICollection<SubDocType> SubDocTypes { get; set; }

    public virtual ICollection<AutoApproval> AutoApprovals { get; set; }

    public void UpdateDocType(string code, string description)
    {
      if (!string.IsNullOrEmpty(code))
        Code = code;
      if (!string.IsNullOrEmpty(description))
        Description = description;
    }
  }
}
