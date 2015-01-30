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
  public class SubDocTypeRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _subDocTypeRepository = new SubDocTypeRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _docTypeRepository = new DocTypeRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _docType1 = BuildMeA.DocType("code 5", "description 5");
      _docType2 = BuildMeA.DocType("code 6", "description 6");
      
      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType1);
      _subDocType2 = BuildMeA.SubDocType("code 2", "description 3").WithDocType(_docType1);
      _subDocType3 = BuildMeA.SubDocType("code 3", "description 3").WithDocType(_docType1);
      _subDocType4 = BuildMeA.SubDocType("code 4", "description 4").WithDocType(_docType2);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private SubDocTypeRepository _subDocTypeRepository;
    private DocTypeRepository _docTypeRepository;
    private SubDocType _subDocType1;
    private SubDocType _subDocType2;
    private SubDocType _subDocType3;
    private SubDocType _subDocType4;
    private DocType _docType1;
    private DocType _docType2;

    [Test]
    public void GivenASubDocType_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _subDocTypeRepository.Entities.Count();
      _subDocTypeRepository.Create(_subDocType1);

      _subDocTypeRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenASubDocType_WhenITryToSearchForItById_ItIsRetrievedFromTheDatabase()
    {
      _subDocTypeRepository.Create(_subDocType1);
      var result = _subDocTypeRepository.GetSubDocType(_subDocType1.Id);

      result.Should().NotBeNull();
      result.Code.Should().Be("code 1");
    }

    [Test]
    public void GivenADocTypeId_WhenITryToSearchSubDocTypesforAGivenDocTypeId_SubDocTypesAreRetrievedFromTheDatabase()
    {
      _docTypeRepository.Create(_docType1);
      _docTypeRepository.Create(_docType2);

      _subDocTypeRepository.Create(_subDocType1);
      _subDocTypeRepository.Create(_subDocType2);
      _subDocTypeRepository.Create(_subDocType3);
      _subDocTypeRepository.Create(_subDocType4);

      var result = _subDocTypeRepository.GetSubDocTypes(_docType1.Id);

      result.Should().NotBeNull()
        .And.HaveCount(3);
    }

    [Test]
    public void WhenIUpdateASubDocType_AndTheSubDocTypeDoesNotExist_ThenAnExceptionIsThrown()
    {
      _subDocTypeRepository.Create(_subDocType1);

      SubDocType docType = _subDocTypeRepository.Entities.Where(p => p.Code == "code 1").FirstOrDefault();
      Assert.Throws<UnityException>(() => _subDocTypeRepository.Update(docType.Id + 1001, "code 1", "description name 1"));
    }

    [Test]
    public void WhenIUpdateASubDocType_ThenTheSubDocTypeIsUpdated()
    {
      _subDocTypeRepository.Create(_subDocType1);

      SubDocType subDocType = _subDocTypeRepository.Entities.Where(p => p.Code == "code 1").FirstOrDefault();
      _subDocTypeRepository.Update(subDocType.Id, "new code", "description 1");
      subDocType = _subDocTypeRepository.Entities.Where(p => p.Code == "new code").FirstOrDefault();

      subDocType.Should().NotBeNull();
    }

    [Test]
    public void GivenASubDocTypeCode_WhenITryToSearchSubDocTypesforAGivenSubDocTypeCode_ASubDocTypeIsRetrievedFromTheDatabase()
    {
        _docTypeRepository.Create(_docType1);
        _docTypeRepository.Create(_docType2);

        _subDocTypeRepository.Create(_subDocType1);
        _subDocTypeRepository.Create(_subDocType2);
        _subDocTypeRepository.Create(_subDocType3);
        _subDocTypeRepository.Create(_subDocType4);

        var result = _subDocTypeRepository.GetSubDocType(_subDocType2.Code);

        result.Should().NotBeNull();
        result.Code.Should().Be("code 2");
        result.DocType.Code.Should().Be("code 5");

    }
  }
}
