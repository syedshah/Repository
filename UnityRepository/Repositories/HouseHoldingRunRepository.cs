namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class HouseHoldingRunRepository : BaseEfRepository<HouseHoldingRun>, IHouseHoldingRunRepository
  {
    public HouseHoldingRunRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public IList<HouseHoldingRun> GetTopFifteenRecentHouseHeldGrids(List<int> manCoIds)
    {
      return Entities.Where(g => manCoIds.Contains(g.Documents.FirstOrDefault().ManCoId))
                .OrderByDescending(o => o.Id)
                .Take(15)
                .ToList();
    }
  }
}
