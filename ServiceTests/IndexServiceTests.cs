namespace ServiceTests
{
  using System;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class IndexServiceTests
  {
    [SetUp]
    public void Setup()
    {
      _indexRepository = new Mock<IIndexRepository>();
      _indexService = new IndexService(_indexRepository.Object);
    }

    private IIndexService _indexService;
    private Mock<IIndexRepository> _indexRepository;

    [Test]
    public void GivenValidData_WhenAnIndexIsRetrieved_AndTheDatabaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      _indexRepository.Setup(p => p.GetIndex(It.IsAny<int>())).Throws<Exception>();

      Action act = () => _indexService.GetIndex(It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenAnIndexIsRetrieved_ThenThatIndexIsReturned()
    {
      _indexRepository.Setup(p => p.GetIndex(It.IsAny<int>())).Returns(new IndexDefinition { Id = 1 });
      IndexDefinition index = _indexService.GetIndex(It.IsAny<int>());

      index.Id.Should().Be(1);
    }

    [Test]
    public void GivenValidData_AndAnUnavailableDatabase_WhenIUpdateAnIndex_ThenAnUnityExceptionIsThhrown()
    {
      _indexRepository.Setup(p => p.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws
          <Exception>();
      var _indexService = new IndexService(_indexRepository.Object);

      Action act = () => _indexService.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>() ,It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenIUpdateAnIndex_ThenTheIndexIsUpdated()
    {
      var _indexService = new IndexService(_indexRepository.Object);

      _indexService.Update(1, "name", "archive Name", "archvie value");
      _indexRepository.Verify(p => p.Update(1, "name", "archive Name", "archvie value"));
    }

    [Test]
    public void GivenValidApplicationId_WhenAnIndexIsRetrieved_ThenThatIndexIsReturned()
    {
        _indexRepository.Setup(p => p.GetIndexByApplicationId(It.IsAny<int>())).Returns(new IndexDefinition { Id = 1, ApplicationId = 3});
        IndexDefinition index = _indexService.GetByApplicationId(It.IsAny<int>());

        _indexRepository.Verify(p => p.GetIndexByApplicationId(It.IsAny<int>()), Times.Once());

        index.Id.Should().Be(1);
        index.ApplicationId.Should().Be(3);
    }

    [Test]
    public void GivenValidApplicationId_WhenAnIndexCannotBeRetrieved_ThenAUnityExceptionIsThrown()
    {
        _indexRepository.Setup(p => p.GetIndexByApplicationId(It.IsAny<int>())).Throws<Exception>();

        Action act = () => _indexService.GetByApplicationId(It.IsAny<int>());

        act.ShouldThrow<UnityException>();

        _indexRepository.Verify(p => p.GetIndexByApplicationId(It.IsAny<int>()), Times.Once());

    }
  }
}
