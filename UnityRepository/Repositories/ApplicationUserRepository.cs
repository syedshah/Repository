namespace UnityRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;
  using EFRepository;
  using Entities;
  using Exceptions;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class ApplicationUserRepository : BaseEfRepository<ApplicationUser>, IApplicationUserRepository
  {
    public ApplicationUserRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {

    }

    public ApplicationUser GetUserByName(string userName)
    {
      return (from a in Entities
              .Include(x => x.ManCos.Select(m => m.ManCo))
              .Include(x => x.Domiciles.Select(d => d.Domicile))
              where a.UserName == userName
              select a)
              .FirstOrDefault();
    }

    public void UpdateUserMancos(string userId, List<ManCo> manCos)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new UnityException("user id not valid");
      }

      var listUserManCos = manCos.Select(manCo => new ApplicationUserManCo { UserId = user.Id, ManCoId = manCo.Id }).ToList();
      user.ManCos.Clear();
      this.Update(user);
      user.ManCos = listUserManCos;
      this.Update(user);
    }

    public void UpdateUserDomiciles(string userId, List<Domicile> domiciles)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new UnityException("user id not valid");
      }

      var listUserDomiciles = domiciles.Select(domicile => new ApplicationUserDomicile { DomicileId = domicile.Id, UserId = user.Id }).ToList();
      user.Domiciles = listUserDomiciles;
      this.Update(user);
    }

    public IList<ManCo> GetManCos(string userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new UnityException("user id not valid");
      }

      var userManCos = user.ManCos;
      return userManCos.Select(applicationUserManCo => applicationUserManCo.ManCo).ToList();
    }

    public IList<ApplicationUser> GetUsers()
    {
      return Entities.ToList();
    }

    public void UpdateUserlastLogindate(string userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new UnityException("user id not valid");
      }

      user.LastLoginDate = DateTime.Now;
      user.FailedLogInCount = 0;
      user.IsLockedOut = false;

      this.Update(user);
    }

    public void UpdateUserLastPasswordChangedDate(string userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new UnityException("user id not valid");
      }

      user.LastPasswordChangedDate = DateTime.Now;

      this.Update(user);
    }

    public ApplicationUser UpdateUserFailedLogin(string userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new UnityException("user id not valid");
      }

      user.FailedLogInCount++;

      this.Update(user);
      return user;
    }

    public void UnlockUser(string userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new UnityException("user id not valid");
      }

      user.FailedLogInCount = 0;
      user.IsLockedOut = false;

      this.Update(user);
    }

    public IList<ApplicationUser> GetUsersByDomicile(IList<int> domicileIds)
    {
      return
        (from a in
           Entities.Include(x => x.ManCos.Select(m => m.ManCo)).Include(x => x.Domiciles.Select(d => d.Domicile))
         where a.Domiciles.Any(c => domicileIds.Contains(c.DomicileId))
         && a.IsLockedOut == false
         select a).ToList();
    }


    public PagedResult<ApplicationUser> GetUsersByDomicile(IList<int> domicileIds, int pageNumber, int numberOfItems)
    {
      var users = (from a in
                     Entities.Include(x => x.ManCos.Select(m => m.ManCo)).Include(x => x.Domiciles.Select(d => d.Domicile))
                   where a.Domiciles.Any(c => domicileIds.Contains(c.DomicileId))
                   select a).ToList();

      return new PagedResult<ApplicationUser>
      {
        CurrentPage = pageNumber,
        ItemsPerPage = numberOfItems,
        TotalItems = users.Count(),
        Results = users.OrderBy(c => c.UserName)
        .Skip((pageNumber - 1) * numberOfItems)
        .Take(numberOfItems)
        .ToList(),
        StartRow = ((pageNumber - 1) * numberOfItems) + 1,
        EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
      };
    }

    public void DeactivateUser(string userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new UnityException("user id not valid");
      }

      user.IsLockedOut = true;
      this.Update(user);
    }

    public bool IsLockedOut(string userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new UnityException("user id not valid");
      }

      return user.IsLockedOut;
    }

    public PagedResult<ApplicationUser> GetUserReport(int domicileId, int pageNumber, int numberOfItems)
    {
      var users = (from a in
                     Entities
                     .Include(x => x.ManCos.Select(m => m.ManCo))
                     .Include(x => x.Domiciles.Select(d => d.Domicile))
                     .Include(x => x.Roles)
                   where a.Domiciles.Any(c => c.DomicileId == domicileId)
                   select a).OrderBy(u => u.UserName).ToList();

      return new PagedResult<ApplicationUser>
      {
        CurrentPage = pageNumber,
        ItemsPerPage = numberOfItems,
        TotalItems = users.Count(),
        Results = users.OrderBy(c => c.UserName)
        .Skip((pageNumber - 1) * numberOfItems)
        .Take(numberOfItems)
        .ToList(),
        StartRow = ((pageNumber - 1) * numberOfItems) + 1,
        EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
      };
    }
  }
}
