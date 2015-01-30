namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;
  using EFRepository;
  using Entities;
  using Exceptions;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class ManCoRepository : BaseEfRepository<ManCo>, IManCoRepository
  {
    public ManCoRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public ManCo GetManCo(int id)
    {
      return (from a in Entities
              where a.Id == id
              select a).FirstOrDefault();
    }

    public ManCo GetManCo(string code)
    {
      return (from a in Entities
              where a.Code == code
              select a).FirstOrDefault();
    }

    public IList<ManCo> GetManCos()
    {
      return Entities.OrderBy(m => m.Description).ToList();
    }

    public void Update(int docTypeId, string name, string description)
    {
      ManCo docType = GetManCo(docTypeId);
      if (docType == null)
      {
        throw new UnityException("man co id not valid");
      }
      docType.UpdateManCo(name, description);
      Update(docType);
    }

    public IList<ManCo> GetManCos(IList<int> manCoIds)
    {
        return (from a in Entities 
                where manCoIds.Contains(a.Id) select a).AsNoTracking().ToList();
    }

    public IEnumerable<ManCo> GetManCos(int domicileId)
    {
        return (from a in Entities where a.DomicileId == domicileId select a).ToList();
    }

    public IList<ManCo> GetManCosByUserId(string userId)
    {
        return (from a in Entities
                where a.Users.Any(x => x.UserId == userId)
                select a).OrderBy(o => o.Description).AsNoTracking().ToList();
    }
  }
}
