
namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Linq;
  using EFRepository;
  using Entities;
  using Exceptions;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class AutoApprovalRepository : BaseEfRepository<AutoApproval>, IAutoApprovalRepository
  {
    public AutoApprovalRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public AutoApproval GetAutoApproval(int id)
    {
      return (from a in Entities where a.Id == id select a).FirstOrDefault();
    }

    public AutoApproval GetAutoApproval(int manCoId, int docTypeId, int subDocTypeId)
    {
      return (from d in Entities
              where d.ManCoId == manCoId & d.DocTypeId == docTypeId & d.SubDocTypeId == subDocTypeId
              select d).FirstOrDefault();
    }

    public IList<AutoApproval> GetAutoApprovals()
    {
      return Entities.ToList();
    }

    public IList<AutoApproval> GetAutoApprovals(int manCoId)
    {
      return (from a in Entities 
              where a.ManCoId == manCoId select a)
              .OrderBy(d => d.DocType.Code).ToList();
    }

    public IList<AutoApproval> GetAutoApprovals(string docTypCode)
    {
      return (from a in Entities
              where a.DocType.Code == docTypCode
              select a).ToList();
    }

    public void Update(int id, int manCoId, int docTypeId, int subDocTypeId)
    {
      var autoApproval = this.GetAutoApproval(id);

      if (autoApproval == null)
      {
        throw new UnityException("auto Approval id not valid");
      }

      autoApproval.ManCoId = manCoId;
      autoApproval.DocTypeId = docTypeId;
      autoApproval.SubDocTypeId = subDocTypeId;

      Update(autoApproval);
    }
  }
}
