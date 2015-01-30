namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface IApplicationService
  {
    void AddIndex(int applicationId, string name, string archiveName, string archiveColumn);
    Application CreateApplication(string code, string description);
    Application GetApplication(string code);
    Application GetApplication(int id);
    IList<Application> GetApplications();
  }
}
