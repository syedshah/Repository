namespace Entities
{
  using System;
  using System.Collections.Generic;
  using Microsoft.AspNet.Identity.EntityFramework;
  
  public class ApplicationUser : IdentityUser
  {
    public ApplicationUser()
    {
      ManCos = new List<ApplicationUserManCo>();
      Domiciles = new List<ApplicationUserDomicile>();
    }

    public ApplicationUser(string userName)
        : base(userName)
    {
      ManCos = new List<ApplicationUserManCo>();
      Domiciles = new List<ApplicationUserDomicile>(); 
    }

    public virtual IList<ApplicationUserDomicile> Domiciles { get; set; }

    public virtual IList<ApplicationUserManCo> ManCos { get; set; }

    public virtual IList<Session> Sessions { get; set; }

    public virtual string FirstName { get; set; }

    public string LastName { get; set; }

    public string Title { get; set; }

    public string Email { get; set; }

    public string Comment { get; set; }

    public virtual DateTime? LastLoginDate { get; set; }

    public virtual DateTime LastPasswordChangedDate { get; set; }

    public virtual bool IsApproved { get; set; }

    public virtual bool IsLockedOut { get; set; }

    public int FailedLogInCount { get; set; }
  }
}
