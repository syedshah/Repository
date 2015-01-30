namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class GridRunService : IGridRunService
  {
    private readonly IGridRunRepository _gridRunRepository;
    private readonly IUserService _userService;

    public GridRunService(IGridRunRepository gridRunRepository, IUserService userService)
    {
      _gridRunRepository = gridRunRepository;
      this._userService = userService;
    }

    public IList<GridRun> GetProcessing()
    {
      try
      {
        return _gridRunRepository.GetProcessing(_userService.GetUserManCoIds());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve grid runs currently processing", e);
      }
    }

    public IList<GridRun> GetTopFifteenSuccessfullyCompleted()
    {
      try
      {
        return _gridRunRepository.GetTopFifteenSuccessfullyCompleted(this._userService.GetUserManCoIds());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve last five recently processed grid runs", e);
      }
    }

    public IList<GridRun> GetTopFifteenRecentExceptions()
    {
      try
      {
        return _gridRunRepository.GetTopFifteenRecentExceptions(this._userService.GetUserManCoIds());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve top five recent exceptions", e);
      }
    }

    public IList<GridRun> GetTopFifteenRecentUnapprovedGrids()
    {
      try
      {
        return _gridRunRepository.GetTopFifteenRecentUnapprovedGrids(_userService.GetUserManCoIds());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve top 10 recent unapproved grids", e);
      }
    }

    public IList<GridRun> GetTopFifteenRecentUnapprovedGrids(List<int> manCoIds)
    {
      try
      {
        return _gridRunRepository.GetTopFifteenRecentUnapprovedGrids(manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve top 10 recent unapproved grids", e);
      }
    }

    public IList<GridRun> GetTopFifteenGridsWithRejectedDocuments()
    {
      try
      {
        return _gridRunRepository.GetTopFifteenGridsWithRejectedDocuments(_userService.GetUserManCoIds());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve top 10 recent unapproved grids", e);
      }
    }

    public IList<GridRun> GetTopFifteenGridsWithRejectedDocuments(List<int> manCoIds)
    {
      try
      {
        return _gridRunRepository.GetTopFifteenGridsWithRejectedDocuments(manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve top 10 recent unapproved grids", e);
      }
    }

    public IList<GridRun> GetTopFifteenGridsAwaitingHouseHolding()
    {
      try
      {
        return _gridRunRepository.GetTopFifteenGridsAwaitingHouseHolding(_userService.GetUserManCoIds());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve top 10 grids ready for householding", e);
      }
    }

    public IList<GridRun> GetTopFifteenGridsAwaitingHouseHolding(List<int> manCoIds)
    {
      try
      {
        return _gridRunRepository.GetTopFifteenGridsAwaitingHouseHolding(manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve top 10 grids ready for householding", e);
      }
    }

    public PagedResult<GridRun> GetGridRuns(int pageNumber, int numberOfItems, string houseHoldingGrid, List<int> manCoIds)
    {
      try
      {
        return _gridRunRepository.GetGridRuns(pageNumber, numberOfItems, houseHoldingGrid, manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve grid runs based upon house holding grid", e);
      }
    }

    public PagedResult<GridRun> GetGridRuns(int pageNumber, int numberOfItems, string houseHoldingGrid)
    {
      try
      {
        return _gridRunRepository.GetGridRuns(pageNumber, numberOfItems, houseHoldingGrid, _userService.GetUserManCoIds());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve grid runs based upon house holding grid", e);
      }
    }

    public PagedResult<GridRun> GetUnapproved(int pageNumber, int numberOfItems)
    {
      try
      {
        return _gridRunRepository.GetUnapproved(pageNumber, numberOfItems, _userService.GetUserManCoIds());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve unapproved GRIDs", e);
      }
    }

    public PagedResult<GridRun> GetUnapproved(int pageNumber, int numberOfItems, List<int> manCoIds)
    {
      try
      {
        return _gridRunRepository.GetUnapproved(pageNumber, numberOfItems, manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve unapproved GRIDs", e);
      }
    }

    public GridRun GetGridRun(int gridRunId)
    {
      try
      {
        return _gridRunRepository.GetGridRun(gridRunId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve Grid Run", e);
      }
    }

    public GridRun GetGridRun(string grid)
    {
      try
      {
        return _gridRunRepository.GetGridRun(grid);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve Grid Run", e);
      }
    }

    public GridRun GetGridRun(string code, string grid)
    {
      try
      {
        return _gridRunRepository.GetGridRun(code, grid);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve Grid Run", e);
      }
    }

    public GridRun GetGridRun(string fileName, string code, string grid, DateTime startDate)
    {
      try
      {
        return _gridRunRepository.GetGridRun(fileName, code, grid, startDate);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve Grid Run", e); 
      }
    }

    public void Create(int applicationId, int? xmlFileId, int status, string grid, bool isDebug, DateTime? startDate, DateTime? endDate)
    {
      var gridRun = new GridRun();
      gridRun.AddGridRun(applicationId, xmlFileId, endDate, grid, isDebug, startDate, status);

      try
      {
        _gridRunRepository.Create(gridRun);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create grid run", e);
      }
    }

    public void Update(int gridRunId, DateTime? startdate, DateTime? endDate, int status)
    {
      UpdateGridRun(gridRunId, startdate, endDate, status, null, null);
    }

    public void Update(int gridRunId, DateTime? startdate, DateTime? endDate, int status, int? xmlFileId)
    {
      UpdateGridRun(gridRunId, startdate, endDate, status, xmlFileId, null);
    }

    public void Update(int gridRunId, DateTime? startdate, DateTime? endDate, int status, int? xmlFileId, int houseHoldingRunId)
    {
      UpdateGridRun(gridRunId, startdate, endDate, status, xmlFileId, houseHoldingRunId);
    }

    public IList<GridRun> Search(string grid)
    {
      try
      {
        return _gridRunRepository.Search(grid);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to search for grid", e);
      }
    }


    public IList<GridRun> GetTopFifteenSuccessfullyCompleted(List<int> manCoIds)
    {
      try
      {
        return _gridRunRepository.GetTopFifteenSuccessfullyCompleted(manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve last five recently processed grid runs", e);
      }
    }


    public IList<GridRun> GetTopFifteenRecentExceptions(List<int> manCoIds)
    {
      try
      {
        return _gridRunRepository.GetTopFifteenRecentExceptions(manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve fifty recent exceptions");
      }
    }


    public IList<GridRun> GetProcessing(List<int> manCoIds)
    {
      try
      {
        return _gridRunRepository.GetProcessing(manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve grid runs currently processing", e);
      }
    }

    private void UpdateGridRun(int gridRunId, DateTime? startdate, DateTime? endDate, int status, int? xmlFileId, int? houseHoldingRunId)
    {
      try
      {
        GridRun gridRun = _gridRunRepository.GetGridRun(gridRunId);
        if (gridRun == null)
        {
          throw new UnityException("grid run not valid");
        }

        gridRun.UpdateGridRun(startdate, endDate, status, xmlFileId, houseHoldingRunId);

        _gridRunRepository.Update(gridRun);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update grid run", e);
      }
    }

    public IList<GridRun> Search(string grid, List<int> manCoIds)
    {
      try
      {
        return _gridRunRepository.Search(grid, manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to search for grid", e);
      }
    }
  }
}
