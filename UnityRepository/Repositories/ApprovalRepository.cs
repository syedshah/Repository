namespace UnityRepository.Repositories
{
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class ApprovalRepository : BaseEfRepository<Approval>, IApprovalRepository
  {
    public ApprovalRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }
  }
}
