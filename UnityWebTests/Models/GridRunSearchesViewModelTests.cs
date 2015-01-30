namespace UnityWebTests.Models
{
  using System.Collections.Generic;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.GridRun;

  [TestFixture]
  public class GridRunSearchesViewModelTest
  {
    [Test]
    public void GivenAGridRunSearchesViewModel_WhenIGetACollectionOfGridRunSearchViewModels_ThenItIsInitialized()
    {
      var model = new GridRunSearchesViewModel();
      List<GridRunSearchViewModel> gridRuns = model.GridRuns;
      gridRuns.Should().NotBeNull();
    }
  }
}
