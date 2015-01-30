namespace ServiceTests
{
  using System;
  using Entities;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class FileSyncServiceTests
  {
    private Mock<IFileSyncRepository> _fileSyneRepository;
    private IFileSyncService _filesyncService;

    [SetUp]
    public void SetUp()
    {
      _fileSyneRepository = new Mock<IFileSyncRepository>();
      _filesyncService = new FileSyncService(_fileSyneRepository.Object);
    }

    [Test]
    public void WhenTheLatestFileSyncIsRetrieved_ThenOneFileSyncIsRetieved()
    {
      _fileSyneRepository.Setup(f => f.GetLatest()).Returns(new FileSync(It.IsAny<int>()));
      var fileSync = _filesyncService.GetLatest();

      fileSync.Should().NotBeNull();
    }

    [Test]
    public void WhenTheLatestFileSyncIsRetrieved_ThenTheFileSyncIsRetrievedFromTheRepository()
    {
      FileSync fileSync1 = null;

      _fileSyneRepository.Setup(s => s.GetLatest()).Returns(fileSync1);

      var fileSync = _filesyncService.GetLatest();
      _fileSyneRepository.Verify(s => s.GetLatest(), Times.Once());
    }

    [Test]
    public void WhenAttemptingToGeTheLatestFileSync_AndThereAreNoFileSyncsCurrently_ANewFileSyncIsCreatedWithADummySyncDate()
    {
      FileSync fileSync1 = null;

      _fileSyneRepository.Setup(s => s.GetLatest()).Returns(fileSync1);
      var fileSync = _filesyncService.GetLatest();

      fileSync.Id.Should().Be(0);
      fileSync.SyncDate.Should().Be(new DateTime(2013, 1, 1));
    }

    [Test]
    public void WhenAttemptingToGeTheLatestFileSync_AndThereAreNoFileSyncsCurrently_AnExistingFileSyncIsReturned()
    {
      DateTime syncDate = new DateTime(2013, 1, 1);
      FileSync fileSyncReturn = new FileSync() { Id = 1, GridRunId = 1, SyncDate = syncDate };

      _fileSyneRepository.Setup(s => s.GetLatest()).Returns(fileSyncReturn);

      var fileSync = _filesyncService.GetLatest();

      fileSync.Id.Should().NotBe(0);
      fileSync.SyncDate.Should().Be(syncDate);
    }

    [Test]
    public void GivenValidAGridRunId_WhenAFileSyncIsSaved_ThenTheFileSyncIsSavedToTheDatabase()
    {
      _filesyncService.CreateFileSync(It.IsAny<int>());
      _fileSyneRepository.Verify(s => s.Create(It.IsAny<FileSync>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenAFileSyncIsUpdated_ThenTheFileSyncIsUpdated()
    {
      _fileSyneRepository.Setup(f => f.GetLatest()).Returns(new FileSync());
      _filesyncService.UpdateFileSync(It.IsAny<int>());

      _fileSyneRepository.Verify(s => s.Update(It.IsAny<FileSync>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenAFileSyncIsUpdatedAndItIsTheFirstFileSync_ThenTheFileSyncIsCreated()
    {
      FileSync _fileSync1 = null;

      _fileSyneRepository.Setup(f => f.GetLatest()).Returns(_fileSync1);
      _filesyncService.UpdateFileSync(It.IsAny<int>());

      _fileSyneRepository.Verify(s => s.Create(It.IsAny<FileSync>()), Times.Once());
    }
  }
}
