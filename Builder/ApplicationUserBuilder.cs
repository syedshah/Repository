namespace Builder
{
  using System;

  using Entities;

  public class ApplicationUserBuilder : Builder<ApplicationUser>
  {
    public ApplicationUserBuilder()
    {
      Instance = new ApplicationUser();
      Instance.LastPasswordChangedDate = DateTime.Now;
      Instance.LastLoginDate = DateTime.Now;
    }

    public ApplicationUserBuilder WithDomicile(Domicile domicile)
    {
      Instance.Domiciles.Add(new ApplicationUserDomicile
                            {
                              UserId = Instance.Id, 
                              DomicileId = domicile.Id
                            });
      return this;
    }
  }
}
