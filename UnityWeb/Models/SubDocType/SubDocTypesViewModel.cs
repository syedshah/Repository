namespace UnityWeb.Models.DocType
{
  using System.Collections.Generic;
  using UnityWeb.Models.SubDocType;

  public class SubDocTypesViewModel
  {
    public SubDocTypesViewModel(IList<Entities.DocType> docTypes)
    {
      AddSubDocTypeViewModel = new AddSubDocTypeViewModel(docTypes);
    }

    public AddSubDocTypeViewModel AddSubDocTypeViewModel { get; set; }

    private List<SubDocTypeViewModel> _subDocTypes = new List<SubDocTypeViewModel>();

    public List<SubDocTypeViewModel> SubDocTypes
    {
      get { return _subDocTypes; }
      set { _subDocTypes = value; }
    }

    public void AddSubDocTypes(IList<Entities.SubDocType> subDocTypes)
    {
      foreach (Entities.SubDocType subDocType in subDocTypes)
      {
        var avm = new SubDocTypeViewModel(subDocType);
        SubDocTypes.Add(avm);
      }
    }
  }
}