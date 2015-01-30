namespace Entities
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  public class Application
  {
    public Application()
    {
      GridRuns = new List<GridRun>();
      IndexDefinitions = new List<IndexDefinition>();
    }

    public Application(string code, string description) : this()
    {
      Code = code;
      Description = description;
    }

    public int Id { get; set; }

    [Required]
    public string Code { get; set; }
    
    public string Description { get; set; }

    public virtual ICollection<GridRun> GridRuns { get; set; }

    public virtual ICollection<IndexDefinition> IndexDefinitions { get; set; }
  }
}
