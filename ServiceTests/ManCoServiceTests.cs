namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class ManCoServiceTests
  {
    private Mock<IManCoRepository> _manCoRepository;
    private IManCoService _manCoService;

    [SetUp]
    public void SetUp()
    {
      _manCoRepository = new Mock<IManCoRepository>();
      _manCoService = new ManCoService(_manCoRepository.Object);
    }

    [Test]
    public void GivenValidManCoDetails_WhenAManCoIsAdded_ThenTheDataBaseIsUpdated()
    {
      _manCoService.CreateManCo(It.IsAny<string>(), It.IsAny<string>());
      _manCoRepository.Verify(s => s.Create(It.IsAny<ManCo>()), Times.Once());
    }

    [Test]
    public void GivenValidManCoDetails_WhenAnApplicationIsAdded_AndTheDatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _manCoService.CreateManCo(It.IsAny<string>(), It.IsAny<string>());
      _manCoRepository.Setup(c => c.Create(It.IsAny<ManCo>())).Throws<Exception>();

      Action act = () => _manCoService.CreateManCo(It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAnId_WhenAManCoIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _manCoRepository.Setup(a => a.GetManCo((It.IsAny<int>()))).Throws<Exception>();

      Action act = () => _manCoService.GetManCo(It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAId_WhenAManCoIsRequested_ThenAManCoIsReturned()
    {
      _manCoRepository.Setup(a => a.GetManCo(It.IsAny<int>())).Returns(new ManCo());
      ManCo manCo = _manCoService.GetManCo(It.IsAny<int>());

      manCo.Should().NotBeNull();
    }

    [Test]
    public void GivenACode_WhenAManCoIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _manCoRepository.Setup(a => a.GetManCo((It.IsAny<string>()))).Throws<Exception>();

      Action act = () => _manCoService.GetManCo(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenACode_WhenAManCoIsRequested_ThenAManCoIsReturned()
    {
      _manCoRepository.Setup(a => a.GetManCo(It.IsAny<string>())).Returns(new ManCo());
      ManCo manCo = _manCoService.GetManCo(It.IsAny<string>());

      manCo.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_WhenIRequestAllManCos_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _manCoRepository.Setup(a => a.GetManCos()).Throws<Exception>();

      Action act = () => _manCoService.GetManCos();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidDomicileId_WhenIRequestAllManCosForADomicile_TheManCosAreRetrieved()
    {
        _manCoRepository.Setup(a => a.GetManCos(It.IsAny<int>())).Returns(new List<ManCo>());

        var result = _manCoService.GetManCos(It.IsAny<int>());

        result.Should().NotBeNull();
        _manCoRepository.Verify(a => a.GetManCos(It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenValidDomicileId_WhenIRequestAllManCosForADomicile_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
        _manCoRepository.Setup(a => a.GetManCos(It.IsAny<int>())).Throws<Exception>();

        Action act = () => _manCoService.GetManCos(It.IsAny<int>());

        act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAValidData_WhenIRequestAllManCos_ThenAllDocTypesReturned()
    {
      var testManCos = new List<ManCo>();

      _manCoRepository.Setup(a => a.GetManCos()).Returns(testManCos);
      var applications = _manCoService.GetManCos();

      applications.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_AndAnUnavailableDatabase_WhenIUpdateADocType_ThenAnUnityExceptionIsThrown()
    {
      _manCoRepository.Setup(p => p.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws
          <Exception>();
      var _docTypeService = new ManCoService(_manCoRepository.Object);

      Action act = () => _docTypeService.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenIUpdateADocType_ThenTheDocTypeisUpdated()
    {
      var _docTypeService = new ManCoService(_manCoRepository.Object);

      _docTypeService.Update(1, "name", "description");
      _manCoRepository.Verify(p => p.Update(1, "name", "description"));
    }

    [Test]
    public void GivenAValidUserId_WhenITryToRetrieveManCos_ThenTheManCosAreRetrieved()
    {
      _manCoRepository.Setup(
          a => a.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo>());

      var result = this._manCoService.GetManCosByUserId(It.IsAny<string>());

      _manCoRepository.Verify(x => x.GetManCosByUserId(It.IsAny<string>()));
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAValidUserId_WhenITryToRetrieveManCos_AndDatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _manCoRepository.Setup(
          a => a.GetManCosByUserId(It.IsAny<string>())).Throws<Exception>();

      Action act = () => this._manCoService.GetManCosByUserId(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }
  }
}
