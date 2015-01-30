namespace UnityRepository.Repositories
{
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class CheckOutRepository : BaseEfRepository<CheckOut>, ICheckOutRepository
  {
    public CheckOutRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public CheckOut GetCheckOut(string documentId)
    {
      return (from e in Entities
              where e.Document.DocumentId == documentId && e.CheckOutBy != null
              select e).FirstOrDefault();
    }
  }
}
