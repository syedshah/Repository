namespace ArchiveWcfTests
{
  using System;
  using AbstractConfigurationManager;
  using ArchiveServiceFactory.Factories;
  using ArchiveServiceFactory.Interfaces;
  using Exceptions;
  using FluentAssertions;
  using IntegrationTestUtils;
  using NUnit.Framework;
  using TransactionScope = System.Transactions.TransactionScope;

  [TestFixture]
  public class ArchiveServiceFactoryTests : IoCSupportedTest
  {
    private TransactionScope _transactionScope;
    private IConfigurationManager _configurationManager;
    private IDocumentServiceFactory _documentServiceFactory;

    [SetUp]
    public void SetUp()
    {
      _transactionScope = new TransactionScope();
      _configurationManager = Resolve<IConfigurationManager>();
      _documentServiceFactory = new DocumentServiceFactory(_configurationManager);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    /*
    [Test]
    public void GivenADocumentServiceFactory_WhenITryToCreateAChannel_TheChannelIsCreated()
    {
      Action act = () => _documentServiceFactory.CreateChannel();

      act.ShouldNotThrow<UnityException>();
    }*/
  }
}
