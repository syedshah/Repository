namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Linq;
  using EFRepository;
  using Entities;
  using Exceptions;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class SubDocTypeRepository : BaseEfRepository<SubDocType>, ISubDocTypeRepository
  {
    public SubDocTypeRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public SubDocType GetSubDocType(int id)
    {
      return (from a in Entities
              where a.Id == id
              select a).FirstOrDefault();
    }

    public IList<SubDocType> GetSubDocTypes()
    {
      return Entities.OrderBy(d => d.Code).ToList();
    }

    public IList<SubDocType> GetSubDocTypes(int docTypeId)
    {
      return (from a in Entities
              where a.DocType.Id == docTypeId
              select a).OrderBy(s => s.Code).ToList();
    }

    public void Update(int subDocTypeId, string name, string description)
    {
      SubDocType subDocType = GetSubDocType(subDocTypeId);
      if (subDocType == null)
      {
        throw new UnityException("sub doctype id not valid");
      }
      subDocType.UpdateSubDocType(name, description);
      Update(subDocType);
    }

    public SubDocType GetSubDocType(string code)
    {
        return (from a in Entities where a.Code == code select a).FirstOrDefault();
    }
  }
}
