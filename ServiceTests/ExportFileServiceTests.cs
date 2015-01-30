namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using BusinessEngineInterfaces;

  using Exceptions;

  using FluentAssertions;

  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;

  [TestFixture]
  public class ExportFileServiceTests
  {
    private Mock<IExportEngine> _exportEngine;
    private IExportFileService _exportFileService;
    private IList<string> _documentIds;

    [SetUp]
    public void SetUp()
    {
      _exportEngine = new Mock<IExportEngine>();
      _exportFileService = new ExportFileService(_exportEngine.Object);
      _documentIds = new List<string>();
      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());
    }

    [Test]
    public void GivenAListOfDocumentIds_WhenIWantToExportToZip_TheZipFileLocationIsReturned()
    {
      var zipFilePath = "file/exportFiles/9897797487dejw.zip";
      this._exportEngine.Setup(x => x.SaveAsZip(It.IsAny<List<string>>())).Returns(zipFilePath);
      var result = this._exportFileService.ExportToZip(_documentIds);

      result.Should().NotBeNullOrEmpty();
      result.ShouldBeEquivalentTo(zipFilePath);
      this._exportEngine.Verify(x => x.SaveAsZip(It.IsAny<List<string>>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAListOfDocumentIds_WhenIWantToExportToZip_IfDatabaseOrRepositoriesIsUnavaila_ThenAUnityExceptionIsThrown()
    {
      this._exportEngine.Setup(x => x.SaveAsZip(It.IsAny<List<string>>())).Throws(new UnityException());
      this._exportFileService.ExportToZip(_documentIds);

      this._exportEngine.Verify(x => x.SaveAsZip(It.IsAny<List<string>>()), Times.AtLeastOnce);
    }
  }
}
