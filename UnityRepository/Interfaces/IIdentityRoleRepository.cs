namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Microsoft.AspNet.Identity.EntityFramework;

  public interface IIdentityRoleRepository
  {
      IList<IdentityRole> GetRoles();
  }
}
