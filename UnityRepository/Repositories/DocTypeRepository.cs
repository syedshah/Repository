namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Linq;
  using EFRepository;
  using Entities;
  using Exceptions;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class DocTypeRepository : BaseEfRepository<DocType>, IDocTypeRepository
  {
    public DocTypeRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public DocType GetDocType(int id)
    {
      return (from a in Entities
              where a.Id == id
              select a).FirstOrDefault();
    }

    public DocType GetDocType(string code)
    {
      return (from a in Entities
              where a.Code == code
              select a).FirstOrDefault();
    }

    public IList<DocType> GetDocTypes()
    {
      return Entities.OrderBy(d => d.Code).ToList();
    }

    public void Update(int docTypeId, string name, string description)
    {
      DocType docType = GetDocType(docTypeId);
      if (docType == null)
      {
        throw new UnityException("doctype id not valid");
      }
      docType.UpdateDocType(name, description);
      Update(docType);
    }

    public DocType AddSubDocType(int id, string code, string description)
    {
      DocType docType = GetDocType(id);
      if (docType == null)
      {
        throw new UnityException("Unable to find doc type");
      }
      docType.SubDocTypes.Add(new SubDocType
      {
        Code = code,
        Description = description
      });
      Save();
      return docType;
    }
  }
}
