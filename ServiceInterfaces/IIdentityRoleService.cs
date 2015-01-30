namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Microsoft.AspNet.Identity.EntityFramework;

  public interface IIdentityRoleService
  {
    IList<IdentityRole> GetRoles();
  }
}
