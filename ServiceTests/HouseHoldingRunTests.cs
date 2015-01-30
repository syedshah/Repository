namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using BusinessEngineInterfaces;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [Category("Unit")]
  [TestFixture]
  public class HouseHoldingRunTests
  {
    private Mock<IHouseHoldingRunEngine> _houseHoldingRunEngine;
    private Mock<IHouseHoldingRunRepository> _houseHoldingRunRepository;
    private Mock<IUserService> _userService;
    private IHouseHoldingRunService _houseHoldingRunService;

    [SetUp]
    public void SetUp()
    {
      _houseHoldingRunEngine = new Mock<IHouseHoldingRunEngine>();
      _houseHoldingRunRepository = new Mock<IHouseHoldingRunRepository>();
      _userService = new Mock<IUserService>();
      _houseHoldingRunService = new HouseHoldingRunService(_houseHoldingRunEngine.Object, _houseHoldingRunRepository.Object, _userService.Object);
    }

    [Test]
    public void GivenAHouseHoldingRun_WhenProcessHouseHoldingRunIsRun_DataIsSavedToTheDatabase()
    {
      _houseHoldingRunService.ProcessHouseHoldingRun();
      _houseHoldingRunEngine.Verify(s => s.ProcessHouseHoldingRun(), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenIAskForRecentlyHouseHeldGrids_AndTheDataBaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      _houseHoldingRunRepository.Setup(p => p.GetTopFifteenRecentHouseHeldGrids(It.IsAny<List<int>>())).Throws<Exception>();

      Action act = () => _houseHoldingRunService.GetTopFifteenRecentHouseHeldGrids(It.IsAny<List<int>>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenIAskForUnapprovedGrids_ThenTheGridsAreReturned()
    {
      _houseHoldingRunRepository.Setup(p => p.GetTopFifteenRecentHouseHeldGrids(It.IsAny<List<int>>())).Returns(new List<HouseHoldingRun> { new HouseHoldingRun() });
      var grids = _houseHoldingRunService.GetTopFifteenRecentHouseHeldGrids(It.IsAny<List<int>>());
      grids.Count.Should().Be(1);
    }
  }
}
