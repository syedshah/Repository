namespace EFRepositoryTests
{
  using System;
  using System.Collections.Generic;
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
  public class HouseHoldingRunTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _houseHoldingRunRepository = new HouseHoldingRunRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _domicile = BuildMeA.Domicile("code", "description");
      _docType = BuildMeA.DocType("code", "description");
      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType);
      _manCo1 = BuildMeA.ManCo("description1", "code1").WithDomicile(_domicile);
      _manCo2 = BuildMeA.ManCo("description2", "code2").WithDomicile(_domicile);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private HouseHoldingRunRepository _houseHoldingRunRepository;
    private DocType _docType;
    private SubDocType _subDocType1;
    private ManCo _manCo1;
    private ManCo _manCo2;
    private Domicile _domicile;

    [Test]
    public void GivenAHouseHoldingRun_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _houseHoldingRunRepository.Entities.Count();
      HouseHoldingRun houseHoldingRun = BuildMeA.HouseHoldingRun(DateTime.Now, DateTime.Now, "grid");
      _houseHoldingRunRepository.Create(houseHoldingRun);

      _houseHoldingRunRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenHouseHoldingRuns_WhenIAskForTheFifteenMostRecentHouseHoldingRuns_IGetTheFifteenMostRecentHouseHeldGrids()
    {
      for (int i = 1; i < 19; i++)
      {
        if (i == 5)
        {
          Document document1 = BuildMeA.Document("id").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1);

          HouseHoldingRun houseHoldingRunDifferentManCo =
            BuildMeA.HouseHoldingRun(DateTime.Now, DateTime.Now, "grid " + i)
            .WithDocument(document1);

          _houseHoldingRunRepository.Create(houseHoldingRunDifferentManCo);
        }
        else
        {
          Document document2 = BuildMeA.Document("id").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo2);

          HouseHoldingRun houseHoldingRun =
            BuildMeA.HouseHoldingRun(DateTime.Now, DateTime.Now, "grid " + i)
            .WithDocument(document2);

          _houseHoldingRunRepository.Create(houseHoldingRun);
        }
      }

      var listManCoId = new List<int> { _manCo2.Id };

      var recentlyHouseHeld = _houseHoldingRunRepository.GetTopFifteenRecentHouseHeldGrids(listManCoId);

      recentlyHouseHeld.Should().HaveCount(15)
                             .And.NotContain(g => g.Grid == "grid 1")
                             .And.NotContain(g => g.Grid == "grid 2")
                             .And.Contain(g => g.Grid == "grid 3")
                             .And.Contain(g => g.Grid == "grid 4")
                             .And.NotContain(g => g.Grid == "grid 5")
                             .And.Contain(g => g.Grid == "grid 6")
                             .And.Contain(g => g.Grid == "grid 7")
                             .And.Contain(g => g.Grid == "grid 8")
                             .And.Contain(g => g.Grid == "grid 9")
                             .And.Contain(g => g.Grid == "grid 10")
                             .And.Contain(g => g.Grid == "grid 11")
                             .And.Contain(g => g.Grid == "grid 12")
                             .And.Contain(g => g.Grid == "grid 13");
    }
  }
}
