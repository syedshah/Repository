namespace UnityRepository.Repositories
{
  using EFRepository;
  using Entities.File;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class ConFileRepository : BaseEfRepository<ConFile>, IConFileRepository
  {
    public ConFileRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }
  }
}
