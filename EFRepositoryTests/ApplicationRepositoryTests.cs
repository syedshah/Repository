namespace EFRepositoryTests
{
  using System.Configuration;
  using System.Linq;
  using System.Transactions;
  using Builder;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class ApplicationRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _applicationRepository = new ApplicationRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _application1 = BuildMeA.Application("code", "description");
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private ApplicationRepository _applicationRepository;
    private Application _application1;

    [Test]
    public void GivenAnApplication_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _applicationRepository.Entities.Count();
      _applicationRepository.Create(_application1);

      _applicationRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenAnApplication_WhenITryToSearchForIt_ItIsRetrievedFromTheDatabase()
    {
      _applicationRepository.Create(_application1);
      var result = _applicationRepository.GetApplication("code");

      result.Should().NotBeNull();
      result.Code.Should().Be("code");
    }

    [Test]
    public void GivenAnApplication_WhenITryToSearchForItById_ItIsRetrievedFromTheDatabase()
    {
      _applicationRepository.Create(_application1);
      var result = _applicationRepository.GetApplication(_application1.Id);

      result.Should().NotBeNull();
      result.Code.Should().Be("code");
    }

    [Test]
    public void GivenThereIsAnApplicaiton_WhenIAddAnIndex_TheIndexIsAdded()
    {
      _applicationRepository.Create(_application1);

      Application application = _applicationRepository.AddIndex(_application1.Id, "IndexName", "IndexArchiveName", "IndexArchiveValue");
      
      application.IndexDefinitions.Count.Should().Be(1);
    }

    [Test]
    public void GivenThereIsNoApplication_WhenIAddAnIndex_ThenAnUnityExceptionIsThrown()
    {
      Assert.Throws<UnityException>(() => _applicationRepository.AddIndex(2222, "IndexName", "IndexArchiveName", "IndexValue"));
    }
  }
}
