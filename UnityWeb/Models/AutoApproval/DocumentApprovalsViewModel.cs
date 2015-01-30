namespace UnityWeb.Models.AutoApproval
{
  using System.Collections.Generic;
  using System.Linq;

  using UnityWeb.Models.AutoApproval;

  public class AutoApprovalsViewModel
  {
    private List<AutoApprovalViewModel> _autoApprovals = new List<AutoApprovalViewModel>();

    public AutoApprovalsViewModel(IList<Entities.DocType> docTypes, IList<Entities.SubDocType> subDocTypes, IList<Entities.ManCo> manCos)
    {
      AddAutoApprovalViewModel = new AddAutoApprovalViewModel(docTypes, subDocTypes, manCos);
    }

    public AutoApprovalsViewModel(IList<Entities.DocType> docTypes, IList<Entities.SubDocType> subDocTypes, int manCoId)
    {
      AddAutoApprovalViewModel = new AddAutoApprovalViewModel(docTypes, subDocTypes, manCoId);
    }

    public AddAutoApprovalViewModel AddAutoApprovalViewModel { get; set; }

    public List<AutoApprovalViewModel> AutoApprovals
    {
      get
      {
        return _autoApprovals;
      }
      set
      {
        _autoApprovals = value;
      }
    }

    public void AddAutoApprovals(IList<Entities.AutoApproval> documentApprovals, IList<Entities.DocType> docTypes, IList<Entities.SubDocType> subDocTypes)
    {
      var docTypeGroup = from d in documentApprovals group d by d.DocType;

      foreach (var docType in docTypeGroup)
      {
        var allSubDocTypes = (from d in subDocTypes where d.DocType.Code == docType.Key.Code select d).ToList();
        var autoApprovalsPerDocTypes = (from a in documentApprovals where a.DocType.Code == docType.Key.Code select a);

        if (allSubDocTypes.Count() == autoApprovalsPerDocTypes.Count())
        {
          var avm = new AutoApprovalViewModel(docType.Key, -1, documentApprovals.First().ManCoId);
          AutoApprovals.Add(avm);
        }
        else
        {
          foreach (var autoApproval in autoApprovalsPerDocTypes)
          {
            var avm = new AutoApprovalViewModel(autoApproval);
            AutoApprovals.Add(avm);
          }
        }
      }
    }
  }
}
