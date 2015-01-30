namespace UnityWeb.Models.DocType
{
  using System.ComponentModel.DataAnnotations;

  public class SubDocTypeViewModel
  {
    public SubDocTypeViewModel()
    {
    }

    public SubDocTypeViewModel(Entities.SubDocType subDocType)
    {
      Id = subDocType.Id;
      Code = subDocType.Code;
      Description = subDocType.Description;
      DocTypeViewModel = new DocTypeViewModel(subDocType.DocType);
    }

    public int Id { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }

    public DocTypeViewModel DocTypeViewModel { get; set; }
  }
}