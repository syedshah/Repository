namespace UnityWeb.Models.Applicaiton
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Models.Index;

  public class ApplicationViewModel
  {
    private List<IndexViewModel> _indexes = new List<IndexViewModel>();

    public ApplicationViewModel(Entities.Application applicaiton)
    {
      ApplicationId = applicaiton.Id;
      Code = applicaiton.Code;
      Description = applicaiton.Description;
      AddIndexViewModel = new AddIndexViewModel(applicaiton.Id);
      foreach (Entities.IndexDefinition indexDefinition in applicaiton.IndexDefinitions)
      {
        var ivm = new IndexViewModel(indexDefinition);
        AddIndexViewModel.AddIndex(ivm);
      }
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

    public AddIndexViewModel AddIndexViewModel { get; set; }
  }
}