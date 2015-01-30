namespace IdentityWrapper.Interfaces
{
  using System.Collections.Generic;
  using System.Data.Entity;
  using Entities;
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;

  public interface IUserManagerProvider
  {
    UserManager<ApplicationUser> UserManager { get; set; }

    DbContext dataDbContext { get; set; }

    System.Security.Claims.ClaimsIdentity CreateIdentity(ApplicationUser user, string authenticationType);

    IdentityResult CreateUser(ApplicationUser user, string password);

    ApplicationUser FindByName(string userName);

    ApplicationUser Find(string userName, string password);

    ApplicationUser FindById(string userId);

    IList<string> GetRoles(string userId);

    IdentityResult Create(ApplicationUser user, string password);

    IdentityResult Update(ApplicationUser user);

    IdentityResult RemoveFromRole(string userId, string role);

    IdentityResult AddToRole(string userId, string role);

    IdentityResult RemovePassword(string userId);

    IdentityResult AddPassword(string userId, string password);

    IdentityResult ChangePassword(string userId, string currentPassword, string newPassword);

    string HashPassword(string password);

    PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
  }
}
