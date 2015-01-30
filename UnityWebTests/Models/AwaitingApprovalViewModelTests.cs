namespace UnityWebTests.Models
{
  using Entities;

  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.Dashboard;

    [TestFixture]
  public class AwaitingApprovalViewModelTests
  {
      [Test]
      public void GivenAwaitingApprovalViewModel_WhenICreateAwaitingApprovalViewModel_TheRecentlyAwaitingApprovalViewModelIsCreatedProperly()
      {
        var vm = new DashboardAwaitingApprovalViewModel("docType", "subDocType", new ManCo("code", "description"), "documentId");

        vm.DocType.Should().Be("docType");
        vm.SubDocType.Should().Be("subDocType");
        vm.ManCo.Should().Be("code - description");
        vm.DocumentId.Should().Be("documentId");
      }
  }
}
