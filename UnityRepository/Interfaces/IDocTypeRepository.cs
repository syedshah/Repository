namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IDocTypeRepository : IRepository<DocType>
  {
    DocType GetDocType(int id);
    DocType GetDocType(string code);
    IList<DocType> GetDocTypes();
    void Update(int docTypeId, string code, string description);
    DocType AddSubDocType(int id, string code, string description);
  }
}
