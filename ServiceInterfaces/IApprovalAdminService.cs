namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface IAutoApprovalService
  {
    AutoApproval GetAutoApproval(int autoApprovalId);

    AutoApproval GetAutoApproval(int manCoId, int docTypeId, int subDocTypeId);

    IList<AutoApproval> GetAutoApprovals();

    IList<AutoApproval> GetAutoApprovals(int manCoId);

    IList<AutoApproval> GetAutoApprovals(string docTypCode);

    void AddAutoApproval(int manCoId, int docTypeId, int subDocTypeId);

    void Update(int autoApprovalId, int manCoId, int docTypeId, int subDocTypeId);

    void Delete(string docTypeCode);

    void Delete(int autoApprovalId);
  }
}
