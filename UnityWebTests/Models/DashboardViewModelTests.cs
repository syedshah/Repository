namespace UnityWebTests.Models
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Entities.File;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.Dashboard;

  [TestFixture]
  public class DashboardViewModelTests
  {
    [Test]
    public void GivenADashBoardViewModel_WhenThereAreXmlFiles_ThenProcessingViewModelHasData()
    {
      var gridRunA = new GridRun { XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };
      var gridRunB = new GridRun { XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };

      var dashboardViewModel = new DashboardViewModel();

      dashboardViewModel.AddProcessing(new List<GridRun> { gridRunA, gridRunB });
      dashboardViewModel.DashboardProcessingViewModels.Should().HaveCount(2);
    }

    [Test]
    public void GivenADashBoardViewModel_WhenThereAreXmlFiles_ThenRecentlyProcessedViewModelHasData()
    {
      var gridRunA = new GridRun { EndDate = DateTime.Now, XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };
      var gridRunB = new GridRun { EndDate = DateTime.Now, XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };

      var dashboardViewModel = new DashboardViewModel();

      dashboardViewModel.AddRecent(new List<GridRun> { gridRunA, gridRunB });
      dashboardViewModel.DashboardProcessedViewModels.Should().HaveCount(2);
    }

    [Test]
    public void GivenADashBoardViewModel_WhenThereArRecenteXmlFiles_AndEndDateIsNull_ThenAUnityExceptionIsThrown()
    {
      var gridRunA = new GridRun { EndDate = null, XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() }};

      var dashboardViewModel = new DashboardViewModel();

      Action act = () => dashboardViewModel.AddRecent(new List<GridRun> { gridRunA });
      act.ShouldThrow<InvalidOperationException>();
    }

    [Test]
    public void GivenADashBoardViewModel_WhenThereAreXmlFiles_ThenRecentExceptionViewModelHasData()
    {
      var gridRunA = new GridRun { EndDate = DateTime.Now, XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };
      var gridRunB = new GridRun { EndDate = DateTime.Now, XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };

      var dashboardViewModel = new DashboardViewModel();

      dashboardViewModel.AddExceptions(new List<GridRun> { gridRunA, gridRunB });
      dashboardViewModel.DashboardExceptionViewModels.Should().HaveCount(2);
    }
  }
}
