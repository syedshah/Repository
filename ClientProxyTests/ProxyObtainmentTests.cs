namespace ClientProxyTests
{
  using ClientProxies;
  using ClientProxies.ArchiveServiceReference;
  using ClientProxies.OneStepServiceReference;

  using Common.Contracts;
  using FluentAssertions;
  using IntegrationTestUtils;
  using NUnit.Framework;

  [TestFixture]
  public class ProxyObtainmentTests : IoCSupportedTest
  {
    [SetUp]
    public void SetUp()
    {
    }

    [Test]
    public void GivenAServiceContract_WhenITryToGetTheDocumentClientProxy_IGetTheCdocumentClientProxy()
    {
      IDocument proxy = Resolve<IDocument>();

      proxy.Should().BeOfType<DocumentClient>();
    }

    [Test]
    public void GivenAServiceFactory_WhenITryToGetDocumentProxy_IGetTheDocumentProxy()
    {
      IServiceFactory factory = new ServiceFactory();
      IDocument proxy = factory.CreateClient<IDocument>();

      proxy.Should().BeOfType<DocumentClient>();
    }

    [Test]
    public void GivenAContainter_WhenITryToGetADocumentProxyFactory_IGetTheDocumentFactory()
    {
      IServiceFactory factory = this.Resolve<IServiceFactory>();

      IDocument proxy = factory.CreateClient<IDocument>();

      proxy.Should().BeOfType<DocumentClient>();
    }

    [Test]
    public void GivenAServiceContract_WhenITryToGetTheFileServiceClientProxy_IGetTheFileSerivceClientProxy()
    {
      IFileService proxy = Resolve<IFileService>();

      proxy.Should().BeOfType<FileServiceClient>();
    }

    [Test]
    public void GivenAServiceFactory_WhenITryToGetFileServiceProxy_IGetTheFileServiceProxy()
    {
      IServiceFactory factory = new ServiceFactory();
      IFileService proxy = factory.CreateClient<IFileService>();

      proxy.Should().BeOfType<FileServiceClient>();
    }

    [Test]
    public void GivenAContainter_WhenITryToGetAFileServiceFactory_IGetTheFileServiceFactory()
    {
      IServiceFactory factory = this.Resolve<IServiceFactory>();

      IFileService proxy = factory.CreateClient<IFileService>();

      proxy.Should().BeOfType<FileServiceClient>();
    }
  }
}
