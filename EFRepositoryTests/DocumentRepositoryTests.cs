namespace EFRepositoryTests
{
  using System;
  using System.Configuration;
  using System.Linq;
  using System.Transactions;
  using Builder;
  using Entities;
  using Entities.File;
  using Exceptions;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class DocumentRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _documentRepository = new DocumentRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _gridRunRepository = new GridRunRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _xmlFileRepository = new XmlFileRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _houseHoldingRunRepository = new HouseHoldingRunRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _domicile = BuildMeA.Domicile("code", "description");
      _manCo1 = BuildMeA.ManCo("description1", "code1").WithDomicile(_domicile);
      _manCo2 = BuildMeA.ManCo("description2", "code2").WithDomicile(_domicile);
      _manCo3 = BuildMeA.ManCo("description3", "code3").WithDomicile(_domicile);
      _docType = BuildMeA.DocType("code", "description");
      _docType2 = BuildMeA.DocType("code2", "description2");
      _docType3 = BuildMeA.DocType("code3", "description3");

      _application1 = new Application("code", "description");

      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType);
      _subDocType2 = BuildMeA.SubDocType("code 2", "description 3").WithDocType(_docType);
      _houseHoldingRun = BuildMeA.HouseHoldingRun(DateTime.Now.AddMinutes(-10), DateTime.Now.AddMinutes(-2), "grid");

      _approval1 = BuildMeA.Approval("name", DateTime.Now);
      _approval2 = BuildMeA.Approval("name", DateTime.Now);
      _approval3 = BuildMeA.Approval("name", DateTime.Now);
      _approval4 = BuildMeA.Approval("name", DateTime.Now);
      _approval5 = BuildMeA.Approval("name", DateTime.Now);
      _export = BuildMeA.Export(DateTime.Now);

      _rejection = BuildMeA.Rejection("user", DateTime.Now);

      _document = BuildMeA.Document("id").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1).WithApproval(_approval4);//.WithGridRun(_gridRun);
      _document2 = BuildMeA.Document("id2").WithDocType(_docType2).WithSubDocType(_subDocType2).WithManCo(_manCo2).WithApproval(_approval5);//.WithGridRun(_gridRun2);
      _document3 = BuildMeA.Document("id3").WithDocType(_docType2).WithSubDocType(_subDocType1).WithManCo(_manCo3);//.WithGridRun(_gridRun3);
      _document4 = BuildMeA.Document("id4").WithDocType(_docType3).WithSubDocType(_subDocType2).WithManCo(_manCo2);//.WithGridRun(_gridRun4);

      _document5 = BuildMeA.Document("id5").WithDocType(_docType2).WithSubDocType(_subDocType1).WithManCo(_manCo1).WithApproval(_approval1).WithExport(_export);//.WithGridRun(_gridRun);
      _document6 = BuildMeA.Document("id6").WithDocType(_docType3).WithSubDocType(_subDocType2).WithManCo(_manCo3);//.WithGridRun(_gridRun2);
      _document7 = BuildMeA.Document("id7").WithDocType(_docType2).WithSubDocType(_subDocType2).WithManCo(_manCo3).WithApproval(_approval2);//.WithGridRun(_gridRun3);
      _document8 = BuildMeA.Document("id8").WithDocType(_docType2).WithSubDocType(_subDocType2).WithManCo(_manCo3).WithApproval(_approval3);//.WithGridRun(_gridRun4);
      _document9 = BuildMeA.Document("id9").WithDocType(_docType2).WithSubDocType(_subDocType2).WithManCo(_manCo3).WithRejection(_rejection);

      _grid = BuildMeA.GridRun("grid", false, DateTime.Now, DateTime.Now, 1).WithApplication(_application1).WithDocument(_document).WithDocument(_document2).WithDocument(_document3);
      _grid1 = BuildMeA.GridRun("grid1", false, DateTime.Now, DateTime.Now, 1).WithApplication(_application1).WithDocument(_document4).WithDocument(_document5);
      _grid2 = BuildMeA.GridRun("grid2", false, DateTime.Now, DateTime.Now, 1).WithApplication(_application1).WithDocument(_document6).WithDocument(_document7).WithDocument(_document8);

      _gridRun = BuildMeA.GridRun("grid1", true, new DateTime(2014, 3, 1), new DateTime(2014, 3, 1), 3).WithApplication(_application1).WithDocument(_document).WithDocument(_document5).WithDocument(_document6).WithDocument(_document9);
      _gridRun2 = BuildMeA.GridRun("grid2", true, new DateTime(2014, 3, 1), new DateTime(2014, 3, 1), 3).WithApplication(_application1).WithDocument(_document2).WithDocument(_document6);
      _gridRun3 = BuildMeA.GridRun("grid3", true, new DateTime(2014, 3, 1), new DateTime(2014, 3, 1), 3).WithApplication(_application1).WithDocument(_document3).WithDocument(_document7);
      _gridRun4 = BuildMeA.GridRun("grid4", true, new DateTime(2014, 1, 1), new DateTime(2014, 1, 1), 3).WithApplication(_application1).WithDocument(_document4);

      _xmlFileOffShore =
        BuildMeA.XmlFile("documentSetId 1", "fileName 1", "parentFileName 1", true, DateTime.Now, DateTime.Now)
                .WithGridRun(_grid)
                .WithDocType(_docType)
                .WithDomicile(_domicile)
                .WithManCo(_manCo1); ;

      _xmlFileOnShore =
        BuildMeA.XmlFile("documentSetId 2", "fileName 2", "parentFileName 2", false, DateTime.Now, DateTime.Now)
                .WithGridRun(_grid1)
                .WithGridRun(_grid2)
                .WithDocType(_docType)
                .WithDomicile(_domicile)
                .WithManCo(_manCo1); ;
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private DocumentRepository _documentRepository;
    private GridRunRepository _gridRunRepository;
    private XmlFileRepository _xmlFileRepository;
    private HouseHoldingRunRepository _houseHoldingRunRepository;
    private Document _document;
    private Document _document2;
    private Document _document3;
    private Document _document4;
    private Document _document5;
    private Document _document6;
    private Document _document7;
    private Document _document8;
    private Document _document9;
    private DocType _docType;
    private DocType _docType2;
    private DocType _docType3;
    private SubDocType _subDocType1;
    private SubDocType _subDocType2;
    private ManCo _manCo1;
    private ManCo _manCo2;
    private ManCo _manCo3;
    private Domicile _domicile;
    private GridRun _grid;
    private GridRun _grid1;
    private GridRun _grid2;
    private Application _application1;
    private Approval _approval1;
    private Approval _approval2;
    private Approval _approval3;
    private Approval _approval4;
    private Approval _approval5;
    private Export _export;
    private XmlFile _xmlFileOffShore;
    private XmlFile _xmlFileOnShore;
    private HouseHoldingRun _houseHoldingRun;
    private GridRun _gridRun;
    private GridRun _gridRun2;
    private GridRun _gridRun3;
    private GridRun _gridRun4;
    private Rejection _rejection;

    [Test]
    public void GivenDocuments_WhenIAskForKPIReportData_DataIsReturned()
    {
      _gridRunRepository.Create(_gridRun);
      _gridRunRepository.Create(_gridRun2);
      _gridRunRepository.Create(_gridRun3);
      _gridRunRepository.Create(_gridRun4);

      var result = _documentRepository.GetDocuments(_manCo3.Id, new DateTime(2014, 2, 28), new DateTime(2014, 3, 2));

      var query = (from i in result where i.DocType == _docType2.Code && i.SubDocType == _subDocType1.Code select i).Single();
      query.NumberOfDocs.Should().Be(1);

      var query2 = (from i in result where i.DocType == _docType2.Code && i.SubDocType == _subDocType2.Code select i).Single();
      query2.NumberOfDocs.Should().Be(2);

      var query3 = (from i in result where i.DocType == _docType3.Code select i).Single();
      query3.NumberOfDocs.Should().Be(1);
    }

    [Test]
    public void GivenDocuments_WhenIAskForKPIReportDataForDataForTheSameDay_DataIsReturned()
    {
      _gridRunRepository.Create(_gridRun);
      _gridRunRepository.Create(_gridRun2);
      _gridRunRepository.Create(_gridRun3);
      _gridRunRepository.Create(_gridRun4);

      var result = _documentRepository.GetDocuments(_manCo3.Id, new DateTime(2014, 3, 1), new DateTime(2014, 3, 1));

      var query = (from i in result where i.DocType == _docType2.Code && i.SubDocType == _subDocType1.Code select i).Single();
      query.NumberOfDocs.Should().Be(1);

      var query2 = (from i in result where i.DocType == _docType2.Code && i.SubDocType == _subDocType2.Code select i).Single();
      query2.NumberOfDocs.Should().Be(2);

      var query3 = (from i in result where i.DocType == _docType3.Code select i).Single();
      query3.NumberOfDocs.Should().Be(1);
    }

    [Test]
    public void GivenADocument_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _documentRepository.Entities.Count();

      _documentRepository.Create(_document);

      _documentRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenADocument_WhenITryToSearchForItById_ItIsRetrieveFromDatabase()
    {
      _documentRepository.Create(_document);
      var result = _documentRepository.GetDocument(_document.Id);

      result.Should().NotBeNull();
      result.DocumentId.Should().Be(_document.DocumentId);
    }

    [Test]
    public void GivenADocument_WhenITryToSearchForItByDocumentId_ItIsRetrieveFromDatabase()
    {
      _documentRepository.Create(_document);
      var result = _documentRepository.GetDocument(_document.DocumentId);

      result.Should().NotBeNull();
      result.DocumentId.Should().Be(_document.DocumentId);
    }

    [Test]
    public void GivenDocuments_WhenIGetDocuments_AllIsRetrieveFromDatabase()
    {
      _documentRepository.Create(_document);
      _documentRepository.Create(_document2);
      var result = _documentRepository.GetDocuments();

      result.Should().NotBeNull();
      result.Count().Should().BeGreaterOrEqualTo(2);
    }

    [Test]
    public void GivenDocuments_WhenISearchByGrid_IGetTheDocumentsForThatGrid()
    {
      _gridRunRepository.Create(_grid);
      _gridRunRepository.Create(_grid1);
      _gridRunRepository.Create(_grid2);

      var gridRuns = _documentRepository.GetDocuments("grid");
      gridRuns.Should()
              .Contain(d => d.DocumentId == "id")
              .And.Contain(d => d.DocumentId == "id2")
              .And.Contain(d => d.DocumentId == "id3")
              .And.NotContain(d => d.DocumentId == "id4")
              .And.NotContain(d => d.DocumentId == "id5")
              .And.NotContain(d => d.DocumentId == "id6")
              .And.NotContain(d => d.DocumentId == "id7")
              .And.NotContain(d => d.DocumentId == "id8");
    }

    [Test]
    public void GivenDocuments_WhenISearchByGridForDocsWithApprovals_IGetTheDocumentsForThatGrid()
    {
      _gridRunRepository.Create(_grid);
      _gridRunRepository.Create(_grid1);
      _gridRunRepository.Create(_grid2);

      var gridRuns = _documentRepository.GetDocumentsWithApprovalAndRejection("grid");
      gridRuns.Should()
              .Contain(d => d.DocumentId == "id")
              .And.Contain(d => d.Approval != null)
              .And.Contain(d => d.DocumentId == "id2")
              .And.Contain(d => d.DocumentId == "id3")
              .And.NotContain(d => d.DocumentId == "id4")
              .And.NotContain(d => d.DocumentId == "id5")
              .And.NotContain(d => d.DocumentId == "id6")
              .And.NotContain(d => d.DocumentId == "id7")
              .And.NotContain(d => d.DocumentId == "id8");
    }

    [Test]
    public void GivenDocuments_WhenISearchForApprovedAndNonExportedForOffShore_IGetTheCorrectDocuments()
    {
      _xmlFileRepository.Create(_xmlFileOffShore);
      _xmlFileRepository.Create(_xmlFileOnShore);

      var documents = _documentRepository.GetApprovedAndNotExported(true);

      documents.Should().NotContain(d => d.DocumentId == "id7")
              .And.Contain(d => d.DocumentId == "id")
              .And.Contain(d => d.DocumentId == "id2")
              .And.NotContain(d => d.DocumentId == "id3")
              .And.NotContain(d => d.DocumentId == "id4")
              .And.NotContain(d => d.DocumentId == "id5")
              .And.NotContain(d => d.DocumentId == "id6")
              .And.NotContain(d => d.DocumentId == "id8");
    }

    [Test]
    public void GivenDocuments_WhenISearchForApprovedAndNonExportedForOnShore_IGetTheCorrectDocuments()
    {
      _xmlFileRepository.Create(_xmlFileOffShore);
      _xmlFileRepository.Create(_xmlFileOnShore);

      var documents = _documentRepository.GetApprovedAndNotExported(false);

      documents.Should().Contain(d => d.DocumentId == "id7")
              .And.NotContain(d => d.DocumentId == "id")
              .And.NotContain(d => d.DocumentId == "id2")
              .And.NotContain(d => d.DocumentId == "id3")
              .And.NotContain(d => d.DocumentId == "id4")
              .And.NotContain(d => d.DocumentId == "id5")
              .And.NotContain(d => d.DocumentId == "id6")
              .And.Contain(d => d.DocumentId == "id8");
    }

    [Test]
    public void WhenIUpdateADocument_AndTheDocumentDoesNotExist_ThenAnExceptionIsThrown()
    {
      _documentRepository.Create(_document);
      _houseHoldingRunRepository.Create(_houseHoldingRun);

      Document document = _documentRepository.Entities.FirstOrDefault(d => d.DocumentId == "id");

      Action act = () => _documentRepository.Update(_document.Id + 1001, _houseHoldingRun.Id);

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void WhenIUpdateAPost_ThenIPostIsUpdated()
    {
      _documentRepository.Create(_document);
      _houseHoldingRunRepository.Create(_houseHoldingRun);

      Document document = _documentRepository.Entities.FirstOrDefault(d => d.DocumentId == "id");

      _documentRepository.Update(_document.Id, _houseHoldingRun.Id);

      document = _documentRepository.Entities.FirstOrDefault(d => d.Id == document.Id && d.HouseHoldingRunId == this._houseHoldingRun.Id);

      document.Should().NotBeNull();
    }

    [Test]
    public void GivenDocuments_WhenISearchForApprovedDocumentsWithinAGrid_IGetTheCorrectDocuments()
    {
      _gridRunRepository.Create(_gridRun);

      var documents = _documentRepository.GetApprovedDocuments("grid1");

      documents.Should().Contain(d => d.DocumentId == "id")
              .And.NotContain(d => d.DocumentId == "id6")
              .And.Contain(d => d.DocumentId == "id5");
    }

    [Test]
    public void GivenDocuments_WhenISearchForUnApprovedDocumentsWithinAGrid_IGetTheCorrectDocuments()
    {
      _gridRunRepository.Create(_gridRun);

      var documents = _documentRepository.GetUnApprovedDocuments("grid1");

      documents.Should().NotContain(d => d.DocumentId == "id")
              .And.Contain(d => d.DocumentId == "id6")
              .And.NotContain(d => d.DocumentId == "id5")
              .And.NotContain(d => d.DocumentId == "id9");
    }
  }
}
