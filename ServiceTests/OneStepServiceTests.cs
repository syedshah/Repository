namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using AbstractConfigurationManager;
  using ClientProxies.OneStepServiceReference;

  using Exceptions;

  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  using Services;

  [TestFixture]
  public class OneStepServiceTests
  {
    [SetUp]
    public void SetUp()
    {
      _configurationManager = new Mock<IConfigurationManager>();
      _fileService = new Mock<IFileService>();
      _oneStepService = new OneStepService(_fileService.Object, _configurationManager.Object);
    }

    private Mock<IConfigurationManager> _configurationManager;
    private Mock<IFileService> _fileService;
    private IOneStepService _oneStepService;

    private IEnumerable<FileStatusData> _data = new BindingList<FileStatusData>()
                                                  {
                                                    new FileStatusData { StartDate = DateTime.Now, EndDate = DateTime.Now, FileName = "1.xml", Grid = "grid" },
                                                    new FileStatusData { StartDate = DateTime.Now, EndDate = DateTime.Now, FileName = "1.zip", Grid = "grid" },
                                                    new FileStatusData { StartDate = DateTime.Now, EndDate = DateTime.Now, FileName = "2.xml", Grid = "grid" }
                                                  };
      
    [Test]
    public void GivenAValidSyncDate_WhenITryToGetRunDataFromOneStep_IGetFileStatusData()
    {
      _fileService.Setup(f => f.GetFileStatusInfo(It.IsAny<DateTime>(), It.IsAny<string>())).Returns(_data.ToList());

      _configurationManager.Setup(c => c.AppSetting(It.IsAny<string>())).Returns("NT");

      var data = _oneStepService.GetFileData(It.IsAny<DateTime>());
      data.Should().NotBeNull();
    }

    [Test]
    public void GivenAValidSyncDate_WhenITryToGetRunDataFromOneStep_AndTheOneStepServiceIsDown_IGetFileStatusData()
    {
      _fileService.Setup(f => f.GetFileStatusInfo(It.IsAny<DateTime>(), It.IsAny<string>())).Throws<Exception>();

      _configurationManager.Setup(c => c.AppSetting(It.IsAny<string>())).Returns("NT");

      Action act = () => _oneStepService.GetFileData(It.IsAny<DateTime>());

      act.ShouldThrow<UnityException>();
    }
  }
}
