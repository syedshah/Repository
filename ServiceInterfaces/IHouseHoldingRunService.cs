namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface IHouseHoldingRunService
  {
    void ProcessHouseHoldingRun();
    IList<HouseHoldingRun> GetTopFifteenRecentHouseHeldGrids();
    IList<HouseHoldingRun> GetTopFifteenRecentHouseHeldGrids(List<int> manCoIds);
  }
}
