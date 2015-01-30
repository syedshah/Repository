namespace UnityWeb.Models.DocType
{
  using System.ComponentModel.DataAnnotations;

  public class DocTypeViewModel
  {
    public DocTypeViewModel()
    {
    }

    public DocTypeViewModel(Entities.DocType docType)
    {
      Id = docType.Id;
      Code = docType.Code;
      Description = docType.Description;
    }

    public int Id { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }
  }
}