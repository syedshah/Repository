namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class GridRunServiceTests
  {
    private Mock<IGridRunRepository> _gridRunRepository;
    private Mock<IUserService> _userService;
    private IGridRunService _gridRunService;

    [SetUp]
    public void SetUp()
    {
      _gridRunRepository = new Mock<IGridRunRepository>();
      _userService = new Mock<IUserService>();
      _gridRunService = new GridRunService(_gridRunRepository.Object, _userService.Object);
        
    }

    [Test]
    public void GivenValidData_WhenTryingToRetreiveProcessingGridRuns_AndTheDatabaseIsNotAvailabe_ThenAnUnityExceptionIsThrown()
    {
      this._gridRunRepository.Setup(x => x.GetProcessing(It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetProcessing();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenProcessingGridRunsAreRetrieved_ThenProcessingGridsAreReturned()
    {
        _gridRunRepository.Setup(x => x.GetProcessing(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
      IList<GridRun> xmlFiles = _gridRunService.GetProcessing();

      xmlFiles.Should().NotBeEmpty()
        .And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenTryingToRetreiveProcessingGridRunsForCertainManCos_AndTheDatabaseIsNotAvailabe_ThenAnUnityExceptionIsThrown()
    {
        this._gridRunRepository.Setup(x => x.GetProcessing(It.IsAny<List<int>>())).Throws<Exception>();
        Action act = () => _gridRunService.GetProcessing(It.IsAny<List<int>>());

        act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenProcessingGridRunsAreRetrievedForCertainManCos_ThenProcessingGridsAreReturned()
    {
        _gridRunRepository.Setup(x => x.GetProcessing(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
        IList<GridRun> xmlFiles = _gridRunService.GetProcessing(It.IsAny<List<int>>());

        xmlFiles.Should().NotBeEmpty()
          .And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenTheFifteenMostRecentProcessedGridRunsAreRetrieved_AndTheDatabaseIsNotAvailabe_ThenAnUnityExceptionIsThrown()
    {
      this._gridRunRepository.Setup(x => x.GetTopFifteenSuccessfullyCompleted(It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetTopFifteenSuccessfullyCompleted();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenTheFifteenMostRecentProcessedGridsAreRetrieved_ThenTheRecentGridRunsAreReturned()
    {
      _gridRunRepository.Setup(x => x.GetTopFifteenSuccessfullyCompleted(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
      IList<GridRun> xmlFiles = _gridRunService.GetTopFifteenSuccessfullyCompleted();

      xmlFiles.Should().NotBeEmpty()
        .And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenTheFifteenMostRecentProcessedGridRunsAreRetrievedForCertainManCos_AndTheDatabaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      this._gridRunRepository.Setup(x => x.GetTopFifteenSuccessfullyCompleted(It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetTopFifteenSuccessfullyCompleted(It.IsAny<List<int>>());

        act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenTheFifteenMostRecentProcessedGridsAreRetrievedForCertainManCos_ThenTheRecentGridRunsAreReturned()
    {
      _gridRunRepository.Setup(x => x.GetTopFifteenSuccessfullyCompleted(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
      IList<GridRun> xmlFiles = _gridRunService.GetTopFifteenSuccessfullyCompleted(It.IsAny<List<int>>());

        xmlFiles.Should().NotBeEmpty()
          .And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenTopFifteenRecentExceptionsAreRetrieved_AndTheDatabaseIsNotAvailabe_ThenAnUnityExceptionIsThrown()
    {
      this._gridRunRepository.Setup(x => x.GetTopFifteenRecentExceptions(It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetTopFifteenRecentExceptions();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenTopFifteenRecentExceptionsAreRetrieved_ThenTheRecentExceptionsGridsAreReturned()
    {
      _gridRunRepository.Setup(x => x.GetTopFifteenRecentExceptions(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
      IList<GridRun> xmlFiles = _gridRunService.GetTopFifteenRecentExceptions();

      xmlFiles.Should().NotBeEmpty()
        .And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenTopFifteenRecentExceptionsAreRetrievedForCertainMancos_AndTheDatabaseIsNotAvailabe_ThenAnUnityExceptionIsThrown()
    {
      this._gridRunRepository.Setup(x => x.GetTopFifteenRecentExceptions(It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetTopFifteenRecentExceptions(It.IsAny<List<int>>());

        act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenTopFiveRecentExceptionsAreRetrievedForCertainMancos_ThenTheRecentExceptionsGridsAreReturned()
    {
      _gridRunRepository.Setup(x => x.GetTopFifteenRecentExceptions(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
      IList<GridRun> xmlFiles = _gridRunService.GetTopFifteenRecentExceptions(It.IsAny<List<int>>());

        xmlFiles.Should().NotBeEmpty()
          .And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenTheFifteenMostRecentExceptionGridsAreRetrieved_ThenTheRecentGridRunsAreReturned()
    {
      _gridRunRepository.Setup(x => x.GetTopFifteenRecentExceptions(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
      IList<GridRun> xmlFiles = _gridRunService.GetTopFifteenRecentExceptions();

      xmlFiles.Should().NotBeEmpty()
        .And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenITryToUpdateTheGridRunAndTheGridRunIsNotFound_IGetAUnityException()
    {
      GridRun gridRun = null;
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<int>())).Returns(gridRun);

      Action act = () => _gridRunService.Update(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int>(), It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenITryToUpdateTheGridRun_TheGridRunIsUpdated()
    {
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<int>())).Returns(new GridRun());

      _gridRunService.Update(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int>(), It.IsAny<int>());

      _gridRunRepository.Verify(s => s.Update(It.IsAny<GridRun>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenISearchForGridRunsGivenApplicaiton_IGetAGridRunReturned()
    {
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<string>(), It.IsAny<string>()));

      _gridRunService.GetGridRun(It.IsAny<string>(), It.IsAny<string>());

      _gridRunRepository.Verify(g => g.GetGridRun(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenISearchForGridRunsGivenApplicaitonAndFileName_IGetAGridRunReturned()
    {
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));

      _gridRunService.GetGridRun(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>());

      _gridRunRepository.Verify(g => g.GetGridRun(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenISearchForGridRunsGivenApplicaiton_AndTheDatabaseIsNotAvailable_IGetAGridRunReturned()
    {
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
      Action act = () => _gridRunService.GetGridRun(It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenISearchForGridRunsGivenAGrid_IGetAGridRunReturned()
    {
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<string>()));

      _gridRunService.GetGridRun(It.IsAny<string>());

      _gridRunRepository.Verify(g => g.GetGridRun(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenISearchForGridRunsGivenAGrid_AndTheDatabaseIsNotAvailable_IGetAGridRunReturned()
    {
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<string>())).Throws<Exception>();
      Action act = () => _gridRunService.GetGridRun(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenISearchForGridRunsGivenApplicaitonAndFileName_AndTheDatabaseIsNotAvailable_IGetAGridRunReturned()
    {
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Throws<Exception>();
      Action act = () =>  _gridRunService.GetGridRun(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenISearchForGridByGridRunId_IGetAGridRunReturned()
    {
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<int>()));

      _gridRunService.GetGridRun(It.IsAny<int>());

      _gridRunRepository.Verify(g => g.GetGridRun(It.IsAny<int>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenISearchForGridByGridRunId_AndTheDatabaseIsNotAvailable_IGetAGridRunReturned()
    {
      _gridRunRepository.Setup(g => g.GetGridRun(It.IsAny<int>())).Throws<Exception>();
      Action act = () => _gridRunService.GetGridRun(It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenISearchForGridRunsByGrid_IGetGridRuns()
    {
      _gridRunRepository.Setup(g => g.Search(It.IsAny<string>())).Returns(new List<GridRun> { new GridRun(), new GridRun() });

      _gridRunService.Search(It.IsAny<string>());

      _gridRunRepository.Verify(g => g.Search(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenTheGetTopFifteenGridsWithRejectedDocumentsAreRetrievedForCertainManCos_AndTheDatabaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      this._gridRunRepository.Setup(x => x.GetTopFifteenGridsWithRejectedDocuments(It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetTopFifteenGridsWithRejectedDocuments(It.IsAny<List<int>>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenTheTenGetTopFifteenGridsWithRejectedDocumentsAreRetrievedForCertainManCos_ThenTheRecentUnapporvedGridRunsAreReturned()
    {
      _gridRunRepository.Setup(x => x.GetTopFifteenGridsWithRejectedDocuments(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
      IList<GridRun> gridRuns = _gridRunService.GetTopFifteenGridsWithRejectedDocuments(It.IsAny<List<int>>());

      gridRuns.Should().NotBeEmpty()
        .And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenIAskForUnApprovedGrids_AndTheDataBaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      _gridRunRepository.Setup(p => p.GetUnapproved(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>())).Throws<Exception>();

      Action act = () => _gridRunService.GetUnapproved(It.IsAny<int>(), It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenIAskForUnapprovedGrids_ThenTheGridsAreReturned()
    {
      _gridRunRepository.Setup(p => p.GetUnapproved(1, 1, It.IsAny<List<int>>())).Returns(new PagedResult<GridRun> { Results = new List<GridRun>() });
      PagedResult<GridRun> products = _gridRunService.GetUnapproved(1, 1);
      products.Results.Count.Should().Be(0);
    }

    [Test]
    public void GivenAReuqestForLessThatAPageOfUnapprovedGrids_AndItsTheFirstPage_ThenTheGridIsReturned()
    {
      _gridRunRepository.Setup(p => p.GetUnapproved(1, 1, It.IsAny<List<int>>()))
                        .Returns(new PagedResult<GridRun> { Results = new List<GridRun>() });
      _gridRunService.GetUnapproved(1, 1);

      _gridRunRepository.Verify(p => p.GetUnapproved(1, 1, null), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenTheFifteenMostRecentUnapprovedGridRunsAreRetrieved_AndTheDatabaseIsNotAvailabe_ThenAnUnityExceptionIsThrown()
    {
      this._gridRunRepository.Setup(x => x.GetTopFifteenRecentUnapprovedGrids(It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetTopFifteenRecentUnapprovedGrids();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenTheFifteenMostRecentUnapprovedGridRunsAreRetrieved_ThenTheRecentExceptionsGridsAreReturned()
    {
      _gridRunRepository.Setup(x => x.GetTopFifteenRecentUnapprovedGrids(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
      IList<GridRun> xmlFiles = _gridRunService.GetTopFifteenRecentUnapprovedGrids();

      xmlFiles.Should().NotBeEmpty()
        .And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenISearchForGridRunsByGridAndManCoId_IGetGridRuns()
    {
      _gridRunRepository.Setup(g => g.Search(It.IsAny<string>(), It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun(), new GridRun() });

      _gridRunService.Search(It.IsAny<string>(), It.IsAny<List<int>>());

      _gridRunRepository.Verify(g => g.Search(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Once());
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenValidData_WhenISearchForGridRunsByGridAndManCoId_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      _gridRunRepository.Setup(g => g.Search(It.IsAny<string>(), It.IsAny<List<int>>())).Throws<UnityException>();

      _gridRunService.Search(It.IsAny<string>(), It.IsAny<List<int>>());

      _gridRunRepository.Verify(g => g.Search(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenTryingToRetreiveGridRunsByHHGrid_WithManCoFilter_AndTheDatabaseIsNotAvailabe_ThenAnUnityExceptionIsThrown()
    {
      _gridRunRepository.Setup(x => x.GetGridRuns(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetGridRuns(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenTryingToRetreiveGridRunsByHHGrid_WithManCoFilter_ThenAnUnityExceptionIsThrown()
    {
      _gridRunRepository.Setup(x => x.GetGridRuns(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>())).Returns(new PagedResult<GridRun> { Results = new List<GridRun>() { new GridRun()} });
      PagedResult<GridRun> gridRuns = _gridRunService.GetGridRuns(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>());

      gridRuns.Results.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenTryingToRetreiveGridRunsByHHGrid_AndTheDatabaseIsNotAvailabe_ThenAnUnityExceptionIsThrown()
    {
      _gridRunRepository.Setup(x => x.GetGridRuns(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetGridRuns(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenTryingToRetreiveGridRunsByHHGrid_ThenAnUnityExceptionIsThrown()
    {
      _gridRunRepository.Setup(x => x.GetGridRuns(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>())).Returns(new PagedResult<GridRun> { Results = new List<GridRun>() { new GridRun() } });
      PagedResult<GridRun> gridRuns = _gridRunService.GetGridRuns(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>());

      gridRuns.Results.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenTheFifteenMostRecentGridsAwaitingHouseHoldingAreRetrieved_AndTheDatabaseIsNotAvailabe_ThenAnUnityExceptionIsThrown()
    {
      this._gridRunRepository.Setup(x => x.GetTopFifteenGridsAwaitingHouseHolding(It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _gridRunService.GetTopFifteenGridsAwaitingHouseHolding();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenTheFifteenMostRecentGridsAwaitingHouseHoldingAreRetrieved_ThenTheRecentGridsAwaitingHouseHoldingAreReturned()
    {
      _gridRunRepository.Setup(x => x.GetTopFifteenGridsAwaitingHouseHolding(It.IsAny<List<int>>())).Returns(new List<GridRun> { new GridRun() });
      IList<GridRun> xmlFiles = _gridRunService.GetTopFifteenGridsAwaitingHouseHolding();

      xmlFiles.Should().NotBeEmpty()
        .And.HaveCount(1);
    }
  }
}
