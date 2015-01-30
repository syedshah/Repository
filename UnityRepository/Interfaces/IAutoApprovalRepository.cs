namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IAutoApprovalRepository : IRepository<AutoApproval>
  {
    AutoApproval GetAutoApproval(int id);

    AutoApproval GetAutoApproval(int manCoId, int docTypeId, int subDocTypeId);

    IList<AutoApproval> GetAutoApprovals();

    IList<AutoApproval> GetAutoApprovals(int manCoId);

    IList<AutoApproval> GetAutoApprovals(string docTypCode);

    void Update(int id, int manCoId, int docTypeId, int subDocTypeId);
  }
}
