namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IApplicationRepository : IRepository<Application>
  {
    Application GetApplication(string code);
    Application GetApplication(int id);
    Application AddIndex(int id, string name, string archiveName, string archiveColumn);
    IList<Application> GetApplications();
  }
}
