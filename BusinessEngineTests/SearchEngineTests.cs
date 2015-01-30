namespace BusinessEngineTests
{
  using System.Collections.Generic;
  using BusinessEngineInterfaces;
  using BusinessEngines;
  using ClientProxies.ArchiveServiceReference;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  [TestFixture]
  public class SearchEngineTests
  {
    private Mock<IDocumentService> _documentService;
    private ISearchEngine _documentEngine;

    private DocumentSearchResultsData _data = new DocumentSearchResultsData()
    {
      DocumentsFoundOverLimit = 0,
      MoreRecords = 0,
      RecordsFound = 36,
      DocumentList = new List<IndexedDocumentData>()
                                               {
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                               }
    };

    private DocumentSearchResultsIndexedData _indexedData = new DocumentSearchResultsIndexedData()
    {
      DocumentsFoundOverLimit = 0,
      MoreRecords = 0,
      RecordsFound = 36,
      DocumentList = new List<IndexedDocumentData>()
                                               {
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                               }
    };

    [SetUp]
    public void SetUp()
    {
      _documentService = new Mock<IDocumentService>();
      _documentEngine = new SearchEngine();
    }

    [Test]
    public void GivenAPageNumberAndPageSize_WhenIAskToGetTheStartAndEndPageNumberForPageOneAndPageSizeTen_IGetTheCorrectStartAndEndPageNumbers()
    {
      const int PageNumber = 1;
      const int PageSize = 10;
      int startRow;
      int endRow;

      _documentEngine.GetStartEndRow(PageNumber, PageSize, out startRow, out endRow);

      startRow.Should().Be(1);
      endRow.Should().Be(10);
    }

    [Test]
    public void GivenAPageNumberAndPageSize_WhenIAskToGetTheStartAndEndPageNumberForPageTwoAndPageSizeTen_IGetTheCorrectStartAndEndPageNumbers()
    {
      int pageNumber = 2;
      int pageSize = 10;
      int startRow;
      int endRow;

      _documentEngine.GetStartEndRow(pageNumber, pageSize, out startRow, out endRow);

      startRow.Should().Be(11);
      endRow.Should().Be(20);
    }

    [Test]
    public void GivenAPageNumberAndPageSize_WhenIAskToGetTheStartAndEndPageNumberForPageThreeAndPageSizeTen_IGetTheCorrectStartAndEndPageNumbers ()
    {
      const int PageNumber = 3;
      const int PageSize = 10;
      int startRow;
      int endRow;

      _documentEngine.GetStartEndRow(PageNumber, PageSize, out startRow, out endRow);

      startRow.Should().Be(21);
      endRow.Should().Be(30);
    }

    [Test]
    public void GivenAPageNumberAndPageSize_WhenIAskToGetTheStartAndEndPageNumberForPageFourAndPageSizeTwenty_IGetTheCorrectStartAndEndPageNumbers ()
    {
      const int PageNumber = 3;
      const int PageSize = 20;
      int startRow;
      int endRow;

      _documentEngine.GetStartEndRow(PageNumber, PageSize, out startRow, out endRow);

      startRow.Should().Be(41);
      endRow.Should().Be(60);
    }

    [Test]
    public void GivenDocumentSearchResultsData_WhenIAskForTheFirstPaginatedResults_IGetCorrectPaginatedResults()
    {
      const int startRow = 1;
      const int endRow = 10;
      const int PageNumber = 1;
      const int ItemsPerPage = 10;

      var pagedResult = _documentEngine.GetPagedResults(startRow, endRow, PageNumber, ItemsPerPage, _data, false);

      pagedResult.StartRow.Should().Be(1);
      pagedResult.EndRow.Should().Be(10);
      pagedResult.ItemsPerPage.Should().Be(10);
      pagedResult.Results.Should().HaveCount(10);
    }

    [Test]
    public void GivenDocumentSearchResultsData_WhenIAskForTheFirstPaginatedResults_AndResultsAreForHouseHolding_IGetCorrectPaginatedResults()
    {
      const int startRow = 1;
      const int endRow = 10;
      const int PageNumber = 1;
      const int ItemsPerPage = 10;

      var pagedResult = _documentEngine.GetPagedResults(startRow, endRow, PageNumber, ItemsPerPage, _data, true);

      pagedResult.StartRow.Should().Be(1);
      pagedResult.EndRow.Should().Be(10);
      pagedResult.ItemsPerPage.Should().Be(10);
      pagedResult.Results.Should().HaveCount(10);
      pagedResult.TotalItems.Should().Be(10);
    }

    [Test]
    public void GivenDocumentSearchResultsData_WhenIAskForTheSecondPaginatedResults_IGetCorrectPaginatedResults()
    {
      int startRow = 11;
      int endRow = 20;
      int pageNumber = 2;
      int itemsPerPage = 10;

      var pagedResult = _documentEngine.GetPagedResults(startRow, endRow, pageNumber, itemsPerPage, _data, false);

      pagedResult.StartRow.Should().Be(11);
      pagedResult.EndRow.Should().Be(20);
      pagedResult.ItemsPerPage.Should().Be(10);
      pagedResult.Results.Should().HaveCount(10);
    }

    [Test]
    public void GivenDocumentSearchResultsData_WhenIAskForTheLastPage_IGetCorrectPaginatedResults()
    {
      DocumentSearchResultsData data = new DocumentSearchResultsData()
    {
      DocumentsFoundOverLimit = 0,
      MoreRecords = 0,
      RecordsFound = 36,
      DocumentList = new List<IndexedDocumentData>()
                                               {
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { }
                                               }
    };

      const int StartRow = 21;
      const int EndRow = 30;
      const int PageNumber = 3;
      const int ItemsPerPage = 10;

      var pagedResult = _documentEngine.GetPagedResults(StartRow, EndRow, PageNumber, ItemsPerPage, data, false);

      pagedResult.StartRow.Should().Be(21);
      pagedResult.EndRow.Should().Be(26);
      pagedResult.ItemsPerPage.Should().Be(10);
      pagedResult.Results.Should().HaveCount(6);
    }

    [Test]
    public void GivenDocumentSearchIndexedResultsData_WhenIAskForTheFirstPaginatedResults_IGetCorrectPaginatedResults()
    {
      const int StartRow = 1;
      const int EndRow = 10;
      const int PageNumber = 1;
      const int ItemsPerPage = 10;

      var pagedResult = _documentEngine.GetPagedResults(StartRow, EndRow, PageNumber, ItemsPerPage, _indexedData);

      pagedResult.StartRow.Should().Be(1);
      pagedResult.EndRow.Should().Be(10);
      pagedResult.ItemsPerPage.Should().Be(10);
      pagedResult.Results.Should().HaveCount(10);
    }

    [Test]
    public void GivenDocumentSearchIndexedResultsData_WhenIAskForTheSecondPaginatedResults_IGetCorrectPaginatedResults()
    {
      const int StartRow = 11;
      const int EndRow = 20;
      const int PageNumber = 2;
      const int ItemsPerPage = 10;

      var pagedResult = _documentEngine.GetPagedResults(StartRow, EndRow, PageNumber, ItemsPerPage, _indexedData);

      pagedResult.StartRow.Should().Be(11);
      pagedResult.EndRow.Should().Be(20);
      pagedResult.ItemsPerPage.Should().Be(10);
      pagedResult.Results.Should().HaveCount(10);
    }

    [Test]
    public void GivenDocumentSearchIndexedResultsData_WhenIAskForTheLastPage_IGetCorrectPaginatedResults()
    {
      DocumentSearchResultsData data = new DocumentSearchResultsData()
      {
        DocumentsFoundOverLimit = 0,
        MoreRecords = 0,
        RecordsFound = 36,
        DocumentList = new List<IndexedDocumentData>()
                                               {
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { }
                                               }
      };

      const int StartRow = 21;
      const int EndRow = 30;
      const int PageNumber = 3;
      const int ItemsPerPage = 10;

      var pagedResult = _documentEngine.GetPagedResults(StartRow, EndRow, PageNumber, ItemsPerPage, data, false);

      pagedResult.StartRow.Should().Be(21);
      pagedResult.EndRow.Should().Be(26);
      pagedResult.ItemsPerPage.Should().Be(10);
      pagedResult.Results.Should().HaveCount(6);
    }

    [Test]
    public void GivenDocumentSearchIndexedResultsData_WhenIAskForTheLastPage_AndFilteringHasBeenApplied_IGetCorrectPaginatedResults()
    {
      DocumentSearchResultsData data = new DocumentSearchResultsData()
      {
        DocumentsFoundOverLimit = 0,
        MoreRecords = 0,
        RecordsFound = 36,
        DocumentList = new List<IndexedDocumentData>()
                                               {
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { },
                                                 new IndexedDocumentData() { }
                                               }
      };

      const int StartRow = 5;
      const int EndRow = 6;
      const int PageNumber = 3;
      const int ItemsPerPage = 2;

      var pagedResult = _documentEngine.GetPagedResults(StartRow, EndRow, PageNumber, ItemsPerPage, data, true);

      pagedResult.StartRow.Should().Be(5);
      //pagedResult.EndRow.Should().Be(6);
      pagedResult.ItemsPerPage.Should().Be(2);
      pagedResult.Results.Should().HaveCount(2);
    }
  }
} 