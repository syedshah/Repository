
namespace UnityWebTests.Models
{
  using System;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.File;

  [TestFixture]
  public class ProcessingViewModelTests
  {
    [Test]
    public void GivenAProcessingViewModel_WhenICreateAProcessingViewModel_TheProcessingViewModelCreatedProperly()
    {
      var processingViewModel = new DashboardProcessingViewModel("1.xml", "1.zip", "docType", new ManCo("code", "description"), DateTime.Now, "grid");
      processingViewModel.FileName.Should().Be("1.xml");
      processingViewModel.BigZip.Should().Be("1.zip");
      processingViewModel.DocType.Should().Be("docType");
      processingViewModel.ManCo.Should().Be("code - description");
      processingViewModel.StartDate.Should().HaveYear(DateTime.Today.Year);
    }
  }
}
