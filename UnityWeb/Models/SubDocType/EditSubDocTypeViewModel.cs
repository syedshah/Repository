namespace UnityWeb.Models.DocType
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  public class EditSubDocTypeViewModel
  {
    private List<DocTypeViewModel> _docTypes = new List<DocTypeViewModel>();

    public List<DocTypeViewModel> DocTypes
    {
      get { return _docTypes; }
      set { _docTypes = value; }
    }

    public void AddDocTypes(IList<Entities.DocType> docTypes)
    {
      foreach (Entities.DocType docType in docTypes)
      {
        var avm = new DocTypeViewModel(docType);
        DocTypes.Add(avm);
      }
    }

    [Required]
    public int SubDocTypeId { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public int DocTypeId { get; set; }
  }
}