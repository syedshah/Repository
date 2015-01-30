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
  public class AutoApprovalRepositoryTests
  {
    private TransactionScope _transactionScope;

    private AutoApprovalRepository _autoApprovalRepository;

    private DocTypeRepository _docTypeRepository;

    private SubDocTypeRepository _subDocTypeRepository;

    private ManCoRepository _manCoRepository;

    private ManCo _manCo1;

    private ManCo _manCo2;

    private AutoApproval _autoApproval;

    private AutoApproval _autoApproval2;

    private AutoApproval _autoApproval3;

    private DocType _docType;

    private SubDocType _subDocType1;

    private SubDocType _subDocType2;

    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _autoApprovalRepository =
        new AutoApprovalRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _docTypeRepository = new DocTypeRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _subDocTypeRepository = new SubDocTypeRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _manCoRepository = new ManCoRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _docType = BuildMeA.DocType("code", "description");
      _manCo1 = BuildMeA.ManCo("code1", "description1");
      _manCo2 = BuildMeA.ManCo("code2", "description2");
      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType);
      _subDocType2 = BuildMeA.SubDocType("code 2", "description 3").WithDocType(_docType);
      _autoApproval =
        BuildMeA.AutoApproval().WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1);
      _autoApproval2 =
        BuildMeA.AutoApproval().WithDocType(_docType).WithSubDocType(_subDocType2).WithManCo(_manCo2);
      _autoApproval3 =
        BuildMeA.AutoApproval().WithDocType(_docType).WithSubDocType(_subDocType2).WithManCo(_manCo1);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    [Test]
    public void WhenIDeleteAnAutoApproval_ThenItIsDeteled()
    {
      _autoApprovalRepository.Create(_autoApproval);
      AutoApproval retrievedAutoApproval = _autoApprovalRepository.GetAutoApproval(_autoApproval.Id);

      _autoApprovalRepository.Delete(_autoApproval);
      AutoApproval autoApproval = _autoApprovalRepository.GetAutoApproval(retrievedAutoApproval.Id);
      autoApproval.Should().BeNull();
    }

    [Test]
    public void GivenAnAutoApproval_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _autoApprovalRepository.Entities.Count();

      _manCoRepository.Create(_manCo1);
      _manCoRepository.Create(_manCo2);

      _docTypeRepository.Create(_docType);

      _subDocTypeRepository.Create(_subDocType1);
      _subDocTypeRepository.Create(_subDocType2);

      _autoApprovalRepository.Create(_autoApproval);
      _autoApprovalRepository.Create(_autoApproval2);

      _autoApprovalRepository.Entities.Count().Should().Be(initialCount + 2);
    }

    [Test]
    public void GivenAnAutoApproval_WhenITryToGetApprovalByMancoDoctypSubDocType_ItIsRetrievedFromTheDatabase()
    {
      _docTypeRepository.Create(_docType);

      _manCoRepository.Create(_manCo1);
      _manCoRepository.Create(_manCo2);

      _subDocTypeRepository.Create(_subDocType1);
      _subDocTypeRepository.Create(_subDocType2);

      _autoApprovalRepository.Create(_autoApproval);
      _autoApprovalRepository.Create(_autoApproval2);
      var result = _autoApprovalRepository.GetAutoApproval(_manCo1.Id, _docType.Id, _subDocType1.Id);

      result.Should().NotBeNull();
    }

    [Test]
    public void WhenITryToGetAllAutoApprovalsByManCoId_AllAutoApprovalsForTheManCoAreRetrievedFromTheDatabase()
    {
      _docTypeRepository.Create(_docType);

      _manCoRepository.Create(_manCo1);
      _manCoRepository.Create(_manCo2);

      _subDocTypeRepository.Create(_subDocType1);
      _subDocTypeRepository.Create(_subDocType2);

      _autoApprovalRepository.Create(_autoApproval);
      _autoApprovalRepository.Create(_autoApproval2);
      _autoApprovalRepository.Create(_autoApproval3);

      var result = _autoApprovalRepository.GetAutoApprovals(_manCo1.Id);

      result.Should().NotBeNull();
      result.Count.Should().BeGreaterOrEqualTo(2);
      result[1].Id.Should().NotBe(_autoApproval2.Id);
    }

    [Test]
    public void WhenITryToGetAllAutoApprovals_AllAutoApprovalsAreRetrievedFromTheDatabase()
    {
      _docTypeRepository.Create(_docType);

      _manCoRepository.Create(_manCo1);
      _manCoRepository.Create(_manCo2);

      _subDocTypeRepository.Create(_subDocType1);
      _subDocTypeRepository.Create(_subDocType2);

      _autoApprovalRepository.Create(_autoApproval);
      _autoApprovalRepository.Create(_autoApproval2);

      var result = _autoApprovalRepository.GetAutoApprovals();

      result.Should().NotBeNull();
      result.Count.Should().BeGreaterOrEqualTo(2);
    }

    [Test]
    public void GivenAnAutoApprovalId_WhenITryToGetAutoApprovalById_TheAutoApprovalIsRetrievedFromTheDatabase(
      )
    {
      _docTypeRepository.Create(_docType);

      _manCoRepository.Create(_manCo1);
      _manCoRepository.Create(_manCo2);

      _subDocTypeRepository.Create(_subDocType1);
      _subDocTypeRepository.Create(_subDocType2);

      _autoApprovalRepository.Create(_autoApproval);
      _autoApprovalRepository.Create(_autoApproval2);

      var result = _autoApprovalRepository.GetAutoApproval(_autoApproval.Id);

      result.Should().NotBeNull();
      result.Id.Should().Be(_autoApproval.Id);
      result.Id.Should().NotBe(_autoApproval2.Id);
    }

    [Test]
    public void GivenAWrongAutoApprovalId_WhenITryToGetAutoApprovalById_TheDocumentApprovalShouldbeNull()
    {
      _docTypeRepository.Create(_docType);

      _manCoRepository.Create(_manCo1);
      _manCoRepository.Create(_manCo2);

      _subDocTypeRepository.Create(_subDocType1);
      _subDocTypeRepository.Create(_subDocType2);

      _autoApprovalRepository.Create(_autoApproval);
      _autoApprovalRepository.Create(_autoApproval2);

      var result = _autoApprovalRepository.GetAutoApproval(_autoApproval2.Id + 1);

      result.Should().BeNull();
    }

    [Test]
    public void GivenAnAutoApproval_WhenITryToUpdateTheAutoApproval_TheAutoAprovalShouldBeUpdated()
    {
      _docTypeRepository.Create(_docType);

      _manCoRepository.Create(_manCo1);
      _manCoRepository.Create(_manCo2);

      _subDocTypeRepository.Create(_subDocType1);
      _subDocTypeRepository.Create(_subDocType2);

      _autoApprovalRepository.Create(_autoApproval);
      _autoApprovalRepository.Create(_autoApproval2);

      var mancoId = _autoApproval2.ManCoId;
      var subDocTypeId = _autoApproval2.SubDocTypeId;

      this._autoApprovalRepository.Update(_autoApproval2.Id, _manCo1.Id, _docType.Id, _subDocType1.Id);

      mancoId.Should().NotBe(_autoApproval2.ManCoId);
      subDocTypeId.Should().NotBe(_autoApproval2.SubDocTypeId);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAnInvalidAutoApproval_WhenITryToUpdateTheAutoApproval_AUnityExceptionShouldBeThrown()
    {
      _docTypeRepository.Create(_docType);

      _subDocTypeRepository.Create(_subDocType1);
      _subDocTypeRepository.Create(_subDocType2);

      _autoApprovalRepository.Create(_autoApproval);
      _autoApprovalRepository.Create(_autoApproval2);

      this._autoApprovalRepository.Update(_autoApproval2.Id + 1, 2, _docType.Id, _subDocType1.Id);
    }
  }
}
