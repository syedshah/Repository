namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IHouseHoldingRunRepository : IRepository<HouseHoldingRun>
  {
    IList<HouseHoldingRun> GetTopFifteenRecentHouseHeldGrids(List<int> manCoIds);
  }
}
