namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using BusinessEngineInterfaces;
  using ClientProxies.OneStepServiceReference;
  using Entities;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;

  [TestFixture]
  public class SyncServiceTests
  {
    private Mock<IFileSyncService> _fileSyncService;
    private Mock<IOneStepService> _oneStepService;
    private Mock<IGridRunEngine> _gridRunEngine;
    private ISyncService _syncService;

    [SetUp]
    public void SetUp()
    {
      _fileSyncService = new Mock<IFileSyncService>();
      _oneStepService = new Mock<IOneStepService>();
      _gridRunEngine = new Mock<IGridRunEngine>();

      _syncService = new SyncService(_fileSyncService.Object, _oneStepService.Object, _gridRunEngine.Object);

      _oneStepService.Setup(o => o.GetFileData(It.IsAny<DateTime>())).Returns(_fileData);
      _fileSyncService.Setup(f => f.GetLatest()).Returns(new FileSync());
      _fileSyncService.Setup(u => u.UpdateFileSync(It.IsAny<int>()));
    }

    List<FileStatusData> _fileData = new List<FileStatusData> 
                                      { 
                                        new FileStatusData { GridRunId = 1 },
                                        new FileStatusData { GridRunId = 2 }
                                      };

    [Test]
    public void GivenAValidFileStatusData_AndTheDaabaseIaAvailable_TheFileStatusDataIsProcessedSuccessfully()
    {
      _gridRunEngine.Setup(
        f =>
        f.ProcessGridRun(
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<GridRunStatus>(),
          It.IsAny<bool>()));

      _syncService.Synchronise();

      _gridRunEngine.Verify(x => x.ProcessGridRun(It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<DateTime?>(),
          It.IsAny<DateTime?>(),
          It.IsAny<GridRunStatus>(),
          It.IsAny<bool>()), Times.Exactly(2));
    }
  }
}
