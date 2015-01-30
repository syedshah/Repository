namespace Services
{
  using System;
  using System.Collections.Generic;
  using Exceptions;
  using Microsoft.AspNet.Identity.EntityFramework;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class IdentityRoleService : IIdentityRoleService
  {

    private IIdentityRoleRepository _identityRoleRepository;

    public IdentityRoleService(IIdentityRoleRepository identityRoleRepository)
    {
      this._identityRoleRepository = identityRoleRepository;
    }

    public IList<IdentityRole> GetRoles()
    {
      try
      {
        return this._identityRoleRepository.GetRoles();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve roles", e);
      }
    }
  }
}
