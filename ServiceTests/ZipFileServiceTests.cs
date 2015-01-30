namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using Exceptions;
  using FluentAssertions;
  using NUnit.Framework;
  using Moq;
  using UnityRepository.Interfaces;
  using Services;
  using ServiceInterfaces;
  using Entities.File;

  [TestFixture]
  public class ZipFileServiceTests
  {
    private Mock<IZipFileRepository> _zipFileRepository;
    private IZipFileService _zipFileService;

    [SetUp]
    public void SetUp()
    {
      _zipFileRepository = new Mock<IZipFileRepository>();
      _zipFileService = new ZipFileService(_zipFileRepository.Object);
    }

    [Test]
    public void GivenABigZipFile_WhenTheZipFileIsAdded_ThenTheZipFileIsAddedToTheRepository()
    {
        _zipFileService.CreateBigZipFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>());
      _zipFileRepository.Verify(s => s.Create(It.IsAny<ZipFile>()), Times.Once());
    }

    [Test]
    public void GivenALittleZipFile_WhenTheZipFileIsAdded_ThenTheZipFileIsAddedToTheRepository()
    {
        _zipFileService.CreateLittleZipFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>());
      _zipFileRepository.Verify(s => s.Create(It.IsAny<ZipFile>()), Times.Once());
    }

    [Test]
    public void GivenABigZipFileName_WhenITryToSearchForIt_AndDatabaseIsAvailable_TheBigZipFilIsRetrieved()
    {
      this._zipFileRepository.Setup(x => x.SearchBigZip(It.IsAny<string>())).Returns(new List<ZipFile>());

      var result = this._zipFileService.SearchBigZip(It.IsAny<string>());

      this._zipFileRepository.Verify(x => x.SearchBigZip(It.IsAny<string>()));

      result.Should().NotBeNull();
      result.Should().BeOfType<List<ZipFile>>();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenABigZipFileName_WhenITryToSearchForIt_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._zipFileRepository.Setup(x => x.SearchBigZip(It.IsAny<string>())).Throws<UnityException>();

      this._zipFileService.SearchBigZip(It.IsAny<string>());

      this._zipFileRepository.Verify(x => x.SearchBigZip(It.IsAny<string>()));
    }

    [Test]
    public void GivenABigZipFileNameAndManCoIds_WhenITryToSearchForIt_AndDatabaseIsAvailable_TheBigZipFilIsRetrieved()
    {
        this._zipFileRepository.Setup(x => x.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>())).Returns(new List<ZipFile>());

        var result = this._zipFileService.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>());

        this._zipFileRepository.Verify(x => x.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>()));

        result.Should().NotBeNull();
        result.Should().BeOfType<List<ZipFile>>();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenABigZipFileNameAndManCoIds_WhenITryToSearchForIt_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
        this._zipFileRepository.Setup(x => x.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>())).Throws<UnityException>();

        this._zipFileService.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>());

        this._zipFileRepository.Verify(x => x.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>()));
    }
  }
}
