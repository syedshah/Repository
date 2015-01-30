namespace ModelTests
{
  using System;
  using Entities;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;

  [Category("Unit")]
  [TestFixture]
  public class FileSyncTests
  {
    [Test]
    public void GivenvalidDate_WhenITryToCreateAFileSyncUsingGridRunIdConstructor_TheSyncDateIsValid()
    {
      var fileSync = new FileSync(It.IsAny<int>());
      fileSync.SyncDate.Should().NotBe(DateTime.MinValue);
    }
  }
}
