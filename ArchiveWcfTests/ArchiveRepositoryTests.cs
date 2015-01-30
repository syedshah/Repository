namespace ArchiveWcfTests
{
  using System.Collections.Generic;
  using System.Transactions;
  using AbstractConfigurationManager;
  using ArchiveServiceFactory.ArchiveService;
  using ArchiveServiceFactory.Interfaces;
  using ArchiveWCFRepository.Interface;
  using ArchiveWCFRepository.Repositories;
  using FluentAssertions;
  using IntegrationTestUtils;
  using NUnit.Framework;

  [TestFixture]
  public class ArchiveRepositoryTests : IoCSupportedTest
  {
    private TransactionScope _transactionScope;
    private IConfigurationManager _configurationManager;
    private IDocumentServiceFactory _documentserviceFactory;
    private IArchiveRepository _archiveRepository;
    private string _grid;
    private string _accountNumber;
    private string _docType1;
    private string _docType2;
    private string _subDocType;
    private string _documentId;
    private string _mailingName;
    private string _managementCompanyId;
    private string _investorReference;
    private List<IndexNameCriteriaData> _searchCriteria;
    private int _pageNumber;
    private int _numberOfItems;

    [SetUp]
    public void SetUp()
    {
      _transactionScope = new TransactionScope();
      _documentserviceFactory = Resolve<IDocumentServiceFactory>();
      _configurationManager = Resolve<IConfigurationManager>();

      _archiveRepository = new ArchiveRepository(_documentserviceFactory, _configurationManager);

      _grid = "132428-113348X-DZXW";
      _accountNumber = "6012 2652";
      _docType1 = "stm";
      _docType2 = "ltr";
      _subDocType = "trn";
      _documentId = "1708F960-F5FE-43E9-BFA3-4035E33FF5F1";
      _mailingName = "Miss Susan Bridget Millington";
      _managementCompanyId = "022";
      _investorReference = "8011 6548";
      _pageNumber = 1;
      _numberOfItems = 10;
    }

    [TearDown]
    public void TearDown()
    {
      _configurationManager = null;
      _documentserviceFactory = null;
      _transactionScope.Dispose();
    }

  /*  [Test]
    public void GivenAValidGrid_WhenICallTheGetDocumentsMethod_IGetDocuments()
    {
      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _grid);
      data.Results.Should().HaveCount(c => c > 0);
    }

    [Test]
    public void GivenAValidAccountNumber_WhenICallTheGetDocumentsMethod_IGetDocuments()
    {
      _searchCriteria = new List<IndexNameCriteriaData>
                          {
                            new IndexNameCriteriaData
                              {
                                IndexName = "accountnumber",
                                SearchValue = _accountNumber
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should().HaveCount(c => c > 0)
        .And.OnlyContain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(_accountNumber.ToLower()));
      data.ItemsPerPage.Should().Be(10);
      data.TotalItems.Should().BeGreaterThan(0);
    }

    [Test]
    public void GivenADocumentType_WhenICallTheGetDocumentsMethod_IGetDocuments()
    {
      _searchCriteria = new List<IndexNameCriteriaData>
                          {
                            new IndexNameCriteriaData
                              {
                                IndexName = "type",
                                SearchValue = _docType2
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should().HaveCount(c => c > 0)
        .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(_docType2.ToLower()));
    }

    [Test]
    public void GivenADocumentTypeThatHasResultsOverSearchLimit_WhenICallTheGetDocumentsMethod_IGetDocuments()
    {
      _searchCriteria = new List<IndexNameCriteriaData>
                          {
                            new IndexNameCriteriaData
                              {
                                IndexName = "type",
                                SearchValue = _docType1
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should().HaveCount(c => c == 0);
      data.TotalItems.Should().BeGreaterThan(5000);
    }

    [Test]
    public void GivenADocumentSubType_WhenICallTheGetDocumentsMethod_IGetDocuments()
    {
      _searchCriteria = new List<IndexNameCriteriaData>
                          {
                            new IndexNameCriteriaData
                              {
                                IndexName = "subtype",
                                SearchValue = _subDocType
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should().HaveCount(c => c > 0)
        .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(_subDocType.ToLower()));
      data.TotalItems.Should().BeGreaterThan(0);
    }

    [Test]
    public void GivenAMailingName_WhenICallTheGetDocumentsMethod_IGetDocuments()
    {
      _searchCriteria = new List<IndexNameCriteriaData>()
                          {
                            new IndexNameCriteriaData()
                              {
                                IndexName = "mailingname",
                                SearchValue = _mailingName
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should().HaveCount(c => c > 0)
        .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(_mailingName.ToLower()));
      data.TotalItems.Should().BeGreaterThan(0);
    }

    [Test]
    public void GivenAPartialMailingName_WhenICallTheGetDocumentsMethod_IGetDocumentsViaTheWildCardSearch()
    {
      _searchCriteria = new List<IndexNameCriteriaData>()
                          {
                            new IndexNameCriteriaData()
                              {
                                IndexName = "mailingname",
                                SearchValue = "*Susan Bridget Millington*"
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should().HaveCount(c => c > 0)
        .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(_mailingName.ToLower()));
      data.TotalItems.Should().BeGreaterThan(0);
    }

    [Test]
    public void GivenAManCo_WhenICallTheGetDocumentsMethod_IGetDocuments()
    {
      _searchCriteria = new List<IndexNameCriteriaData>()
                          {
                            new IndexNameCriteriaData()
                              {
                                IndexName = "ManagementCompanyID",
                                SearchValue = _managementCompanyId
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should().HaveCount(c => c > 0)
        .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(_managementCompanyId.ToLower()));
    }

    [Test]
    public void GivenAmInvestorReferenceManCo_WhenICallTheGetDocumentsMethod_IGetDocuments()
    {
      _searchCriteria = new List<IndexNameCriteriaData>()
                          {
                            new IndexNameCriteriaData()
                              {
                                IndexName = "investorreference",
                                SearchValue = _investorReference
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should().HaveCount(c => c > 0)
        .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(_investorReference.ToLower()));
      data.TotalItems.Should().BeGreaterThan(0);
    }

    [Test]
    public void GivenAnAccountNumber_WhenICallTheGetDocumentsMethod_IGetDocuments()
    {
      _searchCriteria = new List<IndexNameCriteriaData>()
                          {
                            new IndexNameCriteriaData()
                              {
                                IndexName = "accountnumber",
                                SearchValue = _accountNumber
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should().HaveCount(c => c > 0)
        .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(_accountNumber.ToLower()));
      data.TotalItems.Should().BeGreaterThan(0);
    }

    [Test]
    public void GivenMultipleSearchCriteria_WhenICallTheGetDocumetsMethod_IGetDocuments()
    {
      const string AccountNumber = "6012 2652";
      const string DocType = "stm";
      const string SubDocType = "val";

      _searchCriteria = new List<IndexNameCriteriaData>
                          {
                            new IndexNameCriteriaData
                              {
                                IndexName = "accountnumber",
                                SearchValue = AccountNumber
                              },
                            new IndexNameCriteriaData
                              {
                                IndexName = "type",
                                SearchValue = DocType
                              },
                            new IndexNameCriteriaData
                              {
                                IndexName = "subtype",
                                SearchValue = SubDocType
                              }
                          };

      var data = _archiveRepository.GetDocuments(_pageNumber, _numberOfItems, _searchCriteria);
      data.Results.Should()
          .HaveCount(c => c > 0)
          .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(AccountNumber.ToLower()))
          .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(DocType.ToLower()))
          .And.Contain(c => c.Indexes.ConvertAll(i => i.ToLower()).Contains(SubDocType.ToLower()));
      data.TotalItems.Should().BeGreaterThan(0);
    }

    [Test]
    public void GivenAValidDocumentId_WhenICallTheGetDocumentMethod_IGetTheDocument()
    {
      var data = _archiveRepository.GetDocument(_documentId);
      data.Should().NotBeNull();
    }*/
  }
}
