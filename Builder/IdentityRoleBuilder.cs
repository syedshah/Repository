namespace Builder
{
  using Microsoft.AspNet.Identity.EntityFramework;

  public class IdentityRoleBuilder : Builder<IdentityRole>
  {
    public IdentityRoleBuilder()
    {
      Instance = new IdentityRole();
    }
  }
}
