namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface IDocTypeService
  {
    DocType CreateDocType(string code, string description);
    void AddSubDocType(int docTypeId, string code, string description);
    DocType GetDocType(int id);
    DocType GetDocType(string id);
    IList<DocType> GetDocTypes();
    void Update(int docTypeId, string code, string description);
  }
}
