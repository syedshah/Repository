namespace EFRepositoryTests
{
  using System.IO;
  using System.Threading;
  using SystemFileAdapter;
  using Entities;
  using FileRepository.Repositories;
  using FileSystemInterfaces;
  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class ExportFileRepositoryTests
  {
    private IFileInfoFactory _fileInfoFactory;

    private IDirectoryInfo _directoryInfo;

    private ExportFileRepository _exportFileRepository;

    private const string BaseDirectory = "files";

    private const string ExportFilesDirectory = "files/exportFiles";

    private FileInfo _file;

    [SetUp]
    public void Setup()
    {
      _fileInfoFactory = new SystemFileInfoFactory();
      _directoryInfo = new SystemIoDirectoryInfo();

      var di = new DirectoryInfo(ExportFilesDirectory);
      if (di.Exists)
      {
        di.Delete(true);
      }
      di.Create();
      _exportFileRepository = new ExportFileRepository(BaseDirectory, _fileInfoFactory, _directoryInfo);
      CreateFiles(ExportFilesDirectory);
    }

    [TearDown]
    public void TearDown()
    {
      var di = new DirectoryInfo(ExportFilesDirectory);
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
        di = new DirectoryInfo(ExportFilesDirectory);
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

    private void CreateFiles(string directory)
    {
      for (int i = 0; i < 10; i++)
      {
        using (File.Create(Path.Combine(directory, "name" + i + ".json"))) ;
      }
    }

    //[Test]
    //public void GivenASetOfFilesOnDisk_WhenThoseFilesAreRetrieved_ThenTheFIlesAreReturned()
    //{
    //  _exportFileRepository.Entities.Count().Should().Be(10);
    //}

    //[Test]
    //public void GivenAnExistingFile_WhenAFileWithTheSameNameIsCreated_ThenAnExceptionIsThrown()
    //{
    //  var entry = new ExportFile() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) };

    //  _file = new FileInfo(GenerateFileName(entry));
    //  using (_file.Create()) ;
    //  Action act = () => _exportFileRepository.Create(entry);
    //  act.ShouldThrow<Exception>();
    //}

    //[Test]
    //public void GivenTheNameOfAFileThatDoesNotExist_WhenThatFileIsCreated_ThenTheFileExistsOnDisk()
    //{
    //  var entry = new ExportFile { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) };

    //  _exportFileRepository.Create(entry);
    //  _file = new FileInfo(GenerateFileName(entry));
    //  _file.Exists.Should().BeTrue();
    //}

    //[Test]
    //public void GivenTheNameOfAFileThatDoesNotExist_WhenThatFileIsCreated_ThenTheFileDataIsCorrect()
    //{
    //  var entry = new ExportFile { FileName = "filename", Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) };

    //  _exportFileRepository.Create(entry);
    //  _file = new FileInfo(GenerateFileName(entry));
    //  using (var stream = new StreamReader(_file.Open(FileMode.Open)))
    //  {
    //    var json = stream.ReadToEnd();
    //    entry = JsonSerializer.Deserialize<ExportFile>(json);
    //    entry.FileName.Should().Be("filename");
    //  }
    //}

    //[Test]
    //public void GivenTheNameOfAFileThatExists_WhenThatFileIsUpdated_ThenTheFileDataIsCorrect()
    //{
    //  var entry = new ExportFile { FileName = "old filename", Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) };

    //  _exportFileRepository.Create(entry);
    //  entry.FileName = "new filename";

    //  _exportFileRepository.Update(entry);
    //  _file = new FileInfo(GenerateFileName(entry));
    //  using (var stream = new StreamReader(_file.Open(FileMode.Open)))
    //  {
    //    var json = stream.ReadToEnd();
    //    entry = JsonSerializer.Deserialize<ExportFile>(json);
    //    entry.FileName.Should().Be("new filename");
    //  }
    //}

    //[Test]
    //public void GivenAnExistingFile_WhenTheFileIsDeleted_ThenTheFileIsRemovedFromDisk()
    //{
    //  var entry = new ExportFile { FileName = "filename", Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) };

    //  _file = new FileInfo(GenerateFileName(entry));
    //  using (_file.Create())
    //  {
    //  }

    //  _exportFileRepository.Delete(entry);

    //  _file = new FileInfo(GenerateFileName(entry));

    //  _file.Exists.Should().BeFalse();
    //}

    private string GenerateFileName(ExportFile entity)
    {
      return string.Format("{0}/{1}.json", ExportFilesDirectory, entity.Id);
    }
  }
}
