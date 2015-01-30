namespace EFRepositoryTests
{
  using System;
  using System.Collections.Generic;
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
  public class ExportRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _exportRepository = new ExportRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _domicile = BuildMeA.Domicile("code", "description");
      _docType = BuildMeA.DocType("code", "description");
      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType);
      _manCo1 = BuildMeA.ManCo("description1", "code1").WithDomicile(_domicile);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private ExportRepository _exportRepository;
    private Document _document1;
    private Export _export;
    private DocType _docType;
    private SubDocType _subDocType1;
    private ManCo _manCo1;
    private Domicile _domicile;

    [Test]
    public void GivenAnExportCollection_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _exportRepository.Entities.Count();

      var exports = new List<Export>();

      for (var i = 1; i < 1001; i++)
      {
          _document1 = BuildMeA.Document(string.Format("document {0}", i))
                      .WithDocType(_docType)
                      .WithSubDocType(_subDocType1)
                      .WithManCo(_manCo1);

          _export = BuildMeA.Export(DateTime.Now).WithDocument(_document1);

        exports.Add(_export);
      }

      _exportRepository.Create(exports);

      _exportRepository.Entities.Count().Should().Be(initialCount + 1000);
    }

    [Test]
    public void GivenAnExportCollectionLessThan100_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _exportRepository.Entities.Count();

      var exports = new List<Export>();

      for (var i = 1; i < 3; i++)
      {
        _document1 = BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType)
                    .WithSubDocType(_subDocType1)
                    .WithManCo(_manCo1);

        _export = BuildMeA.Export(DateTime.Now).WithDocument(_document1);

        exports.Add(_export);
      }

      _exportRepository.Create(exports);

      _exportRepository.Entities.Count().Should().Be(initialCount + 2);
    }

    [Test]
    public void GivenAnExportCollectionOf100_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _exportRepository.Entities.Count();

      var exports = new List<Export>();

      for (var i = 1; i < 101; i++)
      {
        _document1 = BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType)
                    .WithSubDocType(_subDocType1)
                    .WithManCo(_manCo1);

        _export = BuildMeA.Export(DateTime.Now).WithDocument(_document1);

        exports.Add(_export);
      }

      _exportRepository.Create(exports);

      _exportRepository.Entities.Count().Should().Be(initialCount + 100);
    }

    [Test]
    public void GivenAnExportCollectionOf231_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _exportRepository.Entities.Count();

      var exports = new List<Export>();

      for (var i = 1; i < 232; i++)
      {
        _document1 = BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType)
                    .WithSubDocType(_subDocType1)
                    .WithManCo(_manCo1);

        _export = BuildMeA.Export(DateTime.Now).WithDocument(_document1);

        exports.Add(_export);
      }

      _exportRepository.Create(exports);

      _exportRepository.Entities.Count().Should().Be(initialCount + 231);
    }

    [Test]
    public void GivenAnExportCollectionOf1471_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _exportRepository.Entities.Count();

      var exports = new List<Export>();

      for (var i = 1; i < 1472; i++)
      {
        _document1 = BuildMeA.Document(string.Format("document {0}", i))
                    .WithDocType(_docType)
                    .WithSubDocType(_subDocType1)
                    .WithManCo(_manCo1);

        _export = BuildMeA.Export(DateTime.Now).WithDocument(_document1);

        exports.Add(_export);
      }

      _exportRepository.Create(exports);

      _exportRepository.Entities.Count().Should().Be(initialCount + 1471);
    }
  }
}
