namespace IdentityWrapper.Identity
{
  using IdentityWrapper.Interfaces;
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;
  using UnityRepository.Contexts;

  public class RoleManagerProvider : IRoleManagerProvider
  {

    public RoleManagerProvider(string connectionString)
    {
      this._roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new UnityDbContext(connectionString)));
    }

    private RoleManager<IdentityRole> _roleManager;

    public RoleManager<IdentityRole> RoleManager
    {
      get
      {
        return this._roleManager;
      }
      set
      {
        this._roleManager = value;
      }
    }

    public IdentityRole FindById(string roleId)
    {
       return this.RoleManager.FindById(roleId);
    }
  }
}
