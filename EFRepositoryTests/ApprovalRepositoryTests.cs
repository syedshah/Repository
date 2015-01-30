namespace EFRepositoryTests
{
  using System;
  using System.Configuration;
  using System.Linq;
  using System.Transactions;
  using Builder;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class ApprovalRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _approvalRepository = new ApprovalRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _domicile = BuildMeA.Domicile("code", "description");
      _docType = BuildMeA.DocType("code", "description");
      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType);
      _manCo1 = BuildMeA.ManCo("description1", "code1").WithDomicile(_domicile);

      _document1 = BuildMeA.Document("id").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1);
      _approval = BuildMeA.Approval("name", DateTime.Now).WithDocument(_document1);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private ApprovalRepository _approvalRepository;
    private Document _document1;
    private Approval _approval;
    private DocType _docType;
    private SubDocType _subDocType1;
    private ManCo _manCo1;
    private Domicile _domicile;

    [Test]
    public void GivenAnApproval_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _approvalRepository.Entities.Count();
      _approvalRepository.Create(_approval);
      
      _approvalRepository.Entities.Count().Should().Be(initialCount + 1);
    }
  }
}
