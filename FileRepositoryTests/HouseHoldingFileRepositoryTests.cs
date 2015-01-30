namespace FileRepositoryTests
{
  using System;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading;
  using SystemFileAdapter;
  using FileRepository.Repositories;
  using FileSystemInterfaces;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class HouseHoldingFileRepositoryTests
  {
    IFileInfoFactory _fileInfoFactory;
    private IDirectoryInfo _directoryInfo;
    HouseHoldingFileRepository _houseHoldingFileRepository;
    const string BaseDirectory = "files";

    private string _grid = Guid.NewGuid().ToString();
    private DateTime _processingStart = DateTime.Now.AddMinutes(-100);
    private DateTime _processingEnd = DateTime.Now.AddMinutes(-90);

    private string _processingGrid1 = Guid.NewGuid().ToString();
    private string _document1 = Guid.NewGuid().ToString();
    private DateTime _document1HHDate = DateTime.Now.AddMinutes(-93);

    private string _processingGrid2 = Guid.NewGuid().ToString();
    private string _document2 = Guid.NewGuid().ToString();
    private DateTime _document2HHDate = DateTime.Now.AddMinutes(-96);
    
    private string _document3 = Guid.NewGuid().ToString();
    private DateTime _document3HHDate = DateTime.Now.AddMinutes(-96);

    [SetUp]
    public void Setup()
    {
      _fileInfoFactory = new SystemFileInfoFactory();
      _directoryInfo = new SystemIoDirectoryInfo();

      var di = new DirectoryInfo(BaseDirectory);
      if (di.Exists)
      {
        di.Delete(true);
      }
      di.Create();
      _houseHoldingFileRepository = new HouseHoldingFileRepository(BaseDirectory, _fileInfoFactory, _directoryInfo);
      CreateFiles(BaseDirectory);
    }

    [TearDown]
    public void TearDown()
    {
      var di = new DirectoryInfo(BaseDirectory);
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
      var CSVBuilder = new StringBuilder();

      CSVBuilder.Append(_grid + ",");
      CSVBuilder.Append(_processingStart + ",");
      CSVBuilder.Append(_processingEnd + "\n");
      
      CSVBuilder.Append(_processingGrid1 + ",");
      CSVBuilder.Append(_document1 + ",");
      CSVBuilder.Append(_document1HHDate.ToString() + "\n");

      CSVBuilder.Append(_processingGrid2 + ",");
      CSVBuilder.Append(_document2 + ",");
      CSVBuilder.Append(_document2HHDate.ToString() + "\n");

      CSVBuilder.Append(_processingGrid2 + ",");
      CSVBuilder.Append(_document3 + ",");
      CSVBuilder.Append(_document3HHDate.ToString());

      using (var streamWriter = new StreamWriter(File.Create(Path.Combine(directory, Guid.NewGuid().ToString() + ".csv"))))
      {
        streamWriter.WriteLine(CSVBuilder.ToString());
      }
    }

    [Test]
    public void GivenACorrectCSVFile_WhenITryToReadTheDataInTheFile_ThenTheFileDataIsCorrect()
    {
      var houseHoldingRun = _houseHoldingFileRepository.GetHouseHoldingData();

      houseHoldingRun.Grid.Should().Be(_grid);

      houseHoldingRun.ProcessingGridRun.Should()
                     .HaveCount(2)
                     .And.Contain(_processingGrid1)
                     .And.Contain(_processingGrid2);

      var doc1 = houseHoldingRun.DocumentRunData.Single(d => d.DocumentId == _document1);
      var doc2 = houseHoldingRun.DocumentRunData.Single(d => d.DocumentId == _document2);           

      doc1.HouseHoldDate.ToString().Should().Be(_document1HHDate.ToString());
      doc2.HouseHoldDate.ToString().Should().Be(_document2HHDate.ToString());
    }
  }
}
