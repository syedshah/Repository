namespace BusinessEngineTests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using BusinessEngineInterfaces;
  using BusinessEngines;
  using Entities;
  using FileRepository.Interfaces;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  using ZipManagerWrapper;

  [TestFixture]
  public class ExportEngineTests
  {
    private Mock<IExportFileRepository> _exportFileRepository;
    private Mock<IDocumentService> _documentService;
    private Mock<IZipManager> _zipManager;
    private IExportEngine _exportEngine;
    private string _path;
    private IList<string> _documentIds;

    [SetUp]
    public void SetUp()
    {
      _exportFileRepository = new Mock<IExportFileRepository>();
      _documentService = new Mock<IDocumentService>();
      _zipManager = new Mock<IZipManager>();
      _path = "files/exportFiles"; 
      _documentIds = new List<string>();

      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());
      _documentIds.Add(Guid.NewGuid().ToString());

      this._exportEngine = new ExportEngine(
          _exportFileRepository.Object, 
          _documentService.Object,
          _zipManager.Object,
          _path);

    }

    [Test]
    public void GivenAListOfDocumentIds_WhenIWantTosaveAsZip_AFilePathToZipIsReturned()
    {
      this._documentService.Setup(x => x.GetDocumentStream(It.IsAny<string>())).Returns(new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 });

      this._exportFileRepository.Setup(x => x.Create(It.IsAny<ExportFile>())).Returns(new ExportFile()
                                                                                          {
                                                                                              FileData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
                                                                                              FileName = _path + "/" + Guid.NewGuid().ToString() + ".pdf",
                                                                                              Id = Guid.NewGuid()
                                                                                          });
      this._zipManager.Setup(x => x.Zip(It.IsAny<string>(), It.IsAny<string[]>()));

      this._exportFileRepository.Setup(x => x.Delete(It.IsAny<string>()));
        
      var result = this._exportEngine.SaveAsZip(this._documentIds.ToList());

      result.Should().NotBeNull();

      this._documentService.Verify(x => x.GetDocumentStream(It.IsAny<string>()), Times.AtLeastOnce);

      this._exportFileRepository.Verify(x => x.Create(It.IsAny<ExportFile>()), Times.AtLeastOnce);

      this._zipManager.Verify(x => x.Zip(It.IsAny<string>(), It.IsAny<string[]>()), Times.AtLeastOnce);

      this._exportFileRepository.Verify(x => x.Delete(It.IsAny<string>()), Times.AtLeastOnce);
    }
  }
}
