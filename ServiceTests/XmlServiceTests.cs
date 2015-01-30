namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using Entities.File;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class XmlFileServiceTests
  {
    private Mock<IXmlFileRepository> _xmlFileRepository;
    private IXmlFileService _xmlFileService;

    [SetUp]
    public void SetUp()
    {
      _xmlFileRepository = new Mock<IXmlFileRepository>();
      _xmlFileService = new XmlFileService(_xmlFileRepository.Object);
    }

    [Test]
    public void GivenAValidXmlFile_WhenTheXmlFileIsAdded_AndTheDatabaseIsNotAvailable_ThenIGetAUnityException()
    {
      _xmlFileRepository.Setup(x => x.Create(It.IsAny<XmlFile>())).Throws<Exception>();
      Action act = () => _xmlFileService.CreateXmlFile(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<bool>(),
        It.IsAny<int>(),
        It.IsAny<int>(),
        It.IsAny<int>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<DateTime>(),
        It.IsAny<string>(),
        It.IsAny<DateTime>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAValidXmlFile_WhenTheXmlFileIsAdded_ThenTheXmlFileIsAddedToTheRepository()
    {
      _xmlFileService.CreateXmlFile(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<bool>(),
        It.IsAny<int>(),
        It.IsAny<int>(),
        It.IsAny<int>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<DateTime>(),
        It.IsAny<string>(),
        It.IsAny<DateTime>());

      _xmlFileRepository.Verify(s => s.Create(It.IsAny<XmlFile>()), Times.Once());
    }

    [Test]
    public void GivenAFileName_WhenAXmlFileIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _xmlFileRepository.Setup(a => a.GetFile(It.IsAny<string>())).Throws<Exception>();

      Action act = () => _xmlFileService.GetFile(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAValidFileName_WhenAXmlFileIsRequested_ThenAXmlFileIsReturned()
    {
      _xmlFileRepository.Setup(a => a.GetFile(It.IsAny<string>())).Returns(new XmlFile());
      XmlFile xmlFile = _xmlFileService.GetFile(It.IsAny<string>());

      xmlFile.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_WhenISearchForXmlFilesByFileName_IGetXmlFiles()
    {
      _xmlFileRepository.Setup(f => f.Search(It.IsAny<string>())).Returns(new List<XmlFile> { new XmlFile(), new XmlFile() });

      _xmlFileService.Search(It.IsAny<string>());

      _xmlFileRepository.Verify(f => f.Search(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenISearchForXmlFilesByFileNameAndManCoIds_IGetXmlFiles()
    {
        _xmlFileRepository.Setup(f => f.Search(It.IsAny<string>(), It.IsAny<List<int>>())).Returns(new List<XmlFile> { new XmlFile(), new XmlFile() });

        _xmlFileService.Search(It.IsAny<string>(), It.IsAny<List<int>>());

        _xmlFileRepository.Verify(f => f.Search(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Once());
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenValidData_WhenISearchForXmlFilesByFileNameAndManCoIs_AndDatabaseIsUnvailable_AUnityexceptionIsThrown()
    {
        _xmlFileRepository.Setup(f => f.Search(It.IsAny<string>(), It.IsAny<List<int>>())).Throws<UnityException>();

        _xmlFileService.Search(It.IsAny<string>(), It.IsAny<List<int>>());

        _xmlFileRepository.Verify(f => f.Search(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Once());
    }
  }
}
