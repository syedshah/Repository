namespace Services
{
  using System;
  using System.Collections.Generic;
  using BusinessEngineInterfaces;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class HouseHoldingRunService : IHouseHoldingRunService
  {
    private readonly IHouseHoldingRunEngine _houseHoldingRunEngine;
    private readonly IHouseHoldingRunRepository _houseHoldingRunRepository;
    private readonly IUserService _userService;

    public HouseHoldingRunService(IHouseHoldingRunEngine houseHoldingRunEngine, IHouseHoldingRunRepository houseHoldingRunRepository, IUserService userService)
    {
      _houseHoldingRunEngine = houseHoldingRunEngine;
      _houseHoldingRunRepository = houseHoldingRunRepository;
      _userService = userService;
    }

    public void ProcessHouseHoldingRun()
    {
      try
      {
        _houseHoldingRunEngine.ProcessHouseHoldingRun();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to process householding run", e);
      }
    }

    public IList<HouseHoldingRun> GetTopFifteenRecentHouseHeldGrids()
    {
      try
      {
        return _houseHoldingRunRepository.GetTopFifteenRecentHouseHeldGrids(_userService.GetUserManCoIds());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve top 10 recent unapproved grids", e);
      }
    }

    public IList<HouseHoldingRun> GetTopFifteenRecentHouseHeldGrids(List<int> manCoIds)
    {
      try
      {
        return _houseHoldingRunRepository.GetTopFifteenRecentHouseHeldGrids(manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve top 10 recent unapproved grids", e);
      }
    }
  }
}
