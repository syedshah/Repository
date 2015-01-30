namespace Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class ApplicationUserService : IApplicationUserService
  {
    private readonly IApplicationUserRepository _applicationUserRepository;

    public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
    {
      this._applicationUserRepository = applicationUserRepository;
    }

    public void UpdateUserMancos(string userId, List<ManCo> mancos)
    {
      try
      {
        this._applicationUserRepository.UpdateUserMancos(userId, mancos);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update user mancos", e);
      }
    }

    public void UpdateUserDomiciles(string userId, List<Domicile> domiciles)
    {
      try
      {
        this._applicationUserRepository.UpdateUserDomiciles(userId, domiciles);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update user domiciles", e);
      }
    }


    public IList<ManCo> GetManCos(string userId)
    {
       try
       {
          return this._applicationUserRepository.GetManCos(userId);
       }
       catch (Exception e)
       {
         throw new UnityException("Unable to get user mancos", e);
       }
    }

  }
}
