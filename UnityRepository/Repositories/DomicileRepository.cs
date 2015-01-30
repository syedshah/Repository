namespace UnityRepository.Repositories
{
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class DomicileRepository : BaseEfRepository<Domicile>, IDomicileRepository
  {
    public DomicileRepository(string connectionString)
     : base(new UnityDbContext(connectionString))
    {
    }
  }
}
