namespace EFRepositoryTests
{
 using System.Configuration;
 using System.Linq;
 using System.Transactions;
 using Builder;
 using Entities.File;
 using FluentAssertions;
 using NUnit.Framework;
 using UnityRepository.Repositories;
 using System;
 using System.Collections.Generic;
 using Entities;

  [Category("Integration")]
  [TestFixture]
  public class ZipFileRepositoryTest
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _zipFileRepository = new ZipFileRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _manCoRepository = new ManCoRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _manCo = BuildMeA.ManCo("description", "code");
      _manCo2 = BuildMeA.ManCo("description2", "code2");
      _manCo3 = BuildMeA.ManCo("description3", "code3");
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private ZipFileRepository _zipFileRepository;
    private ManCoRepository _manCoRepository;
    private ManCo _manCo;
    private ManCo _manCo2;
    private ManCo _manCo3;

    [Test]
    public void GivenABigZip_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _zipFileRepository.Entities.Count();
      _manCoRepository.Create(_manCo);
      
      ZipFile bigZipFile = BuildMeA.ZipFile(true, "documentSetId", "file1.zip", string.Empty, DateTime.Now);
      _zipFileRepository.Create(bigZipFile);

      _zipFileRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenALittleZip_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _zipFileRepository.Entities.Count();
      _manCoRepository.Create(_manCo);

      ZipFile littleZipFile = BuildMeA.ZipFile(false, "documentSetId", "file2.zip", "file1.zip", DateTime.Now);
      _zipFileRepository.Create(littleZipFile);

      _zipFileRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenABigZipFileName_WhenITryToSearchByFilenameFromTheDatabase_ItIsRetrievedFromTheDatabase()
    {
      _manCoRepository.Create(_manCo);
      ZipFile bigZipFile1 = BuildMeA.ZipFile(true, "documentSetId1", "file200.zip", "file100.zip", DateTime.Now);
       
      _zipFileRepository.Create(bigZipFile1);

      var result = this._zipFileRepository.SearchBigZip(bigZipFile1.FileName);

      result.Should().NotBeNull();
      result.Count.ShouldBeEquivalentTo(1);
      result[0].FileName.ShouldBeEquivalentTo("file200.zip");
    }

    //[Test]
    public void GivenABigZipFileName_WhenITryToSearchByFilenameAndManCoIdsFromTheDatabase_ItIsRetrievedFromTheDatabase()
    {
        _manCoRepository.Create(_manCo);
        _manCoRepository.Create(_manCo2);
        _manCoRepository.Create(_manCo3);

        ZipFile bigZipFile1 = BuildMeA.ZipFile(true, "documentSetId1", "file200.zip", "file100.zip", DateTime.Now);
        ZipFile bigZipFile2 = BuildMeA.ZipFile(true, "documentSetId2", "file201.zip", "file101.zip", DateTime.Now);
        ZipFile bigZipFile3 = BuildMeA.ZipFile(true, "documentSetId3", "file202.zip", "file101.zip", DateTime.Now);

        _zipFileRepository.Create(bigZipFile1);
        _zipFileRepository.Create(bigZipFile2);
        _zipFileRepository.Create(bigZipFile3);

        var listManCoIds = new List<int>();

        listManCoIds.Add(_manCo.Id);
        listManCoIds.Add(_manCo3.Id);

        var result = this._zipFileRepository.SearchBigZip("file20", listManCoIds);

        result.Should().NotBeNull();
        result.Count.ShouldBeEquivalentTo(2);
        result[0].FileName.ShouldBeEquivalentTo("file200.zip");
        result[1].FileName.ShouldBeEquivalentTo("file202.zip");
    }
  }
}
