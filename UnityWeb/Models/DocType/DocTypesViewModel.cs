namespace UnityWeb.Models.DocType
{
  using System.Collections.Generic;

  public class DocTypesViewModel
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
  }
}