namespace UnityWeb.Models.AutoApproval
{
  using UnityWeb.Models.DocType;

  public class AutoApprovalViewModel
  {
    public AutoApprovalViewModel(Entities.DocType docType, int autoApprovalId, int manCoId)
    {
      this.DocTypeViewModel = new DocTypeViewModel(docType);
      this.SubDocTypeViewModel = new SubDocTypeViewModel()
                                   {
                                     Code = "All"
                                   };
      this.Id = autoApprovalId;
      this.ManCoId = manCoId;
    }

    public AutoApprovalViewModel(Entities.AutoApproval autoApproval)
    {
      this.Id = autoApproval.Id;
      this.DocTypeViewModel = new DocTypeViewModel(autoApproval.DocType);
      this.SubDocTypeViewModel = new SubDocTypeViewModel(autoApproval.SubDocType);
      this.ManCoId = autoApproval.ManCoId;
    }

    public int Id { get; set; }

    public DocTypeViewModel DocTypeViewModel { get; set; }

    public SubDocTypeViewModel SubDocTypeViewModel { get; set; }

    public int ManCoId { get; set; }
  }
}