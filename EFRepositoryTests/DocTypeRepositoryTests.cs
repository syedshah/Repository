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
  public class DocTypeRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _docTypeRepository = new DocTypeRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _docType1 = BuildMeA.DocType("code", "description");
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private DocTypeRepository _docTypeRepository;
    private DocType _docType1;

    [Test]
    public void GivenADocType_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _docTypeRepository.Entities.Count();
      _docTypeRepository.Create(_docType1);

      _docTypeRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenAnDocType_WhenITryToSearchForItById_ItIsRetrievedFromTheDatabase()
    {
      _docTypeRepository.Create(_docType1);
      var result = _docTypeRepository.GetDocType(_docType1.Id);

      result.Should().NotBeNull();
      result.Code.Should().Be("code");
    }

    [Test]
    public void GivenAnDocType_WhenITryToSearchForItByCode_ItIsRetrievedFromTheDatabase()
    {
      _docTypeRepository.Create(_docType1);
      var result = _docTypeRepository.GetDocType(_docType1.Code);

      result.Should().NotBeNull();
      result.Code.Should().Be("code");
    }

    [Test]
    public void WhenIUpdateADocType_AndTheDocTypeDoesNotExist_ThenAnExceptionIsThrown()
    {
      _docTypeRepository.Create(_docType1);

      DocType docType = _docTypeRepository.Entities.Where(p => p.Code == "code").FirstOrDefault();
      Assert.Throws<UnityException>(() => _docTypeRepository.Update(docType.Id + 1001, "code 1", "description name 1"));
    }

    [Test]
    public void WhenIUpdateADocType_ThenTheDocTypeIsUpdated()
    {
      _docTypeRepository.Create(_docType1);

      DocType docType = _docTypeRepository.Entities.Where(p => p.Code == "code").FirstOrDefault();
      _docTypeRepository.Update(docType.Id, "new code", "description 1");
      docType = _docTypeRepository.Entities.Where(p => p.Code == "new code").FirstOrDefault();

      docType.Should().NotBeNull();
    }

    [Test]
    public void GivenThereIsADocType_WhenIAddASubDocType_TheSubDocTypeIsAdded()
    {
      _docTypeRepository.Create(_docType1);

      DocType docType = _docTypeRepository.AddSubDocType(_docType1.Id, "code", "description");

      docType.SubDocTypes.Count.Should().Be(1);
    }

    [Test]
    public void GivenThereIsNoDocType_WhenIAddASubDocType_ThenAnUnityExceptionIsThrown()
    {
      Assert.Throws<UnityException>(() => _docTypeRepository.AddSubDocType(2222, "code", "description"));
    }
  }
}
