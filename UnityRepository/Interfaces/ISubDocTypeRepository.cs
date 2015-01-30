namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface ISubDocTypeRepository : IRepository<SubDocType>
  {
    SubDocType GetSubDocType(int id);
    SubDocType GetSubDocType(string code);
    IList<SubDocType> GetSubDocTypes();
    IList<SubDocType> GetSubDocTypes(int docTypeId);
    void Update(int subDocTypeId, string code, string description);
  }
}
