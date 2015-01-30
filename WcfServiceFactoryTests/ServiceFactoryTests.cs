namespace WcfServiceFactoryTests
{
  using AbstractConfigurationManager;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceFactory.Factories;
  using ServiceFactory.Interfaces;
  using ServiceFactory.OneStep;

  [TestFixture]
  public class ServiceFactoryTests
  {
    private Mock<IConfigurationManager> _configurationManager;
    private IFileServiceFactory _fileServiceFactory;

    [SetUp]
    public void SetUp()
    {
      _configurationManager = new Mock<IConfigurationManager>();
      _fileServiceFactory = new FileServiceFactory(_configurationManager.Object);
    }

    [Test]
    public void GivenAFileServiceFactory_WhenITryToCreateAChannel_TheChannelIsCreated()
    {
      _configurationManager.Setup(c => c.AppSetting(It.IsAny<string>())).Returns("http://beta.nexdox.com/onestepwcf/services/FileService.svc");
      IFileServiceChannel fileServiceChannel =_fileServiceFactory.CreateChannel();

      fileServiceChannel.Should().NotBeNull();
    }
  }
}
