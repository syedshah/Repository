namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class AppManCoEmailService : IAppManCoEmailService
  {
    private readonly IAppManCoEmailRepository _appManCoEmailRepository;

    public AppManCoEmailService(IAppManCoEmailRepository appManCoEmailRepository)
    {
      this._appManCoEmailRepository = appManCoEmailRepository;
    }

    public AppManCoEmail GetAppManCoEmail(int id)
    {
      try
      {
        return this._appManCoEmailRepository.GetAppManCoEmail(id);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve manco emails", e);
      }
    }

    public IList<AppManCoEmail> GetAppManCoEmails()
    {
      try
      {
        return this._appManCoEmailRepository.GetAppManCoEmails();
      }
      catch (Exception e)
      {
         throw new UnityException("Unable to retrieve all manco emails", e);
      }
    }

    public IList<AppManCoEmail> GetAppManCoEmails(int? appId, int? manCoId)
    {
      try
      {
          return this._appManCoEmailRepository.GetAppManCoEmails(appId, manCoId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve all manco emails", e);
      }
    }

    public PagedResult<AppManCoEmail> GetPagedAppManCoEmails(int pageNumber, int numberOfItems)
    {
      try
      {
          return this._appManCoEmailRepository.GetPagedAppManCoEmails(pageNumber, numberOfItems);
      }
      catch (Exception e)
      {
         throw new UnityException("Unable to retrieve all application manco emails", e);
      }
    }

    public PagedResult<AppManCoEmail> GetPagedAppManCoEmails(string accountNumber, int? appId, int? manCoId, int pageNumber, int numberOfItems)
    {
      try
      {
        return this._appManCoEmailRepository.GetPagedAppManCoEmails(accountNumber, appId, manCoId, pageNumber, numberOfItems);
      }
      catch (Exception e)
      {
          throw new UnityException("Unable to retrieve all application manco emails", e);
      }
    }

    public void CreateAppManCoEmail(int appId, int manCoId, int docTypeId, string accountNumber, string email)
    {   
      try
      {
        var appManCoEmail = new AppManCoEmail(appId, manCoId, docTypeId, accountNumber, email);

        this._appManCoEmailRepository.Create(appManCoEmail);

      }
      catch (Exception e)
      {
         throw new UnityException("Unable to create app manco email", e);     
      }
    }

    public void UpdateAppManCoEmail(int id, int appId, int manCoId, int docTypeId, string accountNumber, string email, string userName)
    {
      try
      {
        this._appManCoEmailRepository.UpdateAppManCoEmail(id, appId, manCoId, docTypeId, accountNumber, email, userName);
      }
      catch (Exception e)
      {
         throw new UnityException("Unable to update app manco email", e);
      }
    }

    public void DeleteAppManCoEmail(int id)
    {
      AppManCoEmail appManCoEmail;
      try
      {
        appManCoEmail = _appManCoEmailRepository.GetAppManCoEmail(id);
      }
      catch (Exception)
      {
        throw new UnityException("Unable to get app manco email");
      }

      try
      {
        _appManCoEmailRepository.Delete(appManCoEmail);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to delete app manco email");
      }
    }
  }
}
