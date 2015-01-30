namespace FileRepositoryTests
{
  using System;
  using System.IO;
  using System.Threading;
  using SystemFileAdapter;
  using Builder;
  using Entities;
  using FileRepository.Repositories;
  using FileSystemInterfaces;
  using FluentAssertions;
  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class ExportFileRepositoryTests
  {
    IFileInfoFactory _fileInfoFactory;
    private IDirectoryInfo _directoryInfo;
    private ExportFileRepository _exportFileRepository;
    const string BaseDirectory = "files";
    const string ExportFileDirectory = "files/exportFiles";
    private ExportFile _exportFile;
    private byte[] _fileData;

    [SetUp]
    public void SetUp()
    {
        _fileInfoFactory = new SystemFileInfoFactory();
        _directoryInfo = new SystemIoDirectoryInfo();

        var di = new DirectoryInfo(ExportFileDirectory);
        if (di.Exists)
        {
            di.Delete(true);
        }
        di.Create();
      _fileData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
      this._exportFile = BuildMeA.ExportFile(Guid.NewGuid(), _fileData);

      this._exportFileRepository = new ExportFileRepository(BaseDirectory, _fileInfoFactory, _directoryInfo);
    }

    [Test]
    public void GivenAnExportFile_WhenIWantToCreateADocument_ItIsCreated()
    {
      var result = this._exportFileRepository.Create(_exportFile);

      var fileNames = this._exportFileRepository.GetExportFileNames();

      result.Should().NotBeNull();
      result.FileName.Should().NotBeNullOrEmpty();
      fileNames.Contains(result.FileName.Replace(ExportFileDirectory, "").Substring(1)).ShouldBeEquivalentTo(true);
    }

    [Test]
    public void GivenAnExportFile_WhenIWantToDeleteADocument_ItIsDeleted()
    {
      var result = this._exportFileRepository.Create(_exportFile);

      this._exportFileRepository.Delete(result.FileName);

      var fileNames = this._exportFileRepository.GetExportFileNames();
      result.Should().NotBeNull();
      result.FileName.Should().NotBeNullOrEmpty();
      fileNames.Contains(result.FileName.Replace(ExportFileDirectory, "").Substring(1)).ShouldBeEquivalentTo(false);
    }

    [Test]
    public void GivenAExportFileName_WhenIWantToRetrieveIt_IamAbleToRetrieveIt()
    {
      this._exportFileRepository.Create(_exportFile);
      var fileNames = this._exportFileRepository.GetExportFileNames();

      fileNames.Should().NotBeNull();
      fileNames.Count.ShouldBeEquivalentTo(1);
    }

    [TearDown]
    public void TearDown()
    {
      var di = new DirectoryInfo(ExportFileDirectory);
      while (di.Exists)
      {
        try
        {
          di.Delete(true);
        }
        catch
        {
        }
        Thread.Sleep(10);
        di = new DirectoryInfo(ExportFileDirectory);
      }
      di = new DirectoryInfo(BaseDirectory);
      while (di.Exists)
      {
        try
        {
          di.Delete(true);
        }
        catch
        {
        }
        Thread.Sleep(10);
        di = new DirectoryInfo(BaseDirectory);
      }  
    }
  }
}
