namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Linq;
  using EFRepository;
  using Entities;
  using Exceptions;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class ApplicationRepository : BaseEfRepository<Application>, IApplicationRepository
  {
    public ApplicationRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public Application GetApplication(string code)
    {
      return (from a in Entities 
              where a.Code == code 
              select a).FirstOrDefault();
    }

    public Application GetApplication(int id)
    {
      return (from a in Entities
              where a.Id == id
              select a).FirstOrDefault();
    }

    public Application AddIndex(int id, string name, string archiveName, string archiveColumn)
    {
      Application applicaiton = GetApplication(id);
      if (applicaiton == null)
      {
        throw new UnityException("Unable to find post");
      }
      applicaiton.IndexDefinitions.Add(new IndexDefinition
      {
        Name = name,
        ArchiveName = archiveName,
        ArchiveColumn = archiveColumn,
      });
      Save();
      return applicaiton;
    }

    public IList<Application> GetApplications()
    {
      return Entities.ToList();
    }
  }
}

