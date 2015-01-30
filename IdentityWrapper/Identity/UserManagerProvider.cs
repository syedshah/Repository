namespace IdentityWrapper.Identity
{
  using System;
  using System.Collections.Generic;
  using System.Data.Entity;
  using Entities;
  using IdentityWrapper.Interfaces;
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;
  using UnityRepository.Contexts;

  public class UserManagerProvider : IUserManagerProvider, IDisposable
  {
    public UserManagerProvider(string connectionString)
    {
       this._dataDbContext = new UnityDbContext(connectionString);
       this._userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dataDbContext));
        conString = connectionString;
    }

    private DbContext _dataDbContext;

    private readonly string conString;

    private UserManager<ApplicationUser> _userManager;

    public DbContext dataDbContext
    {
        get
        {
            return _dataDbContext;
        }
        set
        {
            this._dataDbContext = value;
        }
    }
    
    public UserManager<ApplicationUser> UserManager
    {
      get
      {
        return this._userManager;
      }
      set
      {
        this._userManager = value;
      }
    }

    public void Dispose()
    {
        this.UserManager.Dispose();
        this.UserManager = null;
    }

    public System.Security.Claims.ClaimsIdentity CreateIdentity(ApplicationUser user, string authenticationType)
    {
      return this.UserManager.CreateIdentity(user, authenticationType);
    }

    public IdentityResult CreateUser(ApplicationUser user, string password)
    {
        IdentityResult result = new IdentityResult();

        using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new UnityDbContext(conString))))
        {
            result = userManager.Create(user, password);
        }
       
        return result;
    }

    public ApplicationUser FindByName(string userName)
    {
      return this.UserManager.FindByName(userName);
    }

    public ApplicationUser Find(string userName, string password)
    {
      return this.UserManager.Find(userName.Trim(), password.Trim());
    }

    public ApplicationUser FindById(string userId)
    {
       return this.UserManager.FindById(userId);
    }

    public IList<string> GetRoles(string userId)
    {
      return this.UserManager.GetRoles(userId);
    }

    public IdentityResult Create(ApplicationUser user, string password)
    {
      return this.UserManager.Create(user, password);
    }

    public IdentityResult RemoveFromRole(string userId, string role)
    {
      return this.UserManager.RemoveFromRole(userId, role);
    }

    public IdentityResult AddToRole(string userId, string role)
    {
      return this.UserManager.AddToRole(userId, role);
    }

    public IdentityResult Update(ApplicationUser user)
    {
       return this.UserManager.Update(user);
    }

    public IdentityResult RemovePassword(string userId)
    {
       return this.UserManager.RemovePassword(userId);
    }

    public IdentityResult AddPassword(string userId, string password)
    {
       return this.UserManager.AddPassword(userId, password);
    }

    public IdentityResult ChangePassword(string userId, string currentPassword, string newPassword)
    {
       return this.UserManager.ChangePassword(userId, currentPassword, newPassword);
    }

    public string HashPassword(string password)
    {
       return this.UserManager.PasswordHasher.HashPassword(password);
    }


    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
       var verificationResult = this.UserManager.PasswordHasher.VerifyHashedPassword(hashedPassword, providedPassword);
        return verificationResult;
    }
  }
}
