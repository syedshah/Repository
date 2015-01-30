namespace WcfClientTests
{
  using System;
  using System.Transactions;
  using AbstractConfigurationManager;
  using IntegrationTestUtils;
  using NUnit.Framework;

  [TestFixture]
  public class FileDataRepositoryTests : IoCSupportedTest
  {
    private TransactionScope _transactionScope;
    private IConfigurationManager _configurationManager;
    private IFileServiceFactory _fileserviceFactory;
    private IFileDataRepository _fileDataRepository;

    [SetUp]
    public void SetUp()
    {
      _transactionScope = new TransactionScope();
      _fileserviceFactory = Resolve<IFileServiceFactory>();
      _configurationManager = Resolve<IConfigurationManager>();

      _fileDataRepository = new FileDataRepository(_fileserviceFactory, _configurationManager);
    }

    [TearDown]
    public void TearDown()
    {
      _configurationManager = null;
      _fileserviceFactory = null;
      _transactionScope.Dispose();
    }

    [Test]
    public void GivenAValidRequestDate_WhenICallTheOneStepWcfService_IGetFileStatusData()
    {
      var data = _fileDataRepository.GetFileData(DateTime.Now.AddYears(-1));
      data.Should().NotBeEmpty();
    }

    [Test]
    public void GivenAValidRequestDate_WhenICallTheOneStepWcfService_IGetFileStatusDataWithOnlyXmlFiles()
    {
      var data = _fileDataRepository.GetFileData(DateTime.Now.AddYears(-1));
      data.Should().OnlyContain(f => f.FileName.Contains(".xml"));
    }
  }
}
