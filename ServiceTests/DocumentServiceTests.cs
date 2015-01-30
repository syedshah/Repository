namespace ServiceTests
{
  using UnityRepository.Interfaces;
  using System;
  using System.Collections.Generic;
  using BusinessEngineInterfaces;
  using ClientProxies.ArchiveServiceReference;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;

  [TestFixture]
  public class DocumentServiceTests
  {
    private Mock<IDocument> _archiveService;
    private Mock<IDocumentRepository> _documentRepository;
    private Mock<IIndexNameCriteraService> _indexNameCriteraService;
    private Mock<ISearchEngine> _documentEngine;
    private Mock<IDocTypeRepository>_docTypeRepository;
    private Mock<ISubDocTypeRepository> subDocTypeRepository;
    private Mock<IManCoRepository> _manCoRepository;
    private IDocumentService _documentService;
    private Guid _documentId;
    private Guid _documentId2;
    private Guid _documentId3;
    private PagedResult<IndexedDocumentData> _pagedDocuments;
    private DocumentSearchResultsData _documentSearchResultsData;

    [SetUp]
    public void SetUp()
    {
      _archiveService = new Mock<IDocument>();
      _documentRepository = new Mock<IDocumentRepository>();
      _indexNameCriteraService = new Mock<IIndexNameCriteraService>();
      _documentEngine = new Mock<ISearchEngine>();
      _docTypeRepository = new Mock<IDocTypeRepository>();
      subDocTypeRepository = new Mock<ISubDocTypeRepository>();
      _manCoRepository = new Mock<IManCoRepository>();

      _documentService = new DocumentService(
        _archiveService.Object,
        _documentRepository.Object,
        _indexNameCriteraService.Object,
        _documentEngine.Object,
        _docTypeRepository.Object,
        subDocTypeRepository.Object,
        _manCoRepository.Object);

      _documentId = Guid.NewGuid();
      _documentId2 = Guid.NewGuid();
      _documentId3 = Guid.NewGuid();

      _documentSearchResultsData = new DocumentSearchResultsData()
                         {
                           DocumentList = new List<IndexedDocumentData>
                               {
                                 new IndexedDocumentData()
                                   {
                                     ApplicationCode = "NTGENXX",
                                     Id = _documentId,
                                     MappedIndexes = new List<IndexMapped>()
                                         {
                                           new IndexMapped()
                                             {
                                               IndexName = "DocumentType",
                                               IndexValue = "Document Type Value"
                                             },
                                             new IndexMapped()
                                             {
                                               IndexName = "DocumentSubType",
                                               IndexValue = "Document SubType Value"
                                              }
                                         }
                                   },
                                            new IndexedDocumentData()
                                            {
                                              ApplicationCode = "NTGENXX",
                                              Id = _documentId2,
                                              MappedIndexes = new List<IndexMapped>
                                                  {
                                                    new IndexMapped()
                                                      {
                                                        IndexName = "DocumentType2",
                                                        IndexValue = "Document Type Value2"
                                                      },
                                                    new IndexMapped ()
                                                      {
                                                        IndexName = "DocumentSubType2",
                                                        IndexValue = "Document SubType Value2"
                                                      }
                                                  }
                                            }
                                            ,
                                            new IndexedDocumentData()
                                            {
                                              ApplicationCode = "NTGENXX",
                                              Id = _documentId3,
                                              MappedIndexes = new List<IndexMapped>
                                                  {
                                                    new IndexMapped()
                                                      {
                                                        IndexName = "DocumentType3",
                                                        IndexValue = "Document Type Value3"
                                                      },
                                                    new IndexMapped ()
                                                      {
                                                        IndexName = "DocumentSubType3",
                                                        IndexValue = "Document SubType Value3"
                                                      }
                                                  }
                                            }
                               }
                         };

      _archiveService.Setup(
        d =>
        d.DocumentSearch(
          It.IsAny<AuthenticationData>(),
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>()))
                     .Returns(_documentSearchResultsData);

       _pagedDocuments = new PagedResult<IndexedDocumentData>()
                                                                {
                                                                  CurrentPage = 1,
                                                                  EndRow = 10,
                                                                  StartRow = 1,
                                                                  ItemsPerPage = 10,
                                                                  Results =
                                                                    new List<IndexedDocumentData>()
                                                                      {
                                                                        new IndexedDocumentData ()
                                                                          {
                                                                            ApplicationCode = "NTGENXX",
                                                                            Id = _documentId,
                                                                            MappedIndexes = new List<IndexMapped>
                                                                                {
                                                                                  new IndexMapped()
                                                                                    {
                                                                                      IndexName = "DocumentType",
                                                                                      IndexValue = "Document Type Value"
                                                                                    },
                                                                                  new IndexMapped ()
                                                                                    {
                                                                                      IndexName = "DocumentSubType",
                                                                                      IndexValue = "Document SubType Value"
                                                                                    }
                                                                                }
                                                                          },
                                                                          new IndexedDocumentData()
                                                                          {
                                                                            ApplicationCode = "NTGENXX",
                                                                            Id = _documentId2,
                                                                            MappedIndexes = new List<IndexMapped>
                                                                                {
                                                                                  new IndexMapped()
                                                                                    {
                                                                                      IndexName = "DocumentType2",
                                                                                      IndexValue = "Document Type Value2"
                                                                                    },
                                                                                  new IndexMapped ()
                                                                                    {
                                                                                      IndexName = "DocumentSubType2",
                                                                                      IndexValue = "Document SubType Value2"
                                                                                    }
                                                                                }
                                                                          }
                                                                      }
                                                                };

    }

    [Test]
    public void GivenAValidGrid_WhenISearchForDocuments_DocumentsAreReturned()
    {
      _documentEngine.Setup(
        d =>
        d.GetPagedResults(
          It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DocumentSearchResultsData>(), false))
                     .Returns(_pagedDocuments);

      var documents = _documentService.GetDocuments(It.IsAny<int>(), It.IsAny<int>(), "grid", false, false, false);
      documents.Results.Should().HaveCount(2);

      documents.Results[0].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentType");
      documents.Results[0].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentSubType");
      documents.Results[0].MappedIndexes.Should().Contain(d => d.IndexValue == "Document Type Value");
      documents.Results[0].MappedIndexes.Should().Contain(d => d.IndexValue == "Document SubType Value");

      documents.Results[1].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentType2");
      documents.Results[1].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentSubType2");
      documents.Results[1].MappedIndexes.Should().Contain(d => d.IndexValue == "Document Type Value2");
      documents.Results[1].MappedIndexes.Should().Contain(d => d.IndexValue == "Document SubType Value2");
    }

    [Test]
    public void GivenAValidGrid_WhenISearchForDocuments_AndFilteringIsRequested_DocumentsAreReturned()
    {
      _documentEngine.Setup(
        d =>
        d.GetPagedResults(
          It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DocumentSearchResultsData>(), true))
                     .Returns(_pagedDocuments);

      _documentRepository.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(new Document());
      var documents = _documentService.GetDocuments(It.IsAny<int>(), It.IsAny<int>(), "grid", true, true, true);

      documents.Results.Should().HaveCount(2);

      _documentRepository.Verify(d => d.GetDocument(It.IsAny<string>()), Times.Exactly(3));
    }

    [Test]
    public void GivenAValidGrid_WhenISearchForDocuments_AndIFilterByHouseHeld_HouseHeldDocumentsAreReturned()
    {
      _documentRepository.Setup(d => d.GetDocument(_documentId.ToString())).Returns(new Document());
      _documentRepository.Setup(d => d.GetDocument(_documentId2.ToString())).Returns(new Document
                                                                                       {
                                                                                         HouseHold = new HouseHold()
                                                                                       });
      _documentRepository.Setup(d => d.GetDocument(_documentId3.ToString())).Returns(new Document());

      _documentService.FilterHouseHolding(_documentSearchResultsData);

      _documentSearchResultsData.DocumentList.Should().HaveCount(1);

      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentType2");
      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentSubType2");
      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexValue == "Document Type Value2");
      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexValue == "Document SubType Value2");
    }

    [Test]
    public void GivenAValidGrid_WhenISearchForDocuments_AndIFilterByApprovedAndRejected_ApprovedAndRejectedDocumentsAreRemoved()
    {
      _documentRepository.Setup(d => d.GetDocumentsWithApprovalAndRejection("grid"))
                         .Returns(
                           new List<Document>
                             {
                               new Document()
                                 {
                                   DocumentId = _documentId.ToString(),
                                   Approval = new Approval()
                                 },
                               new Document()
                                 {
                                   DocumentId = _documentId2.ToString()
                                 },
                                 new Document()
                                   {
                                     DocumentId = _documentId3.ToString(),
                                     Rejection = new Rejection()
                                   }
                             });

      _documentService.FilterApprovedAndRejected(_documentSearchResultsData, "grid");

      _documentSearchResultsData.DocumentList.Should().HaveCount(1);

      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentType2");
      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentSubType2");
      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexValue == "Document Type Value2");
      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexValue == "Document SubType Value2");
    }

    [Test]
    public void GivenAValidGrid_WhenISearchForDocuments_AndIFilterByUnApproved_ApprovedDocumentsAreRemoved()
    {
      _documentRepository.Setup(d => d.GetDocumentsWithApprovalAndRejection("grid"))
                         .Returns(
                           new List<Document>
                             {
                               new Document()
                                 {
                                   DocumentId = _documentId.ToString(),
                                   Approval = new Approval()
                                 },
                               new Document()
                                 {
                                   DocumentId = _documentId2.ToString()
                                 },
                                 new Document()
                                 {
                                   DocumentId = _documentId3.ToString(),
                                   Rejection = new Rejection()
                                 }
                             });

      _documentService.FilterUnapproved(_documentSearchResultsData, "grid");

      _documentSearchResultsData.DocumentList.Should().HaveCount(1);

      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentType");
      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexName == "DocumentSubType");
      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexValue == "Document Type Value");
      _documentSearchResultsData.DocumentList[0].MappedIndexes.Should().Contain(d => d.IndexValue == "Document SubType Value");
    }

    [Test]
    public void GivenAValidGrid_WhenISearchForDocuments_AndTheArchiveWebServiceIsNotAvailable_ThenAUnityExceptionIsThrown()
    {
      _archiveService.Setup(
        d =>
        d.DocumentSearch(
          It.IsAny<AuthenticationData>(),
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>(),
          It.IsAny<string>())).Throws<Exception>();

      Action act = () => _documentService.GetDocuments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidSearchCriteria_WhenISearchForDocuments_ThenDocumentsAreReturned()
    {
      var returnData = new DocumentSearchResultsIndexedData
                         {
                           DocumentList = new List<IndexedDocumentData>
                                            {
                                            }
                         };

      _archiveService.Setup(d => d.DocumentSearchMapVerbose(It.IsAny<AuthenticationData>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<List<IndexNameCriteriaData>>()))
                             .Returns(returnData);

      _documentEngine.Setup(
        d =>
        d.GetPagedResults(
          It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DocumentSearchResultsIndexedData>()))
                     .Returns(_pagedDocuments);


      var listManCoTexts = new List<string>();
      listManCoTexts.Add("manco1");
      listManCoTexts.Add("manco2");

      var documents = _documentService.GetDocuments(
        1,
        10,
        "doctype",
        "subdoctype",
        "addresseeSubType",
        "accountNumber",
        "mailingName",
        listManCoTexts,
        "investorRef",
        new DateTime(2014, 1, 13, 13, 10, 22),
        new DateTime(2014, 1, 14, 13, 10, 22),
        "primaryHolder",
        "agentReference",
        "addressPoseCode",
        "emailAddress",
        "faxNumber",
        new DateTime(2014, 1, 13, 13, 10, 22),
        new DateTime(2014, 1, 12, 13, 10, 22),
        "documentNumber");
      documents.Should().NotBeNull();
    }

    [Test]
    public void GivenValidSearchCriteria_WhenISearchForDocuments_AndTheWCFServiceIsUnavailable_ThenDocumentsAreReturned()
    {
      _archiveService.Setup(d => d.DocumentSearchMapVerbose(It.IsAny<AuthenticationData>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<List<IndexNameCriteriaData>>())).Throws<Exception>();

      var listManCoTexts = new List<string>();
      listManCoTexts.Add("manco1");
      listManCoTexts.Add("manco2");

      Action act =
        () =>
        _documentService.GetDocuments(
          1,
          10,
          "doctype",
          "subdoctype",
          "addresseeSubType",
          "accountNumber",
          "mailingName",
          listManCoTexts,
          "investorRef",
          new DateTime(2014, 1, 13, 13, 10, 22),
          new DateTime(2014, 1, 14, 13, 10, 22),
          "primaryHolder",
          "agentReference",
          "addressPoseCode",
          "emailAddress",
          "faxNumber",
          new DateTime(2014, 1, 13, 13, 10, 22),
          new DateTime(2014, 1, 12, 13, 10, 22),
          "documentNumber");

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenADocumentId_WhenADocumentIsRequested_AndTheWCFServiceIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _archiveService.Setup(d => d.GetDocument(It.IsAny<AuthenticationData>(), It.IsAny<string>())).Throws<Exception>();

      Action act = () => _documentService.GetDocumentStream(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenADocumentId_WhenADocumentIsRequested_ThenTheDocumentIsReturned()
    {
      _archiveService.Setup(d => d.GetDocument(It.IsAny<AuthenticationData>(), It.IsAny<string>())).Returns(new byte[] { });
      var result = _documentService.GetDocumentStream(It.IsAny<string>());

      result.Should().NotBeNull();
    }

    [Test]
    public void GivenADocument_WhenDocumentIsAdded_ItIsSaved()
    {
        _documentService.AddDocument(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>());
      _documentRepository.Verify(s => s.Create(It.IsAny<Document>()), Times.Once());
    }

    [Test]
    public void GivenADocument_WhenDocumentIsAdded_AndDocumentRepositoryCannotCreateDocument_UsingCodeValues_ThenAUnityExceptionIsThrown()
    {
        _documentRepository.Setup(d => d.Create(It.IsAny<Document>())).Throws<Exception>();
        Assert.Throws<UnityException>(() => _documentService.AddDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()));
    }

    [Test]
    public void GivenADocument_WhenDocumentIsAdded_UsingCodeValues_ItIsSaved()
    {
      _docTypeRepository.Setup(d => d.GetDocType(It.IsAny<string>())).Returns(new DocType() { Id = 1 });
      _manCoRepository.Setup(d => d.GetManCo(It.IsAny<string>())).Returns(new ManCo() { Id = 1 });
      subDocTypeRepository.Setup(d => d.GetSubDocType(It.IsAny<string>())).Returns(new SubDocType() { Id = 1 });

      _documentService.AddDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>());
      _documentRepository.Verify(s => s.Create(It.IsAny<Document>()), Times.Once());
    }

    [Test]
    public void GivenADocument_WhenDocumentIsAdded_AndDocumentRepositoryCannotCreateDocument_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(d => d.Create(It.IsAny<Document>())).Throws<Exception>();
      Assert.Throws<UnityException>(() => _documentService.AddDocument(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));
    }

    [Test]
    public void GivenADocumentId_WhenADocumentIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(a => a.GetDocument((It.IsAny<string>()))).Throws<Exception>();

      Action act = () => _documentService.GetDocument(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenADocumentId_WhenADocumentIsRequested_ThenADocumentIsReturned()
    {
      _documentRepository.Setup(a => a.GetDocument(It.IsAny<string>())).Returns(new Document());
      Document doctpye = _documentService.GetDocument(It.IsAny<string>());

      doctpye.Should().NotBeNull();
    }

    [Test]
    public void GivenAGrid_WhenDocumentsAreRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(a => a.GetDocuments((It.IsAny<string>()))).Throws<Exception>();

      Action act = () => _documentService.GetDocuments(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAGrid_WhenDocumentAreRequested_ThenADocumentDocumentsAreReturned()
    {
      _documentRepository.Setup(a => a.GetDocuments(It.IsAny<string>())).Returns(new List<Document>
                                                                                   {
                                                                                     new Document()
                                                                                   } );
      var documents = _documentService.GetDocuments(It.IsAny<string>());

      documents.Should().NotBeNull().And.HaveCount(1);
    }

        [Test]
    public void GivenAGrid_WhenDocumentsWithApprovalsAreRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(a => a.GetDocuments((It.IsAny<string>()))).Throws<Exception>();

      Action act = () => _documentService.GetDocuments(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAGrid_WhenDocumentWithApprovalsAreRequested_ThenADocumentDocumentsAreReturned()
    {
      _documentRepository.Setup(a => a.GetDocumentsWithApprovalAndRejection(It.IsAny<string>())).Returns(new List<Document>
                                                                                   {
                                                                                     new Document()
                                                                                   } );
      var documents = _documentService.GetDocumentsWithApprovalAndRejection(It.IsAny<string>());

      documents.Should().NotBeNull().And.HaveCount(1);
    }

    [Test]
    public void GivenAManCoId_WhenDocumentsAreRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(a => a.GetDocuments(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws<Exception>();

      Action act = () => _documentService.GetDocuments(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAManCoId_WhenDocumentsAreRequested_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(a => a.GetDocuments(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<KpiReportData>
                                                                                   {
                                                                                     new KpiReportData()
                                                                                   });
      var documents = _documentService.GetDocuments(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());

      documents.Should().NotBeNull().And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_WhenApprovedAndNonExportedDocumentsAreRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(a => a.GetApprovedAndNotExported(It.IsAny<bool>())).Throws<Exception>();

      Action act = () => _documentService.GetApprovedAndNotExported(It.IsAny<bool>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenApprovedAndNonExportedDocumentsAreRequested_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(a => a.GetApprovedAndNotExported(It.IsAny<bool>())).Returns(new List<Document>
                                                                                   {
                                                                                     new Document()
                                                                                   });
      var documents = _documentService.GetApprovedAndNotExported(It.IsAny<bool>());

      documents.Should().NotBeNull().And.HaveCount(1);
    }

    [Test]
    public void GivenValidData_AndAnUnavailableDatabase_WhenIUpdateADocument_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(p => p.Update(It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();

      Action act = () => _documentService.Update(It.IsAny<int>(), It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenIUpdateADocument_ThenTheDocumentIsUpdated()
    {
      _documentService.Update(It.IsAny<int>(), It.IsAny<int>());
      _documentRepository.Verify(d => d.Update(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
    }

    [Test]
    public void GivenAGrid_WhenApprovedDocumentsAreRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(a => a.GetApprovedDocuments((It.IsAny<string>()))).Throws<Exception>();

      Action act = () => _documentService.GetApprovedDocuments(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAGrid_WhenApprovedDocumentAreRequested_ThenADocumentDocumentsAreReturned()
    {
      _documentRepository.Setup(a => a.GetApprovedDocuments(It.IsAny<string>())).Returns(new List<Document>
                                                                                   {
                                                                                     new Document()
                                                                                   });
      var documents = _documentService.GetApprovedDocuments(It.IsAny<string>());

      documents.Should().NotBeNull().And.HaveCount(1);
    }

    [Test]
    public void GivenAGrid_WhenUnApprovedDocumentsAreRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _documentRepository.Setup(a => a.GetUnApprovedDocuments((It.IsAny<string>()))).Throws<Exception>();

      Action act = () => _documentService.GetUnApprovedDocuments(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAGrid_WhenUnApprovedDocumentAreRequested_ThenADocumentDocumentsAreReturned()
    {
      _documentRepository.Setup(a => a.GetUnApprovedDocuments(It.IsAny<string>())).Returns(new List<Document>
                                                                                   {
                                                                                     new Document()
                                                                                   });
      var documents = _documentService.GetUnApprovedDocuments(It.IsAny<string>());

      documents.Should().NotBeNull().And.HaveCount(1);
    }
  }
}
