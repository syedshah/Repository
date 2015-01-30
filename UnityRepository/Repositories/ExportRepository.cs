namespace UnityRepository.Repositories
{
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class ExportRepository : BaseEfRepository<Export>, IExportRepository
  {
    public ExportRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }
  }
}
