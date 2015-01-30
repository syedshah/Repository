namespace BusinessEngineTests
{
  using System;
  using BusinessEngineInterfaces;
  using BusinessEngines;
  using ClientProxies.OneStepServiceReference;
  using Entities;
  using Entities.File;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  [TestFixture]
  public class GridRunBusinessEngineTests
  {
    private Mock<IGridRunService> _gridRunService;
    private Mock<IXmlFileService> _xmlFileService;
    private Mock<IApplicationService> _applicationService;
    private Mock<ILogger> _logger;
    private IGridRunEngine _gridRunEngine;

    [SetUp]
    public void SetUp()
    {
      _gridRunService = new Mock<IGridRunService>();
      _xmlFileService = new Mock<IXmlFileService>();
      _applicationService = new Mock<IApplicationService>();
      _logger = new Mock<ILogger>();
      _gridRunEngine = new GridRunEngine(_gridRunService.Object, _xmlFileService.Object, _applicationService.Object, _logger.Object);
    }

    [Test]
    public void GivenAValidGridAndNoFileName_AndTheGridRunIsAlreadyInTheDatabase_TheGridRunIsUpdatedInTheDatabase()
    {
      _gridRunService.Setup(f => f.GetGridRun(It.IsAny<string>(), It.IsAny<string>())).Returns(new GridRun());

      _gridRunEngine.ProcessGridRun(
        string.Empty,
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        null,
        null,
        GridRunStatus.Undefined, 
        It.IsAny<bool>());

      _gridRunService.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
    }

    [Test]
    public void GivenAValidGridAndFileName_AndTheGridRunIsAlreadyInTheDatabase_TheGridRunIsUpdatedInTheDatabase()
    {
      _gridRunService.Setup(f => f.GetGridRun("fileName", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(new GridRun());
      _xmlFileService.Setup(x => x.GetFile(It.IsAny<string>())).Returns(new XmlFile());

      _gridRunEngine.ProcessGridRun(
        "fileName",
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<DateTime?>(),
        It.IsAny<DateTime?>(),
        It.IsAny<GridRunStatus>(),
        It.IsAny<bool>());

      _gridRunService.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
    }

    [Test]
    public void GivenAValidGridRun_AndTheGridRunINotInTheDatabase_AndTheApplicationIsNew_TheApplicaitonIsAddedToTheDatabase()
    {
      GridRun gridRun = null;
      Application application = null;

      _gridRunService.Setup(f => f.GetGridRun(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(gridRun);
      _applicationService.Setup(a => a.GetApplication(It.IsAny<string>())).Returns(application);
      _xmlFileService.Setup(x => x.GetFile(It.IsAny<string>())).Returns(new XmlFile());
      _applicationService.Setup(a => a.CreateApplication(It.IsAny<string>(), It.IsAny<string>())).Returns(new Application());

      _gridRunEngine.ProcessGridRun(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<DateTime?>(),
        It.IsAny<DateTime?>(),
        It.IsAny<GridRunStatus>(),
        It.IsAny<bool>());
      
      _applicationService.Verify(x => x.CreateApplication(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
      _applicationService.Verify(x => x.GetApplication(It.IsAny<string>()));

      _gridRunService.Verify(g => g.Create(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<bool>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>()),
          Times.Once());
    }

    [Test]
    public void GivenAValidGrid_AndTheGridRunINotInTheDatabase_AndTheFileIsNotInTheDatabse_ThenAUnityLogWarningIsThrown()
    {
      GridRun gridRun = null;
      XmlFile xmlFile = null;

      _gridRunService.Setup(f => f.GetGridRun(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(gridRun);
      _applicationService.Setup(a => a.GetApplication(It.IsAny<string>())).Returns(new Application());
      _xmlFileService.Setup(x => x.GetFile(It.IsAny<string>())).Returns(xmlFile);

      _gridRunEngine.ProcessGridRun(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<DateTime?>(),
        It.IsAny<DateTime?>(),
        It.IsAny<GridRunStatus>(),
        It.IsAny<bool>());

      _logger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once());

      _gridRunService.Verify(g => g.Create(
          It.IsAny<int>(),
          null,
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<bool>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>()));
    }

    [Test]
    public void GivenAValidGrid_AndTheGridRunINotInTheDatabase_AndTheFileIInTheDatabse_ThenANewGridRunIsAddedToTheDatabase()
    {
      GridRun gridRun = null;

      _gridRunService.Setup(f => f.GetGridRun(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(gridRun);
      _applicationService.Setup(a => a.GetApplication(It.IsAny<string>())).Returns(new Application());
      _xmlFileService.Setup(x => x.GetFile(It.IsAny<string>())).Returns(new XmlFile());

      _gridRunEngine.ProcessGridRun(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<DateTime?>(),
        It.IsAny<DateTime?>(),
        It.IsAny<GridRunStatus>(),
        It.IsAny<bool>());

      _gridRunService.Verify(g => g.Create(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<bool>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>()));
    }
  }
}
