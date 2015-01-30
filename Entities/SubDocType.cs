namespace Entities
{
  using Microsoft.Build.Framework;
  using System.Collections.Generic;

  public class SubDocType
  {
    public SubDocType()
    {
      AutoApprovals = new List<AutoApproval>();
    }

    public SubDocType(string code)
      : this()
    {
      Code = code;
    }

    public SubDocType(string code, string description)
      : this()
    {
      Code = code;
      Description = description;
    }

    public int Id { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }

    public virtual DocType DocType { get; set; }

    public virtual ICollection<AutoApproval> AutoApprovals { get; set; }

    public void UpdateSubDocType(string code, string description)
    {
      if (!string.IsNullOrEmpty(code)) Code = code;
      if (!string.IsNullOrEmpty(description)) Description = description;
    }
  }
}
