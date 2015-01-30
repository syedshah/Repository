namespace UnityRepository.Repositories
{
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class FileSyncRepository : BaseEfRepository<FileSync>, IFileSyncRepository
  {
    public FileSyncRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public FileSync GetLatest()
    {
      return Entities.OrderByDescending(e => e.GridRunId).FirstOrDefault();
    }
  }
}
