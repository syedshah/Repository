namespace EFRepositoryTests
{
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using System.Linq;
  using System.Transactions;
  using Builder;
  using Entities;
  using Entities.File;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  public class GridRunRepositoryTest
  {
    [Category("Integration")]
    [SetUp]
    public void SetUp()
    {
      _transactionScope = new TransactionScope();
      _gridRunRepository = new GridRunRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _xmlFileRepository = new XmlFileRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _houseHoldingRunRepository = new HouseHoldingRunRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _application1 = new Application("code", "description");
      _startDate = new DateTime(2013, 1, 1);

      _docType1 = BuildMeA.DocType("code 1", "deccription 1");
      _domicile1 = BuildMeA.Domicile("code 1", "description 1");

      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType1);
      
      _manCo1 = BuildMeA.ManCo("description 1", "name 1").WithDomicile(_domicile1);
      _manCo2 = BuildMeA.ManCo("description 2", "name 2").WithDomicile(_domicile1);
      _manCo3 = BuildMeA.ManCo("description 3", "name 3").WithDomicile(_domicile1);
      _manCo4 = BuildMeA.ManCo("description 4", "name 4").WithDomicile(_domicile1);

      _approval = BuildMeA.Approval("name", DateTime.Now);
      _rejection = BuildMeA.Rejection("name", DateTime.Now);
      _houseHold = BuildMeA.HouseHold(DateTime.Now);

      _document =
        BuildMeA.Document("documentId")
                .WithDocType(_docType1)
                .WithSubDocType(_subDocType1)
                .WithManCo(_manCo1);

      _gridRunCompleted = BuildMeA.GridRun("grid 1", false, DateTime.Now, DateTime.Now, 2)
                                  .WithApplication(_application1);

      _gridRunException = BuildMeA.GridRun("grid 4", false, DateTime.Now, null, 5)
                                   .WithApplication(_application1);

      _xmlFile1 =
        BuildMeA.XmlFile("documentSetId 1", "fileName 1", "parentFileName 1", false, DateTime.Now, DateTime.Now)
                .WithGridRun(_gridRunCompleted)
                .WithGridRun(_gridRunException)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      _gridRun2 = BuildMeA.GridRun("grid 2", false, _startDate, null, 1).WithApplication(_application1);

      _xmlFile2 =
        BuildMeA.XmlFile("documentSetId 2", "fileName 2", "parentFileName 2", false, DateTime.Now, DateTime.Now)
                .WithGridRun(_gridRun2)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      _gridRun3 =
        BuildMeA.GridRun("grid 3", false, DateTime.Now.AddHours(-3), DateTime.Now.AddHours(-2), 1)
                .WithApplication(_application1);

      _xmlFile3 =
        BuildMeA.XmlFile("documentSetId 3", "fileName 3", "parentFileName 3", false, DateTime.Now, DateTime.Now)
                .WithGridRun(this._gridRun3)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      _gridRun10 = BuildMeA.GridRun("grid 10", false, DateTime.Now.AddHours(-3), DateTime.Now.AddHours(-2), 2)
                .WithApplication(_application1);

      _gridRun12 = BuildMeA.GridRun("grid 12", false, DateTime.Now.AddHours(-3), DateTime.Now.AddHours(-2), 2)
                  .WithApplication(_application1);

      _gridRun13 = BuildMeA.GridRun("grid 13", false, DateTime.Now.AddHours(-3), DateTime.Now.AddHours(-2), 2)
              .WithApplication(_application1);

      _gridRun14 = BuildMeA.GridRun("grid 14", false, DateTime.Now.AddHours(-3), DateTime.Now.AddHours(-2), 2)
              .WithApplication(_application1);

      _xmlFile10 =
        BuildMeA.XmlFile("documentSetId 10", "fileName 10", "parentFileName 10", false, DateTime.Now, DateTime.Now)
                .WithGridRun(_gridRun10)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      _xmlFile12 =
      BuildMeA.XmlFile("documentSetId 12", "fileName 12", "parentFileName 12", false, DateTime.Now, DateTime.Now)
              .WithGridRun(_gridRun12)
              .WithDocType(_docType1)
              .WithDomicile(_domicile1)
              .WithManCo(_manCo2);

      _xmlFile13 =
      BuildMeA.XmlFile("documentSetId 13", "fileName 13", "parentFileName 13", false, DateTime.Now, DateTime.Now)
              .WithGridRun(_gridRun13)
              .WithDocType(_docType1)
              .WithDomicile(_domicile1)
              .WithManCo(_manCo3);

      _xmlFile14 =
      BuildMeA.XmlFile("documentSetId 14", "fileName 14", "parentFileName 14", false, DateTime.Now, DateTime.Now)
              .WithGridRun(_gridRun14)
              .WithDocType(_docType1)
              .WithDomicile(_domicile1)
              .WithManCo(_manCo4);

      _houseHoldingRun = BuildMeA.HouseHoldingRun(DateTime.Now, DateTime.Now, "grid");
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;

    private GridRunRepository _gridRunRepository;
    private XmlFileRepository _xmlFileRepository;
    private HouseHoldingRunRepository _houseHoldingRunRepository;

    private DocType _docType1;
    private Domicile _domicile1;
    private SubDocType _subDocType1;
   
    private ManCo _manCo1;
    private ManCo _manCo2;
    private ManCo _manCo3;
    private ManCo _manCo4;

    private Document _document;
    private Approval _approval;
    private Rejection _rejection;
    private HouseHold _houseHold;

    private GridRun _gridRunCompleted;
    private GridRun _gridRunException;
    private XmlFile _xmlFile1;

    private GridRun _gridRun2;
    private XmlFile _xmlFile2;

    private GridRun _gridRun3;
    private XmlFile _xmlFile3;

    private GridRun _gridRun4;
    private XmlFile _xmlFile4;

    private GridRun _gridRun5;
    private XmlFile _xmlFile5;

    private GridRun _gridRun6;
    private XmlFile _xmlFile6;

    private GridRun _gridRun7;
    private XmlFile _xmlFile7;

    private GridRun _gridRun8;
    private XmlFile _xmlFile8;

    private GridRun _gridRun9;
    private XmlFile _xmlFile9;

    private GridRun _gridRun10;
    private XmlFile _xmlFile10;

    private XmlFile _xmlFile11;

    private GridRun _gridRun12;
    private XmlFile _xmlFile12;

    private GridRun _gridRun13;
    private XmlFile _xmlFile13;

    private GridRun _gridRun14;
    private XmlFile _xmlFile14;

    private Application _application1;
    private HouseHoldingRun _houseHoldingRun;

    private DateTime _startDate;

    [Test]
    public void GivenDifferentTypesOfGridRuns_WhenISearchGridRunsThatAreCurrentlyRunning_IGetGridRunsForFilesThatAreRunning()
    {
      _xmlFileRepository.Create(_xmlFile1);
      _xmlFileRepository.Create(_xmlFile2);
      _xmlFileRepository.Create(_xmlFile3);

      var processingGridRuns = _gridRunRepository.GetProcessing();

      processingGridRuns.Should()
                        .NotBeEmpty()
                        .And.Contain(d => d.EndDate == null)
                        .And.Contain(d => d.XmlFile.FileName == "fileName 1")
                        .And.Contain(d => d.XmlFile.FileName  == "fileName 2")
                        .And.NotContain(d => d.XmlFile.FileName == "fileName 3");
    }

    [Test]
    public void GivenDifferentTypesOfGridRuns_WhenISearchGridRunsThatAreCurrentlyRunningForCertainMancoIds_IGetGridRunsForFilesThatAreRunning()
    {
      _gridRun4 = BuildMeA.GridRun("grid 4", false, DateTime.Now, DateTime.Now, 2).WithApplication(_application1);
        _xmlFile4 =
          BuildMeA.XmlFile("documentSetId 4", "fileName 4", "parentFileName 4", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRun4)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo2);

        _gridRun5 = BuildMeA.GridRun("grid 5", false, DateTime.Now, DateTime.Now, 2)
                        .WithApplication(_application1);
        _xmlFile5 =
          BuildMeA.XmlFile("documentSetId 5", "fileName 5", "parentFileName 5", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRun5)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo3);

      _gridRun6 = BuildMeA.GridRun("grid 6", false, DateTime.Now, null, 1).WithApplication(_application1);
        _xmlFile6 =
          BuildMeA.XmlFile("documentSetId 6", "fileName 6", "parentFileName 6", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRun6)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo4);

        _xmlFileRepository.Create(_xmlFile1);
        _xmlFileRepository.Create(_xmlFile2);
        _xmlFileRepository.Create(_xmlFile3);
        _xmlFileRepository.Create(_xmlFile4);
        _xmlFileRepository.Create(_xmlFile5);
        _xmlFileRepository.Create(_xmlFile6);

        var listManCoId = new List<int>();

        listManCoId.Add(_xmlFile1.ManCoId);
        listManCoId.Add(_xmlFile6.ManCoId);

        var processingGridRuns = _gridRunRepository.GetProcessing(listManCoId);

        processingGridRuns.Should().NotBeEmpty()
                          .And.Contain(d => d.EndDate == null)
                          .And.Contain(d => d.XmlFile.FileName == "fileName 1")
                          .And.Contain(d => d.XmlFile.FileName == "fileName 2")
                          .And.NotContain(d => d.XmlFile.FileName == "fileName 3");
    }

    [Test]
    public void GivenGridRuns_WhenIAskForTheFifteenMostRecentlySuccessfullyCompleted_IGetTheFifteenMostRecentSuccefullyCompleted()
    {
      for (int i = 4; i < 19; i++)
      {
        GridRun _gridRunxx = BuildMeA.GridRun(String.Format("grid {0}", i) , false, DateTime.Now, DateTime.Now, 2).WithApplication(_application1);

        XmlFile _xmlFilexx =
          BuildMeA.XmlFile(String.Format("documentSetId {0}", i), String.Format("fileName {0}", i), String.Format("parentFileName {0}", i), false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRunxx)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo1);

        _xmlFileRepository.Create(_xmlFilexx);
      }

      var recentlyProcessedFiles = _gridRunRepository.GetTopFifteenSuccessfullyCompleted();

      recentlyProcessedFiles.Should()
                            .NotBeEmpty()
                            .And.HaveCount(15)
                            .And.Contain(f => f.XmlFile.FileName == "fileName 18")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 17")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 16")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 15")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 14")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 13")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 12")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 11")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 10")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 9")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 8")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 7")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 5")
                            .And.Contain(f => f.XmlFile.FileName == "fileName 4")
                            .And.NotContain(f => f.XmlFile.FileName == "fileName 3")
                            .And.NotContain(d => d.XmlFile.FileName == "fileName 2")
                            .And.Contain(d => d.XmlFile.FileName == "fileName 6")
                            .And.OnlyContain(d => d.EndDate != null);
    }

    [Test]
    public void GivenGridRuns_WhenIAskForTheFifteenMostRecentlySuccessfullyCompletedForCertainMancos_IGetTheFifteenMostRecentSuccefullyCompleted()
    {
        _gridRun4 = BuildMeA.GridRun("grid 4", false, DateTime.Now, DateTime.Now, 2)
                        .WithApplication(_application1);

        _xmlFile4 =
          BuildMeA.XmlFile("documentSetId 4", "fileName 4", "parentFileName 4", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRun4)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo3);

      _gridRun5 = BuildMeA.GridRun("grid 5", false, DateTime.Now, DateTime.Now, 2).WithApplication(_application1);

        _xmlFile5 =
          BuildMeA.XmlFile("documentSetId 5", "fileName 5", "parentFileName 5", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRun5)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo2);

      _gridRun6 = BuildMeA.GridRun("grid 6", false, DateTime.Now, null, 1).WithApplication(_application1);
        _xmlFile6 =
          BuildMeA.XmlFile("documentSetId 6", "fileName 6", "parentFileName 6", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRun6)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo3);

        _gridRun7 = BuildMeA.GridRun("grid 7", false, DateTime.Now, DateTime.Now, 2)
                        .WithApplication(_application1);
        _xmlFile7 =
          BuildMeA.XmlFile("documentSetId 7", "fileName 7", "parentFileName 7", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRun7)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo4);

      _gridRun8 = BuildMeA.GridRun("grid 8", false, DateTime.Now, DateTime.Now, 2).WithApplication(_application1);
        _xmlFile8 =
          BuildMeA.XmlFile("documentSetId 8", "fileName 8", "parentFileName 8", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRun8)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo2);

        _xmlFileRepository.Create(_xmlFile1);
        _xmlFileRepository.Create(_xmlFile2);
        _xmlFileRepository.Create(_xmlFile3);
        _xmlFileRepository.Create(_xmlFile4);
        _xmlFileRepository.Create(_xmlFile5);
        _xmlFileRepository.Create(_xmlFile6);
        _xmlFileRepository.Create(_xmlFile7);
        _xmlFileRepository.Create(_xmlFile8);

        var listManCoIds = new List<int>();

        listManCoIds.Add(_xmlFile1.ManCoId);
        listManCoIds.Add(_xmlFile6.ManCoId);

        var recentlyProcessedFiles = _gridRunRepository.GetTopFifteenSuccessfullyCompleted(listManCoIds);

        recentlyProcessedFiles.Should().NotBeEmpty()
                        .And.HaveCount(2)
                        .And.Contain(f => f.XmlFile.FileName == "fileName 4")
                        .And.Contain(f => f.XmlFile.FileName == "fileName 1")
                        .And.NotContain(d => d.XmlFile.FileName == "fileName 2")
                        .And.NotContain(d => d.XmlFile.FileName == "fileName 3")
                        .And.NotContain(d => d.XmlFile.FileName == "fileName 6")
                        .And.NotContain(f => f.XmlFile.FileName == "fileName 8")
                        .And.NotContain(f => f.XmlFile.FileName == "fileName 7")
                        .And.NotContain(f => f.XmlFile.FileName == "fileName 5")
                        .And.OnlyContain(d => d.EndDate != null);
    }

    [Test]
    public void GivenGridRuns_WhenIAskForTheFiveMostRecentExceptions_IGetTheFiveMostRecentExceptions()
    {
      GridRun gridRun1 = BuildMeA.GridRun("grid 1", false, DateTime.Now, DateTime.Now, 4).WithApplication(_application1);

      _xmlFile1 =
        BuildMeA.XmlFile("documentSetId 1", "fileName 1", "parentFileName 1", false, DateTime.Now, DateTime.Now)
                .WithGridRun(gridRun1)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      this._gridRun2 =
        BuildMeA.GridRun("grid 2", false, DateTime.Now, DateTime.Now, 5)
                .WithApplication(_application1);

      _xmlFile2 =
        BuildMeA.XmlFile("documentSetId 2", "fileName 2", "parentFileName 2", false, DateTime.Now, DateTime.Now)
                .WithGridRun(this._gridRun2)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      this._gridRun3 =
        BuildMeA.GridRun("grid 3", false, DateTime.Now.AddHours(-3), DateTime.Now.AddHours(-2), 4)
                .WithApplication(_application1);

      _xmlFile3 =
        BuildMeA.XmlFile("documentSetId 3", "fileName 3", "parentFileName 3", false, DateTime.Now, DateTime.Now)
                .WithGridRun(this._gridRun3)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      this._gridRun4 = BuildMeA.GridRun("grid 4", false, DateTime.Now, DateTime.Now, 4)
                       .WithApplication(_application1);

      _xmlFile4 =
        BuildMeA.XmlFile("documentSetId 4", "fileName 4", "parentFileName 4", false, DateTime.Now, DateTime.Now)
                .WithGridRun(this._gridRun4)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      this._gridRun5 =
        BuildMeA.GridRun("grid 5", false, DateTime.Now, null, 1)
                  .WithApplication(_application1);

      _xmlFile5 =
        BuildMeA.XmlFile("documentSetId 5", "fileName 5", "parentFileName 5", false, DateTime.Now, DateTime.Now)
                .WithGridRun(this._gridRun5)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      this._gridRun6 = BuildMeA.GridRun("grid 6", false, DateTime.Now, DateTime.Now, 5).WithApplication(_application1);

      _xmlFile6 =
        BuildMeA.XmlFile("documentSetId 6", "fileName 6", "parentFileName 6", false, DateTime.Now, DateTime.Now)
                .WithGridRun(this._gridRun6)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      this._gridRun7 = BuildMeA.GridRun("grid 7", false, DateTime.Now, DateTime.Now, 4)
                        .WithApplication(_application1);

      _xmlFile7 =
        BuildMeA.XmlFile("documentSetId 7", "fileName 7", "parentFileName 7", false, DateTime.Now, DateTime.Now)
                .WithGridRun(this._gridRun7)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      this._gridRun8 = BuildMeA.GridRun("grid 8", false, DateTime.Now, DateTime.Now, 3).WithApplication(_application1);

      _xmlFile8 =
        BuildMeA.XmlFile("documentSetId 8", "fileName 8", "parentFileName 8", false, DateTime.Now, DateTime.Now)
                .WithGridRun(this._gridRun8)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      this._gridRun9 = BuildMeA.GridRun("grid 9", false, DateTime.Now, DateTime.Now, 6).WithApplication(_application1);

      _xmlFile9 =
        BuildMeA.XmlFile("documentSetId 9", "fileName 9", "parentFileName 9", false, DateTime.Now, DateTime.Now)
                .WithGridRun(this._gridRun9)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      _xmlFileRepository.Create(_xmlFile1);
      _xmlFileRepository.Create(_xmlFile2);
      _xmlFileRepository.Create(_xmlFile3);
      _xmlFileRepository.Create(_xmlFile4);
      _xmlFileRepository.Create(_xmlFile5);
      _xmlFileRepository.Create(_xmlFile6);
      _xmlFileRepository.Create(_xmlFile7);
      _xmlFileRepository.Create(_xmlFile8);
      _xmlFileRepository.Create(_xmlFile9);

      for (int i = 20; i < 29; i++)
      {
        GridRun _gridRunxx = BuildMeA.GridRun(String.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2).WithApplication(_application1);

        XmlFile _xmlFilexx =
          BuildMeA.XmlFile(String.Format("documentSetId {0}", i), String.Format("fileName {0}", i), String.Format("parentFileName {0}", i), false, DateTime.Now, DateTime.Now)
                  .WithGridRun(_gridRunxx)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo1);

        _xmlFileRepository.Create(_xmlFilexx);
      }

      var recentExceptions = _gridRunRepository.GetTopFifteenRecentExceptions();

      recentExceptions.Should()
                      .NotBeEmpty()
                      .And.HaveCount(15)
                      .And.Contain(f => f.XmlFile.FileName == "fileName 7")
                      .And.Contain(f => f.XmlFile.FileName == "fileName 6")
                      .And.Contain(f => f.XmlFile.FileName == "fileName 4")
                      .And.Contain(f => f.XmlFile.FileName == "fileName 3")
                      .And.Contain(f => f.XmlFile.FileName == "fileName 2");
    }

    [Test]
    public void GivenGridRuns_WhenIAskForTheFifteenMostRecentExceptionsForCertainMancos_IGetTheFifteenMostRecentExceptions()
    {
      GridRun gridRun1 = BuildMeA.GridRun("grid 1", false, DateTime.Now, DateTime.Now, 4).WithApplication(_application1);

        _xmlFile1 =
          BuildMeA.XmlFile("documentSetId 1", "fileName 1", "parentFileName 1", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(gridRun1)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo1);

      this._gridRun2 = BuildMeA.GridRun("grid 2", false, DateTime.Now, DateTime.Now, 5).WithApplication(_application1);

        _xmlFile2 =
          BuildMeA.XmlFile("documentSetId 2", "fileName 2", "parentFileName 2", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(this._gridRun2)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo2);

      this._gridRun3 =
        BuildMeA.GridRun("grid 3", false, DateTime.Now.AddHours(-3), DateTime.Now.AddHours(-2), 4)
                .WithApplication(_application1);

        _xmlFile3 =
          BuildMeA.XmlFile("documentSetId 3", "fileName 3", "parentFileName 3", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(this._gridRun3)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo4);

      this._gridRun4 = BuildMeA.GridRun("grid 4", false, DateTime.Now, DateTime.Now, 4).WithApplication(_application1);

        _xmlFile4 =
          BuildMeA.XmlFile("documentSetId 4", "fileName 4", "parentFileName 4", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(this._gridRun4)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo1);

      this._gridRun5 = BuildMeA.GridRun("grid 5", false, DateTime.Now, null, 1).WithApplication(_application1);

        _xmlFile5 =
          BuildMeA.XmlFile("documentSetId 5", "fileName 5", "parentFileName 5", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(this._gridRun5)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo3);

      this._gridRun6 = BuildMeA.GridRun("grid 6", false, DateTime.Now, DateTime.Now, 5).WithApplication(_application1);

        _xmlFile6 =
          BuildMeA.XmlFile("documentSetId 6", "fileName 6", "parentFileName 6", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(this._gridRun6)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo2);

      this._gridRun7 = BuildMeA.GridRun("grid 7", false, DateTime.Now, DateTime.Now, 4).WithApplication(_application1);

        _xmlFile7 =
          BuildMeA.XmlFile("documentSetId 7", "fileName 7", "parentFileName 7", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(this._gridRun7)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo4);

        this._gridRun8 = BuildMeA.GridRun("grid 8", false, DateTime.Now, DateTime.Now, 3)
                        .WithApplication(_application1);

        _xmlFile8 =
          BuildMeA.XmlFile("documentSetId 8", "fileName 8", "parentFileName 8", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(this._gridRun8)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo4);

      this._gridRun9 = BuildMeA.GridRun("grid 9", false, DateTime.Now, DateTime.Now, 6).WithApplication(_application1);

        _xmlFile9 =
          BuildMeA.XmlFile("documentSetId 9", "fileName 9", "parentFileName 9", false, DateTime.Now, DateTime.Now)
                  .WithGridRun(this._gridRun9)
                  .WithDocType(_docType1)
                  .WithDomicile(_domicile1)
                  .WithManCo(_manCo3);

        _xmlFileRepository.Create(_xmlFile1);
        _xmlFileRepository.Create(_xmlFile2);
        _xmlFileRepository.Create(_xmlFile3);
        _xmlFileRepository.Create(_xmlFile4);
        _xmlFileRepository.Create(_xmlFile5);
        _xmlFileRepository.Create(_xmlFile6);
        _xmlFileRepository.Create(_xmlFile7);
        _xmlFileRepository.Create(_xmlFile8);
        _xmlFileRepository.Create(_xmlFile9);

        var listManCoIds = new List<int>();

        listManCoIds.Add(_xmlFile1.ManCoId);
        listManCoIds.Add(_xmlFile6.ManCoId);

        var recentExceptions = _gridRunRepository.GetTopFifteenRecentExceptions(listManCoIds);

        recentExceptions.Should()
                        .NotBeEmpty()
                        .And.Contain(f => f.XmlFile.FileName == "fileName 6")
                        .And.Contain(f => f.XmlFile.FileName == "fileName 4")
                        .And.Contain(f => f.XmlFile.FileName == "fileName 2")
                        .And.Contain(f => f.XmlFile.FileName == "fileName 1")
                        .And.NotContain(f => f.XmlFile.FileName == "fileName 5")
                        .And.NotContain(f => f.XmlFile.FileName == "fileName 3")
                        .And.NotContain(f => f.XmlFile.FileName == "fileName 7")
                        .And.HaveCount(4);
    }

    [Test]
    public void GivenAGridRun_WhenISearchById_IRetrieveTheGridRun()
    {
      _xmlFileRepository.Create(_xmlFile2);

      int id = _xmlFile2.GridRuns.First(j => j.Grid == "grid 2").Id;

      var result = _gridRunRepository.GetGridRun(id);
      result.Grid.Should().Be("grid 2");
    }

    [Test]
    public void GivenAGridRun_WhenISearchByApplicationAndGrid_IRetrieveTheGridRun()
    {
      _xmlFileRepository.Create(_xmlFile2);

      var result = _gridRunRepository.GetGridRun("code", "grid 2");
      result.Grid.Should().Be("grid 2");
      result.XmlFile.FileName.Should().Be("fileName 2");
      result.Application.Code.Should().Be("code");
      result.StartDate.Should().Be(_startDate);
    }

    [Test]
    public void GivenAGridRun_WhenISearchByGrid_IRetrieveTheGridRun()
    {
      _xmlFileRepository.Create(_xmlFile2);

      var result = _gridRunRepository.GetGridRun("grid 2");
      result.Grid.Should().Be("grid 2");
      result.XmlFile.FileName.Should().Be("fileName 2");
      result.Application.Code.Should().Be("code");
      result.StartDate.Should().Be(_startDate);
    }

    [Test]
    public void GivenAGridRun_WhenISearchByFileApplicationAndGrid_IRetrieveTheGridRun()
    {
      _xmlFileRepository.Create(_xmlFile2);

      var result = _gridRunRepository.GetGridRun("fileName 2", "code", "grid 2", _startDate);
      result.Grid.Should().Be("grid 2");
      result.XmlFile.FileName.Should().Be("fileName 2");
      result.Application.Code.Should().Be("code");
      result.StartDate.Should().Be(_startDate);
    }

    [Test]
    public void GivenAGridRun_WhenITryToCreateIt_TheGridRunIsSavedToTheDataBase()
    {
      int initialCount = _xmlFileRepository.Entities.Count();
      
      _xmlFileRepository.Create(_xmlFile2);

      _xmlFileRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenAGridRun_WhenIUpdateTheGridRun_TheGridRunIsUpdated()
    {
      DateTime newEndDate = new DateTime(2013, 2, 2);
      DateTime newStartDate = new DateTime(2013, 2, 1);

      _xmlFileRepository.Create(_xmlFile2);

      int id = _xmlFile2.GridRuns.First(j => j.Grid == "grid 2").Id;

      var gridRun = _gridRunRepository.GetGridRun(id);

      gridRun.UpdateGridRun(newStartDate, newEndDate, 2, null, null);
      _gridRunRepository.Update(gridRun);

      gridRun = _gridRunRepository.GetGridRun(gridRun.Id);

      gridRun.StartDate.Should().Be(newStartDate);
      gridRun.EndDate.Should().Be(newEndDate);
      gridRun.Status.Should().Be(2);
    }

    [Test]
    public void GivenAGridRunWhereTheStatusIsUnknown_WhenIUpdateTheGridRunForAGridRunWhereTheStatusIsNotUnknow_TheGridRunIsNotUpdated()
    {
      DateTime newEndDate = new DateTime(2013, 2, 2);
      DateTime newStartDate = new DateTime(2013, 2, 1);

      _xmlFileRepository.Create(_xmlFile10);

      int id = _xmlFile10.GridRuns.First(j => j.Grid == "grid 10").Id;
      var gridRun = _gridRunRepository.GetGridRun(id);

      DateTime oldEndDate = gridRun.EndDate.Value;
      DateTime oldStartDate = gridRun.StartDate.Value;
      var oldStatus = gridRun.Status;

      gridRun.UpdateGridRun(null, null, 0, null, null);
      _gridRunRepository.Update(gridRun);

      gridRun = _gridRunRepository.GetGridRun(gridRun.Id);

      gridRun.StartDate.Should().Be(oldStartDate);
      gridRun.EndDate.Should().Be(oldEndDate);
      gridRun.Status.Should().Be(oldStatus);
    }

    [Test]
    public void GivenAGridRunWhereTheFileIsAlreadyKnown_WhenIUpdateTheGridRun_TheFileIsNotUpdated()
    {
      _xmlFileRepository.Create(_xmlFile10);

      int xmlFileId = _xmlFile10.GridRuns.First(j => j.Grid == "grid 10").XmlFile.Id;
      int gridRunId = _xmlFile10.GridRuns.First(j => j.Grid == "grid 10").Id;

      var gridRun = _gridRunRepository.GetGridRun(gridRunId);

      _xmlFile11 = BuildMeA.XmlFile("documentSetId 11", "fileName 11", "parentFileName 11", false, DateTime.Now, DateTime.Now)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      _xmlFileRepository.Create(_xmlFile11);

      gridRun.UpdateGridRun(null, null, 0, _xmlFile11.Id, null);
      _gridRunRepository.Update(gridRun);

      gridRun = _gridRunRepository.GetGridRun(gridRun.Id);

      gridRun.XmlFileId.Should().Be(xmlFileId);
    }

    [Test]
    public void GivenAProcssedGridRun_WhenIUpdateTheGridRunWithHouseHoldInfo_TheFileIsNotUpdated()
    {
      _xmlFileRepository.Create(_xmlFile10);

      int xmlFileId = _xmlFile10.GridRuns.First(j => j.Grid == "grid 10").XmlFile.Id;
      int gridRunId = _xmlFile10.GridRuns.First(j => j.Grid == "grid 10").Id;

      var gridRun = _gridRunRepository.GetGridRun(gridRunId);

      _xmlFile11 = BuildMeA.XmlFile("documentSetId 11", "fileName 11", "parentFileName 11", false, DateTime.Now, DateTime.Now)
                .WithDocType(_docType1)
                .WithDomicile(_domicile1)
                .WithManCo(_manCo1);

      _xmlFileRepository.Create(_xmlFile11);
      _houseHoldingRunRepository.Create(_houseHoldingRun);

      gridRun.UpdateGridRun(null, null, 0, _xmlFile11.Id, _houseHoldingRun.Id);
      _gridRunRepository.Update(gridRun);

      gridRun = _gridRunRepository.GetGridRun(gridRun.Id);

      gridRun.XmlFileId.Should().Be(xmlFileId);
      gridRun.HouseHoldingRunId.Should().Be(_houseHoldingRun.Id);
    }

    [Test]
    public void GivenGridRunData_WhenISearchForASingleGridRun_TheGridRunIsReturned()
    {
      _xmlFileRepository.Create(_xmlFile1);
      _xmlFileRepository.Create(_xmlFile2);
      _xmlFileRepository.Create(_xmlFile3);

      var gridRuns = _gridRunRepository.Search("grid 2");
      gridRuns.Should().HaveCount(1)
        .And.Contain(g => g.Grid == "grid 2");
    }

    [Test]
    public void GivenGridRunData_WhenISearchForGridRunsUsingAWildCard_TheGridDataIsReturned()
    {
      _xmlFileRepository.Create(_xmlFile1);
      _xmlFileRepository.Create(_xmlFile2);
      _xmlFileRepository.Create(_xmlFile3);

      var gridRuns = _gridRunRepository.Search("grid");
      gridRuns.Should().HaveCount(4)
        .And.Contain(g => g.Grid == "grid 1")
        .And.Contain(g => g.Grid == "grid 2")
        .And.Contain(g => g.Grid == "grid 3")
        .And.Contain(g => g.Grid == "grid 4");
    }

    [Test]
    public void GivenGridRuns_WhenIAskForTheFifteenMostRecentGridsThatHaveDocumentsThatAreNeitherApprovedOrRejected_IGetTheFifteenMostRecentUnapporvedGrids()
    {
      for (int i = 1; i < 19; i++)
      {
        if (i == 5)
        {
          Document documentApproved =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo1)
                    .WithSubDocType(_subDocType1)
                    .WithApproval(_approval);

          GridRun gridRunApproved =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentApproved);

          _gridRunRepository.Create(gridRunApproved);

        }
        else if (i == 6)
        {
          Document documentDifferentManCo =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo2)
                    .WithSubDocType(_subDocType1);

          GridRun gridRunDifferentManco =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentDifferentManCo);

          _gridRunRepository.Create(gridRunDifferentManco);
        }
        else if (i == 7)
        {
          Document document =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo1)
                    .WithSubDocType(_subDocType1)
                    .WithRejection(_rejection);

          GridRun gridRun =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(document);

          _gridRunRepository.Create(gridRun);
        }

        else
        {
          Document document =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo1)
                    .WithSubDocType(_subDocType1);

          GridRun gridRun =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(document);

          _gridRunRepository.Create(gridRun);
        }
      }

      var listManCoId = new List<int>();

      listManCoId.Add(_manCo1.Id);

      var recentlyUnapprovedGrids = _gridRunRepository.GetTopFifteenRecentUnapprovedGrids(listManCoId);

      recentlyUnapprovedGrids.Should()
                             .HaveCount(15)
                             .And.Contain(g => g.Grid == "grid 1")
                             .And.Contain(g => g.Grid == "grid 2")
                             .And.Contain(g => g.Grid == "grid 3")
                             .And.Contain(g => g.Grid == "grid 4")
                             .And.NotContain(g => g.Grid == "grid 5")
                             .And.NotContain(g => g.Grid == "grid 6")
                             .And.NotContain(g => g.Grid == "grid 7")
                             .And.Contain(g => g.Grid == "grid 8")
                             .And.Contain(g => g.Grid == "grid 9")
                             .And.Contain(g => g.Grid == "grid 10")
                             .And.Contain(g => g.Grid == "grid 12")
                             .And.Contain(g => g.Grid == "grid 13");
    }

    [Test]
    public void GivenGridRuns_WhenIAskForTheFifteenMostRecentGridsThatHaveDocumentsThatHaveBeenRejected_IGetTheFifteenMostRecentGridsThatHaveRejectedDocuments()
    {
      for (int i = 1; i < 19; i++)
      {
        if (i == 5)
        {
          Document documentApproved =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo1)
                    .WithSubDocType(_subDocType1)
                    .WithApproval(_approval);

          GridRun gridRunApproved =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentApproved);

          _gridRunRepository.Create(gridRunApproved);

        }
        else if (i == 6)
        {
          Document documentDifferentManCo =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo2)
                    .WithSubDocType(_subDocType1);

          GridRun gridRunDifferentManco =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentDifferentManCo);

          _gridRunRepository.Create(gridRunDifferentManco);
        }
        else if (i == 7)
        {
          Document document =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo1)
                    .WithSubDocType(_subDocType1);

          GridRun gridRun =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(document);

          _gridRunRepository.Create(gridRun);
        }

        else
        {
          Rejection rejection = BuildMeA.Rejection("name", DateTime.Now);

          Document document =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo1)
                    .WithSubDocType(_subDocType1)
                    .WithRejection(rejection);

          GridRun gridRun =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(document);

          _gridRunRepository.Create(gridRun);
        }
      }

      var listManCoId = new List<int>();

      listManCoId.Add(_manCo1.Id);

      var recentlyUnapprovedGrids = _gridRunRepository.GetTopFifteenGridsWithRejectedDocuments(listManCoId);

      recentlyUnapprovedGrids.Should()
                             .HaveCount(15)
                             .And.Contain(g => g.Grid == "grid 1")
                             .And.Contain(g => g.Grid == "grid 2")
                             .And.Contain(g => g.Grid == "grid 3")
                             .And.Contain(g => g.Grid == "grid 4")
                             .And.NotContain(g => g.Grid == "grid 5")
                             .And.NotContain(g => g.Grid == "grid 6")
                             .And.NotContain(g => g.Grid == "grid 7")
                             .And.Contain(g => g.Grid == "grid 8")
                             .And.Contain(g => g.Grid == "grid 9")
                             .And.Contain(g => g.Grid == "grid 10")
                             .And.Contain(g => g.Grid == "grid 12")
                             .And.Contain(g => g.Grid == "grid 13");
    }

    [Test]
    public void GivenGridRuns_WhenIAskForTheFifteenMostRecentGridsThatHaveDocumentsThatAreReadyForHouseHolding_IGetTheFifteenMostRecentGridsThatHaveDocumentsReadyForHouseHolding()
    {
      for (int i = 1; i < 19; i++)
      {
        if (i == 5)
        {
          Document documentApprovedAndHouseHeld =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo1)
                    .WithSubDocType(_subDocType1)
                    .WithApproval(_approval)
                    .WithHouseHeld(_houseHold);

          GridRun gridRunAlreadyHouseHeld =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentApprovedAndHouseHeld);

          _gridRunRepository.Create(gridRunAlreadyHouseHeld);

        }
        else if (i == 6)
        {
          Approval approval = BuildMeA.Approval("name", DateTime.Now);

          Document documentDifferentManCo =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo2)
                    .WithSubDocType(_subDocType1)
                    .WithApproval(approval);

          GridRun gridRunDifferentManco =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentDifferentManCo);

          _gridRunRepository.Create(gridRunDifferentManco);
        }
        else
        {
          Approval approval = BuildMeA.Approval("name", DateTime.Now);

          Document documentReadyForHouseHolding =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo1)
                    .WithSubDocType(_subDocType1)
                    .WithApproval(approval);

          GridRun gridRunReadyForHouseHolding =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentReadyForHouseHolding);

          _gridRunRepository.Create(gridRunReadyForHouseHolding);
        }
      }

      var listManCoId = new List<int>();

      listManCoId.Add(_manCo1.Id);

      var recentlyUnapprovedGrids = _gridRunRepository.GetTopFifteenGridsAwaitingHouseHolding(listManCoId);

      recentlyUnapprovedGrids.Should()
                             .HaveCount(15)
                             .And.NotContain(g => g.Grid == "grid 1")
                             .And.Contain(g => g.Grid == "grid 2")
                             .And.Contain(g => g.Grid == "grid 3")
                             .And.Contain(g => g.Grid == "grid 4")
                             .And.NotContain(g => g.Grid == "grid 5")
                             .And.NotContain(g => g.Grid == "grid 6")
                             .And.Contain(g => g.Grid == "grid 7")
                             .And.Contain(g => g.Grid == "grid 8")
                             .And.Contain(g => g.Grid == "grid 9")
                             .And.Contain(g => g.Grid == "grid 10")
                             .And.Contain(g => g.Grid == "grid 12")
                             .And.Contain(g => g.Grid == "grid 13");
    }

    [Test]
    public void GivenGridRuns_WhenIAddUnApprovedGridsToTheDatabase_ICanRetrieveTheFirstPage()
    {
      for (int i = 1; i < 32; i++)
      {
        if (i == 6)
        {
          Document documentDifferentManCo =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo2)
                    .WithSubDocType(_subDocType1);

          GridRun gridRunDifferentManco =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentDifferentManCo);

          _gridRunRepository.Create(gridRunDifferentManco);
        }
        else
        {
          Document document =
            BuildMeA.Document(string.Format("document {0}", i))
          .WithDocType(_docType1)
          .WithManCo(_manCo1)
          .WithSubDocType(_subDocType1);

          GridRun gridRun =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(document);

          _gridRunRepository.Create(gridRun);
        }
      }

      var listManCoId = new List<int>();

      listManCoId.Add(_manCo1.Id);

      var recentlyUnapprovedGrids = _gridRunRepository.GetUnapproved(1, 10, listManCoId);

      recentlyUnapprovedGrids.ItemsPerPage.Should().Be(10);
      recentlyUnapprovedGrids.CurrentPage.Should().Be(1);
      recentlyUnapprovedGrids.Results.Should()
                             .HaveCount(10)
                             .And.Contain(g => g.Grid == "grid 31")
                             .And.Contain(g => g.Grid == "grid 30")
                             .And.Contain(g => g.Grid == "grid 29")
                             .And.Contain(g => g.Grid == "grid 28")
                             .And.Contain(g => g.Grid == "grid 27")
                             .And.NotContain(g => g.Grid == "grid 6")
                             .And.Contain(g => g.Grid == "grid 26")
                             .And.Contain(g => g.Grid == "grid 25")
                             .And.Contain(g => g.Grid == "grid 24")
                             .And.Contain(g => g.Grid == "grid 23")
                             .And.Contain(g => g.Grid == "grid 22")
                             .And.NotContain(g => g.Grid == "grid 12")
                             .And.NotContain(g => g.Grid == "grid 21");
    }

    [Test]
    public void GivenGridRuns_WhenIAddUnApprovedGridsToTheDatabase_ICanRetrieveTheSecondPage()
    {
      for (int i = 1; i < 32; i++)
      {
        if (i == 16)
        {
          Document documentDifferentManCo =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo2)
                    .WithSubDocType(_subDocType1);

          GridRun gridRunDifferentManco =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentDifferentManCo);

          _gridRunRepository.Create(gridRunDifferentManco);
        }
        else
        {
          Document document =
            BuildMeA.Document(string.Format("document {0}", i))
          .WithDocType(_docType1)
          .WithManCo(_manCo1)
          .WithSubDocType(_subDocType1);

          GridRun gridRun =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(document);

          _gridRunRepository.Create(gridRun);
        }
      }

      var listManCoId = new List<int>();

      listManCoId.Add(_manCo1.Id);

      var recentlyUnapprovedGrids = _gridRunRepository.GetUnapproved(2, 10, listManCoId);
      
      recentlyUnapprovedGrids.ItemsPerPage.Should().Be(10);
      recentlyUnapprovedGrids.CurrentPage.Should().Be(2);
      recentlyUnapprovedGrids.Results.Should()
                             .HaveCount(10)
                             .And.Contain(g => g.Grid == "grid 12")
                             .And.Contain(g => g.Grid == "grid 13")
                             .And.Contain(g => g.Grid == "grid 14")
                             .And.Contain(g => g.Grid == "grid 15")
                             .And.NotContain(g => g.Grid == "grid 16")
                             .And.Contain(g => g.Grid == "grid 17")
                             .And.Contain(g => g.Grid == "grid 18")
                             .And.Contain(g => g.Grid == "grid 19")
                             .And.Contain(g => g.Grid == "grid 20")
                             .And.Contain(g => g.Grid == "grid 21")
                             .And.NotContain(g => g.Grid == "grid 22");
    }

    [Test]
    public void GivenGridRuns_WhenIAddUnApprovedGridsToTheDatabase_ICanRetrieveTheThirdPage()
    {
      for (int i = 1; i < 32; i++)
      {
        if (i == 26)
        {
          Document documentDifferentManCo =
            BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType1)
                    .WithManCo(_manCo2)
                    .WithSubDocType(_subDocType1);

          GridRun gridRunDifferentManco =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentDifferentManCo);

          _gridRunRepository.Create(gridRunDifferentManco);
        }
        else
        {
          Document document =
            BuildMeA.Document(string.Format("document {0}", i))
          .WithDocType(_docType1)
          .WithManCo(_manCo1)
          .WithSubDocType(_subDocType1);

          GridRun gridRun =
            BuildMeA.GridRun(string.Format("grid {0}", i), false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(document);

          _gridRunRepository.Create(gridRun);
        }
      }

      var listManCoId = new List<int>();

      listManCoId.Add(_manCo1.Id);

      var recentlyUnapprovedGrids = _gridRunRepository.GetUnapproved(3, 10, listManCoId);

      recentlyUnapprovedGrids.ItemsPerPage.Should().Be(10);
      recentlyUnapprovedGrids.CurrentPage.Should().Be(3);
      recentlyUnapprovedGrids.Results.Should().HaveCount(10)
                             .And.NotContain(g => g.Grid == "grid11")
                             .And.Contain(g => g.Grid == "grid 10")
                             .And.Contain(g => g.Grid == "grid 9")
                             .And.Contain(g => g.Grid == "grid 8")
                             .And.Contain(g => g.Grid == "grid 7")
                             .And.Contain(g => g.Grid == "grid 5")
                             .And.Contain(g => g.Grid == "grid 4")
                             .And.Contain(g => g.Grid == "grid 3")
                             .And.Contain(g => g.Grid == "grid 2")
                             .And.Contain(g => g.Grid == "grid 1");
    }

    [Test]
    public void GivenGridRunsAndMancoId_WhenIWantToRetrieveGridRuns_ICanRetrieveTheGridRuns()
    {
        _gridRunRepository.Create(_gridRun12);
        _gridRunRepository.Create(_gridRun13);
        _gridRunRepository.Create(_gridRun14);
        
        _xmlFileRepository.Create(_xmlFile12);
        _xmlFileRepository.Create(_xmlFile13);
        _xmlFileRepository.Create(_xmlFile14);
        
        var listManCoId = new List<int>();

        listManCoId.Add(_xmlFile12.ManCoId);
        listManCoId.Add(_xmlFile13.ManCoId);

        var gridRuns = _gridRunRepository.Search("grid", listManCoId);

        gridRuns.Should().NotBeNull();
        gridRuns.Should().HaveCount(2)
            .And.Contain(g => g.Grid == "grid 12")
            .And.Contain(g => g.Grid == "grid 13");
    }

    [Test]
    public void GivenAHouseHoldingGrid_WhenISearchForGridRuns_IRetrieveGridRuns()
    {
      HouseHoldingRun houseHoldingRun1 =
        BuildMeA.HouseHoldingRun(DateTime.Now.AddMinutes(-10), DateTime.Now.AddMinutes(-1), "hhGrid1")
                .WithGridRun(_gridRun2)
                .WithGridRun(_gridRun3);

      _houseHoldingRunRepository.Create(houseHoldingRun1);
      
        Document documentDifferentManCo =
            BuildMeA.Document("document 1")
                    .WithDocType(_docType1)
                    .WithManCo(_manCo2)
                    .WithSubDocType(_subDocType1);

        GridRun gridRunDifferentManco =
            BuildMeA.GridRun("grid 1", false, DateTime.Now, DateTime.Now, 2)
                    .WithApplication(_application1)
                    .WithDocument(documentDifferentManCo);

      Document documentOKManCo =
            BuildMeA.Document("document 2")
                    .WithDocType(_docType1)
                    .WithManCo(_manCo1)
                    .WithSubDocType(_subDocType1);

        GridRun grd2 =
          BuildMeA.GridRun("grid 2", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(documentOKManCo);

        Document doc3 =
              BuildMeA.Document("document 2")
                      .WithDocType(_docType1)
                      .WithManCo(_manCo1)
                      .WithSubDocType(_subDocType1);

        GridRun grd3 =
          BuildMeA.GridRun("grid 3", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc3);

        Document doc4 =
                BuildMeA.Document("document 2")
                        .WithDocType(_docType1)
                        .WithManCo(_manCo1)
                        .WithSubDocType(_subDocType1);

        GridRun grd4 =
          BuildMeA.GridRun("grid 4", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc4);

        Document doc5 =
                BuildMeA.Document("document 2")
                        .WithDocType(_docType1)
                        .WithManCo(_manCo1)
                        .WithSubDocType(_subDocType1);

        GridRun grd5 =
          BuildMeA.GridRun("grid 5", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc5);

        Document doc6 =
                BuildMeA.Document("document 2")
                        .WithDocType(_docType1)
                        .WithManCo(_manCo1)
                        .WithSubDocType(_subDocType1);

        GridRun grd6 =
          BuildMeA.GridRun("grid 6", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc6);

        Document doc7 =
                BuildMeA.Document("document 2")
                        .WithDocType(_docType1)
                        .WithManCo(_manCo1)
                        .WithSubDocType(_subDocType1);

        GridRun grd7 =
          BuildMeA.GridRun("grid 7", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc7);

        Document doc8 =
                BuildMeA.Document("document 2")
                        .WithDocType(_docType1)
                        .WithManCo(_manCo1)
                        .WithSubDocType(_subDocType1);

        GridRun grd8 =
          BuildMeA.GridRun("grid 8", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc8);

        Document doc9 =
                BuildMeA.Document("document 2")
                        .WithDocType(_docType1)
                        .WithManCo(_manCo1)
                        .WithSubDocType(_subDocType1);

        GridRun grd9 =
          BuildMeA.GridRun("grid 9", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc9);

        Document doc10 =
                BuildMeA.Document("document 2")
                        .WithDocType(_docType1)
                        .WithManCo(_manCo1)
                        .WithSubDocType(_subDocType1);

        GridRun grd10 =
          BuildMeA.GridRun("grid 10", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc10);

        Document doc11 =
                BuildMeA.Document("document 2")
                        .WithDocType(_docType1)
                        .WithManCo(_manCo1)
                        .WithSubDocType(_subDocType1);

        GridRun grd11 =
          BuildMeA.GridRun("grid 11", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc11);

        Document doc12 =
                  BuildMeA.Document("document 2")
                          .WithDocType(_docType1)
                          .WithManCo(_manCo1)
                          .WithSubDocType(_subDocType1);

        GridRun grd12 =
          BuildMeA.GridRun("grid 11", false, DateTime.Now, DateTime.Now, 2)
                  .WithApplication(_application1)
                  .WithDocument(doc12);

      HouseHoldingRun houseHoldingRun2 =
        BuildMeA.HouseHoldingRun(DateTime.Now.AddMinutes(-10), DateTime.Now.AddMinutes(-1), "hhGrid2")
                .WithGridRun(gridRunDifferentManco)
                .WithGridRun(grd2)
                .WithGridRun(grd3)
                .WithGridRun(grd4)
                .WithGridRun(grd5)
                .WithGridRun(grd6)
                .WithGridRun(grd7)
                .WithGridRun(grd8)
                .WithGridRun(grd9)
                .WithGridRun(grd10)
                .WithGridRun(grd11);
      
      _houseHoldingRunRepository.Create(houseHoldingRun2);

      var listManCoId = new List<int>();
      listManCoId.Add(_manCo1.Id);

      var gridRuns = _gridRunRepository.GetGridRuns(1, 10, "hhGrid2", listManCoId);

      gridRuns.ItemsPerPage.Should().Be(10);
      gridRuns.CurrentPage.Should().Be(1);
      gridRuns.Results.Should().HaveCount(10)
                             .And.NotContain(g => g.Grid == "gridRunDifferentManco")
                             .And.Contain(g => g.Grid == "grid 2")
                             .And.Contain(g => g.Grid == "grid 3")
                             .And.Contain(g => g.Grid == "grid 4")
                             .And.Contain(g => g.Grid == "grid 5")
                             .And.Contain(g => g.Grid == "grid 6")
                             .And.Contain(g => g.Grid == "grid 7")
                             .And.Contain(g => g.Grid == "grid 8")
                             .And.Contain(g => g.Grid == "grid 9")
                             .And.Contain(g => g.Grid == "grid 10")
                             .And.Contain(g => g.Grid == "grid 11")
                             .And.NotContain(g => g.Grid == "grid 12");
    }
  }
}
