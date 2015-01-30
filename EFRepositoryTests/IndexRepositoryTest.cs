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
  public class IndexRepositoryTest
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();

      _application1 = BuildMeA.Application("code1", "description1");
      _application2 = BuildMeA.Application("code2", "description2");

      _index1 = BuildMeA.Index("name", "indexColumn")
        .WithApplication(_application1);

      _index2 = BuildMeA.Index("name2", "indexColumn2")
        .WithApplication(_application2);

      _indexRepository = new IndexRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private IndexRepository _indexRepository;
    private IndexDefinition _index1;
    private IndexDefinition _index2;
    private Application _application1;
    private Application _application2;

    [Test]
    public void WhenIGetASpecificIndexPost_ThenIGetTheCorrectIndex()
    {
      _indexRepository.Create(_index1);
      IndexDefinition newIndex = _indexRepository.GetIndex(_index1.Id);
      newIndex.Should().NotBeNull();
      newIndex.Id.Should().Be(_index1.Id);
    }

    [Test]
    public void WhenIUpdateAnIndex_AndTheIndexDoesNotExist_ThenAnExceptionIsThrown()
    {
      _indexRepository.Create(_index1);

      IndexDefinition index = _indexRepository.Entities.Where(p => p.Name == "name").FirstOrDefault();
      Assert.Throws<UnityException>(
        () => _indexRepository.Update(index.Id + 1001, "name 1", "archive name 1", "archive col 1"));
    }

    [Test]
    public void WhenIUpdateAnIndex_ThenIIndexIsUpdated()
    {
      _indexRepository.Create(_index1);

      IndexDefinition index = _indexRepository.Entities.Where(p => p.Name == "name").FirstOrDefault();
      _indexRepository.Update(index.Id, "new name", "archive name 1", "archive col 1");
      index = _indexRepository.Entities.Where(p => p.Name == "new name").FirstOrDefault();

      index.Should().NotBeNull();
    }

    [Test]
    public void WhenIGetASpecificApplicationId_ThenIGetTheCorrectIndex()
    {
      _indexRepository.Create(_index1);
      _indexRepository.Create(_index2);

      IndexDefinition newIndex = _indexRepository.GetIndexByApplicationId(_index1.ApplicationId);
      newIndex.Should().NotBeNull();
      newIndex.ApplicationId.Should().Be(_index1.ApplicationId);
      newIndex.Id.Should().NotBe(_index2.Id);
    }
  }
}
