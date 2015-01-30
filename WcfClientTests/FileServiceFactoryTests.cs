namespace WcfClientTests
{
  using System;
  using System.Transactions;
  using AbstractConfigurationManager;
  using Exceptions;
  using FluentAssertions;
  using IntegrationTestUtils;
  using NUnit.Framework;
  using ServiceFactory.Factories;
  using ServiceFactory.Interfaces;

  [TestFixture]
  public class FileServiceFactoryTests : IoCSupportedTest
  {
    private TransactionScope _transactionScope;
    private IConfigurationManager _configurationManager;
    private IFileServiceFactory _fileServiceFactory;

    [SetUp]
    public void SetUp()
    {
      _transactionScope = new TransactionScope();
      _configurationManager = Resolve<IConfigurationManager>();
      _fileServiceFactory = new FileServiceFactory(_configurationManager);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    [Test]
    public void GivenAFileServiceFactory_WhenITryToCreateAChannel_TheChannelIsCreated()
    {
      Action act = () => _fileServiceFactory.CreateChannel();

      act.ShouldNotThrow<UnityException>();
    }
  }
}
