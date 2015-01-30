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
  public class CheckOutRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _checkOutRepository = new CheckOutRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _domicile = BuildMeA.Domicile("code", "description");
      _manCo1 = BuildMeA.ManCo("description1", "code1").WithDomicile(_domicile);
      _docType = BuildMeA.DocType("code", "description");
      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType);
      _document1 = BuildMeA.Document("id").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1);
      _checkOut = BuildMeA.CheckOut("name", DateTime.Now.AddHours(-2)).WithDocument(_document1);

      _document2 = BuildMeA.Document("id2").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1);
      _checkOut2 = BuildMeA.CheckOut(null, null).WithDocument(_document1);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private CheckOutRepository _checkOutRepository;
    private Document _document1;
    private Document _document2;
    private DocType _docType;
    private SubDocType _subDocType1;
    private ManCo _manCo1;
    private Domicile _domicile;
    private CheckOut _checkOut;
    private CheckOut _checkOut2;

    [Test]
    public void GivenACheckOut_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _checkOutRepository.Entities.Count();
      _checkOutRepository.Create(_checkOut);
      
      _checkOutRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenADocumentIdForADocumentThatIsNotCheckedOut_WhenIAskToSeeIfItIsCheckedOut_IGetTheCheckOut()
    {
      _checkOutRepository.Create(_checkOut);

      var checkOut = _checkOutRepository.GetCheckOut("id");

      checkOut.Should().NotBeNull();
    }

    [Test]
    public void GivenADocumentIdForADocumentThatIsCheckedOut_WhenIAskToSeeIfItIsCheckedOut_IGetNoCheckOut()
    {
      _checkOutRepository.Create(_checkOut);

      var checkOut = _checkOutRepository.GetCheckOut("id2");

      checkOut.Should().BeNull();
    }

    [Test]
    public void WhenIDeleteACheckOutItem_ThenItIsDeletedFromTheDatabase()
    {
      _checkOutRepository.Create(_checkOut);
      CheckOut retrievedCheckOut = _checkOutRepository.GetCheckOut(_checkOut.Document.DocumentId);
      _checkOutRepository.Delete(retrievedCheckOut);
      CheckOut deletedCheckOut = _checkOutRepository.Entities.FirstOrDefault(c => c.DocumentId == retrievedCheckOut.DocumentId);
      deletedCheckOut.Should().BeNull();
    }
  }
}
