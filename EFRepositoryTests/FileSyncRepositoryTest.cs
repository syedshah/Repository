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
  public class FileSyncRepositoryTest
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _fileSyncRepository = new FileSyncRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _fileSync1 = new FileSync { GridRunId = 1 };
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private FileSyncRepository _fileSyncRepository;
    private FileSync _fileSync1;

    [Test]
    public void GivenFileSync_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _fileSyncRepository.Entities.Count();
      var oneStepSync = BuildMeA.OneStepSync(1);
      _fileSyncRepository.Create(oneStepSync);

      _fileSyncRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenMoreThanOneOneStepSync_WhenITryToGetTheMostRecentSync_IGetTheMostRecentSyc()
    {
      var onsStepSyncOne = BuildMeA.OneStepSync(1);
      _fileSyncRepository.Create(onsStepSyncOne);

      var onsStepSyncTwo = BuildMeA.OneStepSync(2);
      _fileSyncRepository.Create(onsStepSyncTwo);

      var onsStepSyncThree = BuildMeA.OneStepSync(3);
      _fileSyncRepository.Create(onsStepSyncThree);

      var latestOneStepSync = _fileSyncRepository.GetLatest();
      latestOneStepSync.GridRunId.Should().Be(3);
    }

    [Test]
    public void GivenAnExistingFileSync_WhenIUpdateTheSyncDate_TheSyncDateIsUpdated()
    {
      DateTime updateDate = new DateTime(2013, 1, 1, 0, 0, 0);

      _fileSyncRepository.Create(_fileSync1);

      FileSync fileSync = _fileSyncRepository.GetLatest();

      fileSync.SyncDate = updateDate;
      
      _fileSyncRepository.Update(_fileSync1);
      fileSync.SyncDate.Should().Be(updateDate);
    }
  }
}
