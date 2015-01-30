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
  public class ExportServiceTests
  {
    private Mock<IExportRepository> _exportRepository;
    private IExportService _exportService;

    [SetUp]
    public void SetUp()
    {
      _exportRepository = new Mock<IExportRepository>();
      _exportService = new ExportService(_exportRepository.Object);
    }

    [Test]
    public void GivenExports_WhenTheExportsAreAdded_ThenTheEportsAreAddedToTheRepository()
    {
      _exportService.CreateExport(It.IsAny<List<Export>>());
      _exportRepository.Verify(s => s.Create(It.IsAny<List<Export>>()), Times.Once());
    }

    [Test]
    public void GivenExports_WhenTheExportsAreAdded_AndTheDatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _exportService.CreateExport(It.IsAny<List<Export>>());
      _exportRepository.Setup(c => c.Create(It.IsAny<List<Export>>())).Throws<Exception>();

      Action act = () => _exportService.CreateExport(It.IsAny<List<Export>>());

      act.ShouldThrow<UnityException>();
    }
  }
}
