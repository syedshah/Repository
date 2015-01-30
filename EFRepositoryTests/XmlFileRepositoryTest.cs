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
  [TestFixture]
  public class XmlFileRepositoryTest
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _xmlFileRepository = new XmlFileRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _manCo1 = BuildMeA.ManCo("description", "name");
      _manCo2 = BuildMeA.ManCo("description2", "name2");
      _manCo3 = BuildMeA.ManCo("description3", "name3");
      _docType1 = BuildMeA.DocType("code", "description");
      _domicile1 = BuildMeA.Domicile("code", "description");
      _gridRun1 = BuildMeA.GridRun("grid", false, DateTime.Now, null, 0);

      _xmlFile1 =
        BuildMeA.XmlFile("documentSetId", "file1.xml", "littleZip.xml", false, DateTime.Now, DateTime.Now)
                .WithDocType(_docType1)
                .WithManCo(_manCo1)
                .WithDomicile(_domicile1);

      _xmlFile2 =
        BuildMeA.XmlFile("documentSetId", "file2.xml", "littleZip.xml", false, DateTime.Now, DateTime.Now)
                .WithDocType(_docType1)
                .WithManCo(_manCo1)
                .WithDomicile(_domicile1);

      _xmlFile3 =
        BuildMeA.XmlFile("documentSetId", "name3.xml", "littleZip.xml", false, DateTime.Now, DateTime.Now)
                .WithDocType(_docType1)
                .WithManCo(_manCo1)
                .WithDomicile(_domicile1);
    }

    [TearDown]
    public void TearDown()
    {
     _transactionScope.Dispose();
    }

    private ManCo _manCo1;
    private ManCo _manCo2;
    private ManCo _manCo3;
    private DocType _docType1;
    private Domicile _domicile1;
    private GridRun _gridRun1;
    private XmlFile _xmlFile1;
    private XmlFile _xmlFile2;
    private XmlFile _xmlFile3;
    private XmlFile _xmlFile4;
    private XmlFile _xmlFile5;
    private XmlFile _xmlFile6;
    private TransactionScope _transactionScope;
    private XmlFileRepository _xmlFileRepository;

    [Test]
    public void GivenAnXmlFile_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _xmlFileRepository.Entities.Count();
      _xmlFileRepository.Create(_xmlFile1);
      _xmlFileRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenAXmlFile_WhenISearchByFileName_IRetrieveTheXmlFile()
    {
      _xmlFileRepository.Create(_xmlFile1);
      var result = _xmlFileRepository.GetFile("file1.xml");
      result.FileName.Should().Be("file1.xml");
    }

    [Test]
    public void GivenAXmlFile_WhenISearchByFileName_AndThereIsAlreadyAXMLFileWithTheSameName_IRetrieveTheLatestVersionOfTheXmlFile()
    {
      _xmlFileRepository.Create(_xmlFile1);

      DateTime receivedDate = DateTime.Now;

      _xmlFile4 =
        BuildMeA.XmlFile("documentSetId", "file1.xml", "littleZip.xml", false, receivedDate, DateTime.Now)
                .WithDocType(_docType1)
                .WithManCo(_manCo1)
                .WithDomicile(_domicile1);

      _xmlFileRepository.Create(_xmlFile4);

      var result = _xmlFileRepository.GetFile("file1.xml");
      
      result.FileName.Should().Be("file1.xml");
      result.Received.Should().Be(receivedDate);
    }
    
    [Test]
    public void GivenXmlFileData_WhenISearchForXmlFilesUsingAWildCard_TheXmlFileDataIsReturned()
    {
      _xmlFileRepository.Create(_xmlFile1);
      _xmlFileRepository.Create(_xmlFile2);
      _xmlFileRepository.Create(_xmlFile3);

      var xmlFiles = _xmlFileRepository.Search("file");

      xmlFiles.Should().HaveCount(2)
        .And.Contain(f => f.FileName == "file1.xml")
        .And.Contain(f => f.FileName == "file2.xml")
        .And.NotContain(f => f.FileName == "name3.xml");
    }

    [Test]
    public void GivenXmlFileData_WhenISearchUsingWildCardAndManCoIds_ThenXMLFileDataIsReturned()
    {
       _xmlFile5 =
       BuildMeA.XmlFile("documentSetId", "name5.xml", "littleZip.xml", false, DateTime.Now, DateTime.Now)
               .WithDocType(_docType1)
               .WithManCo(_manCo2)
               .WithDomicile(_domicile1);

       _xmlFile6 =
       BuildMeA.XmlFile("documentSetId", "name6.xml", "littleZip.xml", false, DateTime.Now, DateTime.Now)
               .WithDocType(_docType1)
               .WithManCo(_manCo3)
               .WithDomicile(_domicile1);

       _xmlFileRepository.Create(_xmlFile1);
       _xmlFileRepository.Create(_xmlFile5);
       _xmlFileRepository.Create(_xmlFile6);

       var listManCoId = new List<int>();

       listManCoId.Add(_manCo1.Id);
       listManCoId.Add(_manCo2.Id);

       var xmlFiles = _xmlFileRepository.Search("", listManCoId);

       xmlFiles.Should().NotBeNull();
       xmlFiles.Should().HaveCount(2)
        .And.Contain(f => f.FileName == "file1.xml")
        .And.Contain(f => f.FileName == "name5.xml")
        .And.NotContain(f => f.FileName == "name6.xml");
    }
  }
}