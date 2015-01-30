namespace UnityWebTests.Models
{
  using System;

  using Entities;

  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.Dashboard;

  [TestFixture]
  public class RecentExceptionViewModelTests
  {
    [Test]
    public void GivenARecentExceptionViewModel_WhenICreateARecentExceptionViewModel_TheRecentExceptionViewModelIsCreatedProperly()
    {
      var vm = new DashboardExceptionViewModel(
        "1.xml", "docType", new ManCo("code", "description"), new DateTime(2013, 05, 20, 17, 0, 0), "grid");

      vm.FileName.Should().Be("1.xml");
      vm.DocType.Should().Be("docType");
      vm.ManCo.Should().Be("code - description");
      vm.StartDate.Should().HaveDay(20);
    }
  }
}

