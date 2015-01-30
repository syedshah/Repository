namespace UnityWeb.Models.SubDocType
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Models.DocType;

  public class AddSubDocTypeViewModel
  {
    public AddSubDocTypeViewModel()
    {
    }

    public AddSubDocTypeViewModel(IList<Entities.DocType> docTypes)
    {
      DocTypesViewModel = new DocTypesViewModel();
      DocTypesViewModel.AddDocTypes(docTypes);
    }

    public int Id { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public int DocTypeId { get; set; }
    
    public DocTypesViewModel DocTypesViewModel { get; set; }
  }
}