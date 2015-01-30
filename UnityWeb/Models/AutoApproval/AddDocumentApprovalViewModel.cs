namespace UnityWeb.Models.AutoApproval
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Models.DocType;
  using UnityWeb.Models.ManCo;

  public class AddAutoApprovalViewModel
  {
    public AddAutoApprovalViewModel()
    {

    }

    public AddAutoApprovalViewModel(
      IList<Entities.DocType> docTypes, IList<Entities.SubDocType> subDocTypes, IList<Entities.ManCo> manCos)
    {
      DocTypesViewModel = new DocTypesViewModel();
      SubDocTypesViewModel = new SubDocTypesViewModel(docTypes);
      ManCosViewModel = new ManCosViewModel();

      DocTypesViewModel.AddDocTypes(docTypes);
      SubDocTypesViewModel.AddSubDocTypes(subDocTypes);
      ManCosViewModel.AddMancos(manCos);
    }

    public AddAutoApprovalViewModel(
      IList<Entities.DocType> docTypes, IList<Entities.SubDocType> subDocTypes, int manCoId)
    {
      DocTypesViewModel = new DocTypesViewModel();
      SubDocTypesViewModel = new SubDocTypesViewModel(docTypes);
      ManCosViewModel = new ManCosViewModel();

      DocTypesViewModel.AddDocTypes(docTypes);
      SubDocTypesViewModel.AddSubDocTypes(subDocTypes);
      ManCoId = manCoId;
    }

    [Required]
    public int DocTypeId { get; set; }

    [Required]
    public int SubDocTypeId { get; set; }

    [Required]
    public int ManCoId { get; set; }

    public DocTypesViewModel DocTypesViewModel { get; set; }

    public SubDocTypesViewModel SubDocTypesViewModel { get; set; }

    public ManCosViewModel ManCosViewModel { get; set; }

    public string SelectedDocText { get; set; }

    public string SelectedSubDocText { get; set; }

    public string SelectedManCoText { get; set; }

    public string SelectedApprovedValue { get; set; }

    public List<string> Approved { get; set; }

    public string SelectedApprovedText { get; set; }
  }
}