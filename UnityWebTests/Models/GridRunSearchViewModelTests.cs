namespace UnityWebTests.Models
{
  using System.Collections.Generic;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.GridRun;

  [TestFixture]
  public class GridRunSearchViewModelTests
  {
    [Test]
    public void GridRunSearchCollection_WhenIAccessTheCollection_ThenItIsTheSameAsTheSetValue()
    {
      var gridRuns = new List<GridRunSearchViewModel>();
      var model = new GridRunSearchesViewModel();
      model.GridRuns = gridRuns;

      model.GridRuns.Should().BeEquivalentTo(gridRuns);
    }
  }
}
