using NUnit.Framework;
using Entities.File;

namespace ModelTests.File
{
  using System;

  using FluentAssertions;

  [TestFixture]
  public class ZipFileTests
  {
    [Test]
    public void GivenAZipFile_WhenITryToCreateABigZip_ABigZipFileIsCreated()
    {
      var bigZipFile = new ZipFile("documentSetId", "fileName", string.Empty, true, DateTime.Now);
      bigZipFile.BigZip.Should().BeTrue();
    }

    [Test]
    public void GivenAZipFile_WhenITryToCreateALittleZip_ALittleZipFileIsCreated()
    {
      var littleZipFile = new ZipFile("documentSetId", "fileName", string.Empty, false, DateTime.Now);
      littleZipFile.BigZip.Should().BeFalse();
    }

    [Test]
    public void GivenAZipFile_WhenICreateAZipFile_TheZipFileIsCreatedProperly()
    {
      var dateReceived = new DateTime(2013, 1, 1);
      var zipFile = new ZipFile("documentSetId", "fileName", "parentFileName", true, dateReceived);

      zipFile.DocumentSetId.Should().Be("documentSetId");
      zipFile.FileName.Should().Be("fileName");
      zipFile.ParentFileName.Should().Be("parentFileName");
      zipFile.Received.Should().Be(dateReceived);
    }
  }
}
