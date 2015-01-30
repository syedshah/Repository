namespace UnityWeb.Models.AutoApproval
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Models.DocType;
  using UnityWeb.Models.ManCo;

  public class EditAutoApprovalViewModel
  {
    private List<SubDocTypeViewModel> _subDocTypes = new List<SubDocTypeViewModel>();

    private List<DocTypeViewModel> _docTypes = new List<DocTypeViewModel>();

    private List<ManCoViewModel> _manCos = new List<ManCoViewModel>();

    public List<DocTypeViewModel> DocTypes
    {
      get
      {
        return _docTypes;
      }
      set
      {
        _docTypes = value;
      }
    }

    public List<SubDocTypeViewModel> SubDocTypes
    {
      get
      {
        return _subDocTypes;
      }
      set
      {
        _subDocTypes = value;
      }
    }

    public List<ManCoViewModel> ManCos
    {
      get
      {
        return _manCos;
      }
      set
      {
        _manCos = value;
      }
    }

    public int AutoApprovalId { get; set; }

    [Required]
    public int DocTypeId { get; set; }

    [Required]
    public string DocTypeCode { get; set; }

    [Required]
    public int SubDocTypeId { get; set; }

    [Required]
    public int ManCoId { get; set; }

    public void AddDocTypes(IList<Entities.DocType> docTypes)
    {
      foreach (Entities.DocType docType in docTypes)
      {
        var avm = new DocTypeViewModel(docType);
        DocTypes.Add(avm);
      }
    }

    public void AddSubDocTypes(IList<Entities.SubDocType> subDocTypes)
    {
      foreach (Entities.SubDocType subDocType in subDocTypes)
      {
        var avm = new SubDocTypeViewModel(subDocType);
        SubDocTypes.Add(avm);
      }
    }

    public void AddManCos(IList<Entities.ManCo> manCos)
    {
      foreach (Entities.ManCo manCo in manCos)
      {
        var avm = new ManCoViewModel(manCo);
        avm.AddDisplayName(manCos);
        ManCos.Add(avm);
      }
    }
  }
}