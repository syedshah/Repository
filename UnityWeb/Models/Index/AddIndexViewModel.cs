namespace UnityWeb.Models.Index
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  public class AddIndexViewModel
  {
    public AddIndexViewModel()
    {
    }

    public AddIndexViewModel(int applicationId)
    {
      Indexes = new List<IndexViewModel>();
      ApplicaitonId = applicationId;
    }

    [Required]
    public int ApplicaitonId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string ArchiveName { get; set; }

    [Required]
    public string ArchiveColumn { get; set; }

    public List<IndexViewModel> Indexes { get; set; }

    public void AddIndex(IndexViewModel indexViewModel)
    {
      Indexes.Add(indexViewModel);
    }
  }
}