namespace UnityWeb.Models.Applicaiton
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  using UnityWeb.Models.Index;

  public class EditApplicationViewModel
  {
    private List<IndexViewModel> _indexes = new List<IndexViewModel>();

    public EditApplicationViewModel(Entities.Application applicaiton)
    {
      ApplicationId = applicaiton.Id;
      Code = applicaiton.Code;
      Description = applicaiton.Description;
      AddIndexes(applicaiton.IndexDefinitions);     
    }

    public List<IndexViewModel> Indexes
    {
      get { return _indexes; }
      set { _indexes = value; }
    }

    public int ApplicationId { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }

    public void AddIndexes(ICollection<Entities.IndexDefinition> indexDefinitions)
    {
      foreach (Entities.IndexDefinition indexDefinition in indexDefinitions)
      {
        var avm = new IndexViewModel(indexDefinition);
        Indexes.Add(avm);
      }
    }
  }
}