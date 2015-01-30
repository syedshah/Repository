namespace UnityRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using EFRepository;

  using Entities;

  using Exceptions;

  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  using System.Data.Entity;

  public class SessionRepository : BaseEfRepository<Session>, ISessionRepository
  {
    public SessionRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public Session GetSession(string guid)
    {
      return (from s in Entities 
              where s.Guid == guid 
              select s).FirstOrDefault();
    }

    public IList<Session> GetSessionsByGovReadOnlyAdmin(DateTime startDate, int domicileId)
    {
      return (from s in this.SessionAndUser()
              where s.Start >= startDate &&
              s.ApplicationUser.Domiciles.Any(d => d.DomicileId == domicileId) &&
              s.ApplicationUser.Roles.Any(d => d.Role.Name.ToLower() == "read only" || d.Role.Name.ToLower() == "governor" || d.Role.Name.ToLower() == "Admin")
              select s).ToList();
    }

    public void Update(string guid, DateTime end)
    {
      var session = this.GetSession(guid);

      if (session == null)
      {
        throw new UnityException("session guid is not valid");
      }

      session.End = end;

      Update(session);
    }

    private IQueryable<Session> SessionAndUser()
    {
      return Entities.Include(a => a.ApplicationUser);
    }
  }
}
