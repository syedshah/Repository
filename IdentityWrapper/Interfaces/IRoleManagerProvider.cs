namespace IdentityWrapper.Interfaces
{
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;

  public interface IRoleManagerProvider
  {
    RoleManager<IdentityRole> RoleManager { get; set; }

    IdentityRole FindById(string roleId);
  }
}
