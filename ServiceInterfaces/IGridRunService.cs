namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;

  public interface IGridRunService
  {
    IList<GridRun> GetProcessing();

    IList<GridRun> GetProcessing(List<int> manCoIds);

    IList<GridRun> GetTopFifteenSuccessfullyCompleted();

    IList<GridRun> GetTopFifteenSuccessfullyCompleted(List<int> manCoIds);

    IList<GridRun> GetTopFifteenRecentExceptions();

    IList<GridRun> GetTopFifteenRecentExceptions(List<int> manCoIds);

    IList<GridRun> GetTopFifteenRecentUnapprovedGrids();

    IList<GridRun> GetTopFifteenRecentUnapprovedGrids(List<int> manCoIds);

    IList<GridRun> GetTopFifteenGridsWithRejectedDocuments();

    IList<GridRun> GetTopFifteenGridsWithRejectedDocuments(List<int> manCoIds);

    IList<GridRun> GetTopFifteenGridsAwaitingHouseHolding();

    IList<GridRun> GetTopFifteenGridsAwaitingHouseHolding(List<int> manCoIds);

    PagedResult<GridRun> GetGridRuns(int pageNumber, int numberOfItems, string houseHoldingGrid);

    PagedResult<GridRun> GetGridRuns(int pageNumber, int numberOfItems, string houseHoldingGrid, List<int> manCoIds);

    PagedResult<GridRun> GetUnapproved(int pageNumber, int numberOfItems);

    PagedResult<GridRun> GetUnapproved(int pageNumber, int numberOfItems, List<int> manCoIds);

    GridRun GetGridRun(int gridRunId);

    GridRun GetGridRun(string grid);

    GridRun GetGridRun(string code, string grid);

    GridRun GetGridRun(string fileName, string code, string grid, DateTime startDate);

    void Create(int applicationId, int? xmlFileId, int status, string grid, bool isDebug, DateTime? startDate, DateTime? endDate);

    void Update(int gridRunId, DateTime? startdate, DateTime? endDate, int status);

    void Update(int gridRunId, DateTime? startdate, DateTime? endDate, int status, int? xmlFileId);

    void Update(int gridRunId, DateTime? startdate, DateTime? endDate, int status, int? xmlFileId, int houseHoldingRunId);

    IList<GridRun> Search(string grid);

    IList<GridRun> Search(string grid, List<int> manCoIds);
  }
}
