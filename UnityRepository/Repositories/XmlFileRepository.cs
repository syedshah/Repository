namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Linq;
  using EFRepository;
  using Entities.File;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class XmlFileRepository : BaseEfRepository<XmlFile>, IXmlFileRepository
  {
    public XmlFileRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public XmlFile GetFile(int id)
    {
      return (from f in Entities 
              where f.Id == id 
              select f).FirstOrDefault();
    }

    public XmlFile GetFile(string fileName)
    {
      return Entities
              .Where(f => f.FileName == fileName)
              .OrderByDescending(o => o.Id)
              .FirstOrDefault();
    }

    public IList<XmlFile> Search(string fileName)
    {
      return (from f in Entities 
              where f.FileName.Contains(fileName) 
              select f).ToList();
    }

    public IList<XmlFile> Search(string fileName, List<int> manCoIds)
    {
      return (from f in Entities
              where f.FileName.Contains(fileName) && manCoIds.Contains(f.ManCoId)
              select f).ToList();
    }
  }
}
