namespace UnityWebTests.Models
{
  using System;

  using Entities;

  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.File;

  [TestFixture]
  public class RecentlyProcessedViewModelTests
  {
    [Test]
    public void GivenARecentlyProcessedViewModel_WhenICreateARecentlyProcessedViewModel_TheRecentlyProcessedViewModelIsCreatedProperly()
    {
      var vm = new DashboardProcessedViewModel("1.xml", "1.zip", "docType", new ManCo("code", "description"), new DateTime(2013, 05, 20, 17, 0, 0), new DateTime(2013, 05, 20, 17, 2, 30), "grid");
      vm.FileName.Should().Be("1.xml");
      vm.BigZip.Should().Be("1.zip");
      vm.DocType.Should().Be("docType");
      vm.ManCo.Should().Be("code - description");
      vm.StartDate.Should().HaveDay(20);
      vm.EndDate.Should().HaveHour(17);
      vm.Duration.Should().Be(2.Minutes().And(30.Seconds()));
    }
  }
}
