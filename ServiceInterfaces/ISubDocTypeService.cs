namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface ISubDocTypeService
  {
    SubDocType GetSubDocType(int id);
    IList<SubDocType> GetSubDocTypes();
    IList<SubDocType> GetSubDocTypes(int docTypeId);
    void Update(int subDocTypeId, string code, string description);
    SubDocType GetSubDocType(string code);
  }
}
