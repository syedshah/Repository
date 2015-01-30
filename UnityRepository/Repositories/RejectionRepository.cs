namespace UnityRepository.Repositories
{
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class RejectionRepository : BaseEfRepository<Rejection>, IRejectionRepository
  {
    public RejectionRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }
  }
}
