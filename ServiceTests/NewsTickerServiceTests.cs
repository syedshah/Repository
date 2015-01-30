namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class NewsTickerServiceTests
  {
    private Mock<INewsTickerRepository> _newsTickerRepository;
    private INewsTickerService _newsTickerService;

    [SetUp]
    public void SetUp()
    {
      _newsTickerRepository = new Mock<INewsTickerRepository>();
      _newsTickerService = new NewsTickerService(_newsTickerRepository.Object);
    }

    [Test]
    public void GivenValidTickerValues_IfRepositoryCallAtleastOnce()
    {
      _newsTickerService.AddNewsTicker(It.IsAny<string>(), It.IsAny<DateTime>());
      _newsTickerRepository.Verify(s => s.Create(It.IsAny<NewsTicker>()), Times.Once());
    }

    [Test]
    public void GivenValidNewsTicker_WhenNewsTickerIsAdded_AndTheDatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _newsTickerRepository.Setup(c => c.Create(It.IsAny<NewsTicker>())).Throws<Exception>();
      Action act = () => _newsTickerService.AddNewsTicker(It.IsAny<string>(), It.IsAny<DateTime>());
      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void Given_A_Id_When_NewsTicker_IsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _newsTickerRepository.Setup(n => n.GetNewsTicker()).Throws<Exception>();
      Action act = () => _newsTickerService.GetNewsTicker();
      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAValidId_WhenAnNewsTickerIsRequested_ThenNewsTickerIsReturned()
    {
      _newsTickerRepository.Setup(n => n.GetNewsTicker()).Returns(new NewsTicker());
      NewsTicker newsTicker = _newsTickerService.GetNewsTicker();
      newsTicker.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_WhenIRequestAllNewsTicker_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _newsTickerRepository.Setup(n => n.GetNewsTicker()).Throws<Exception>();

      Action act = () => _newsTickerService.GetNewsTicker();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAValidCode_WhenIRequestAllNewsTicker_ThenAllNewsTickerAreReturned()
    {
      var testNewsTicker = new NewsTicker();

      _newsTickerRepository.Setup(n => n.GetNewsTicker()).Returns(testNewsTicker);
      var newsTicker = _newsTickerService.GetNewsTicker();

      newsTicker.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_WhenIUpdateNewsTickerIsUpdated()
    {
      _newsTickerService.UpdateNewsTicker(It.IsAny<int>() , It.IsAny<string>(),It.IsAny<DateTime>());
      _newsTickerRepository.Verify(p => p.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()));
    }


    

  }
}
