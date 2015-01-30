namespace UnityWeb.Models.Search
{
  using System;
  using System.Collections.Generic;
  using UnityWeb.Models.DocType;

  public class SubDocTypeJsonResponse
  {
    public Exception exception;

    public string message;

    public string success;

    public string url;

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