namespace IdentityWrapper.Identity
{
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;
  using Entities;
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;
  using UnityRepository.Contexts;
  using System;
  using System.Configuration;

  public class IndentityConfigUtility
  {
    private static UnityDbContext context = new UnityDbContext(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

    public static void Initialize()
    {       
        string name = "dstadmin";
        string password = "Password123";
        string adminRole = "dstadmin";

        // Create adminUser user
        var newAdmin = new ApplicationUser(name);
       
        newAdmin.PasswordHash = password;
        newAdmin.LastLoginDate = DateTime.Now;
        newAdmin.LastPasswordChangedDate = DateTime.Now;

        var user = CreateIfUserDoesNotExist(newAdmin, password, adminRole);

        AddOrUpdateDomicileIds(user);

        AddOrUpdateManCoIds(user);
    }

    private static ApplicationUser CreateIfUserDoesNotExist(ApplicationUser newUser, string password, string role)
    {
        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

        IdentityResult userResult = new IdentityResult();

        var user = UserManager.FindByName(newUser.UserName);

        if (user == null)
        {
            userResult = UserManager.Create(newUser, password);
        }

        if (userResult.Succeeded)
        {
            AddToRole(newUser.Id, role);
            user = newUser;
        }

        return user;
    }

    private static void AddToRole(string id, string role)
    {
      var userManager =
            new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(
                    context));

      var roleManager =
            new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(
                    context));

      if (!roleManager.RoleExists(role))
      {
         roleManager.Create(new IdentityRole(role));
      }

      userManager.AddToRole(id, role);
    }

    private static List<int> GetDomicileIds()
    {
       return context.Domicile.ToList().Select(x => x.Id).ToList();
    }


    private static List<int> GetManCoIds()
    {
        return context.Mancos.ToList().Select(x => x.Id).ToList();
    }

    private static void AddOrUpdateManCoIds(ApplicationUser user)
    {
      var listManCoIds = GetManCoIds();

      var listUserManCoIds = user.ManCos.Select(x => x.ManCoId).ToList();

      if (!listManCoIds.OrderBy(x => x).SequenceEqual(listUserManCoIds.OrderBy(x => x)))
      {
          foreach (var manCoId in listManCoIds)
          {
             if (!listUserManCoIds.Contains(manCoId))
             {
                 user.ManCos.Add(new ApplicationUserManCo {UserId = user.Id, ManCoId = manCoId});
             }
          } 
      }

      context.Entry(user).State = EntityState.Modified;
      context.SaveChanges();
    }

    private static void AddOrUpdateDomicileIds(ApplicationUser user)
    {
      var listDomicileIds = GetDomicileIds();

      var listUserDomicileIds = user.Domiciles.Select(x => x.DomicileId).ToList();

      if (listDomicileIds.OrderBy(x => x).SequenceEqual(listUserDomicileIds.OrderBy(x => x)))
      {
          foreach (var domicileId in listDomicileIds)
          {
             if (!listUserDomicileIds.Contains(domicileId))
             {
                 user.Domiciles.Add(new ApplicationUserDomicile() {UserId = user.Id, DomicileId = domicileId});
             }
          }
      }

        context.Entry(user).State = EntityState.Modified;
        context.SaveChanges();
    }
  }
}

