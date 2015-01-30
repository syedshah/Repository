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
  public class NewsTickerRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _newsTickerRepository = new NewsTickerRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _newsTicker1 = BuildMeA.NewsTicker("test news",DateTime.Now);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private NewsTickerRepository _newsTickerRepository;
    private NewsTicker _newsTicker1;

    [Test]
    public void GivenANewsTicker_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _newsTickerRepository.Entities.Count();
      _newsTickerRepository.Create(_newsTicker1);
      _newsTickerRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenANewsTicker_WhenITryToSearchForIt_ItIsRetrievedFromTheDatabase()
    {
      _newsTickerRepository.Create(_newsTicker1);
      var result = _newsTickerRepository.GetNewsTicker();
      result.Should().NotBeNull();
      
    }

    [Test]
    public void GivenANewsTicker_WhenITryToSearchForItById_ItIsRetrievedFromTheDatabase()
    {
      NewsTicker nt= _newsTickerRepository.Create(_newsTicker1);
      var result = _newsTickerRepository.GetNewsTicker(nt.Id);
      result.Should().NotBeNull();
      result.News.Should().Be("test news");
    }
  }
}
