namespace UnityRepository.Interfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IGridRunRepository : IRepository<GridRun>
  {
    IList<GridRun> GetProcessing();

    IList<GridRun> GetProcessing(List<int> manCoIds);

    IList<GridRun> GetTopFifteenSuccessfullyCompleted();

    IList<GridRun> GetTopFifteenSuccessfullyCompleted(List<int> manCoIds);

    IList<GridRun> GetTopFifteenRecentExceptions();

    IList<GridRun> GetTopFifteenRecentExceptions(List<int> manCoIds);

    IList<GridRun> GetTopFifteenRecentUnapprovedGrids(List<int> manCoIds);

    IList<GridRun> GetTopFifteenGridsWithRejectedDocuments(List<int> manCoIds);

    IList<GridRun> GetTopFifteenGridsAwaitingHouseHolding(List<int> manCoIds);
      
    PagedResult<GridRun> GetGridRuns(int pageNumber, int numberOfItems, string houseHoldingGrid, List<int> manCoIds);
      
    PagedResult<GridRun> GetUnapproved(int pageNumber, int numberOfItems, List<int> manCoIds);
      
    GridRun GetGridRun(int id);

    GridRun GetGridRun(string grid);

    GridRun GetGridRun(string code, string grid);

    GridRun GetGridRun(string fileName, string code, string grid, DateTime startDate);

    IList<GridRun> Search(string grid);

    IList<GridRun> Search(string grid, List<int> manCoIds);
  }
}
