namespace UnityWebTests.Models
{
  using System.Collections.Generic;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.GridRun;

  [TestFixture]
  public class GridRunsViewModelTests
  {
    [Test]
    public void GivenAGridRunsVieWModel_WhenIGetACollectionOfGridRuns_ThenItIsInitialized()
    {
      var model = new GridRunsDetailViewModel();
      List<GridRunDetailViewModel> gridRuns = model.GridRuns;
      gridRuns.Should().NotBeNull();
    }
  }
}
