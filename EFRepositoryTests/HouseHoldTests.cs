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
  public class HouseHoldTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _houseHoldRepository = new HouseHoldRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _domicile = BuildMeA.Domicile("code", "description");
      _docType = BuildMeA.DocType("code", "description");
      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType);
      _manCo1 = BuildMeA.ManCo("description1", "code1").WithDomicile(_domicile);

      _document1 = BuildMeA.Document("id").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1);
      _houseHold = BuildMeA.HouseHold(DateTime.Now).WithDocument(_document1);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private HouseHoldRepository _houseHoldRepository;
    private Document _document1;
    private HouseHold _houseHold;
    private DocType _docType;
    private SubDocType _subDocType1;
    private ManCo _manCo1;
    private Domicile _domicile;

    [Test]
    public void GivenAHouseHold_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _houseHoldRepository.Entities.Count();
      _houseHoldRepository.Create(_houseHold);

      _houseHoldRepository.Entities.Count().Should().Be(initialCount + 1);
    }
  }
}
