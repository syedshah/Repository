namespace UnityWebTests.Models
{
  using System.Collections.Generic;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.GridRun;

  [TestFixture]
  public class GridRunViewModelTests
  {
    [Test]
    public void GivenANewGridRunsCollection_WhenIAccessTheCollection_ThenItsTheSameAsTheSetValue()
    {
      var gridRuns = new List<GridRunDetailViewModel>();
      var model = new GridRunsDetailViewModel();
      model.GridRuns = gridRuns;
      model.GridRuns.ShouldBeEquivalentTo(gridRuns);
    }
  }
}
