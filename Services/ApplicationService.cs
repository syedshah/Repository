namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class ApplicationService : IApplicationService
  {
    private readonly IApplicationRepository _applicationRepository;

    public ApplicationService(IApplicationRepository applicationRepository)
    {
      _applicationRepository = applicationRepository;
    }

    public void AddIndex(int applicationId, string name, string archiveName, string archiveColumn)
    {
      try
      {
        _applicationRepository.AddIndex(applicationId, name, archiveName, archiveColumn);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to add index", e);
      }
    }

    public Application CreateApplication(string code, string description)
    {
      try
      {
        var application = new Application(code, description);
        return _applicationRepository.Create(application);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create application", e);
      }
    }

    public Application GetApplication(string code)
    {
      try
      {
        return _applicationRepository.GetApplication(code);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve application", e);
      }
    }

    public Application GetApplication(int id)
    {
      try
      {
        return _applicationRepository.GetApplication(id);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve application", e);
      }
    }

    public IList<Application> GetApplications()
    {
      try
      {
        return _applicationRepository.GetApplications();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve applications", e);
      }
    }
  }
}
