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
  public class SubDocTypeServiceTests
  {
    private Mock<ISubDocTypeRepository> _subDocTypeRepository;

    private ISubDocTypeService _subDocTypeService;

    [SetUp]
    public void SetUp()
    {
      _subDocTypeRepository = new Mock<ISubDocTypeRepository>();
      _subDocTypeService = new SubDocTypeService(_subDocTypeRepository.Object);
    }

    [Test]
    public void GivenAnId_WhenASubDocTypeIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _subDocTypeRepository.Setup(a => a.GetSubDocType((It.IsAny<int>()))).Throws<Exception>();

      Action act = () => _subDocTypeService.GetSubDocType(It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAId_WhenASubDocTypeIsRequested_ThenASubDocTypeIsReturned()
    {
      _subDocTypeRepository.Setup(a => a.GetSubDocType(It.IsAny<int>())).Returns(new SubDocType());
      SubDocType subDocType = _subDocTypeService.GetSubDocType(It.IsAny<int>());

      subDocType.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_WhenIRequestAllSubDocTypes_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _subDocTypeRepository.Setup(a => a.GetSubDocTypes()).Throws<Exception>();

      Action act = () => _subDocTypeService.GetSubDocTypes();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAValidData_WhenIRequestAllSubDocTypes_ThenAllSubDocTypesAreReturned()
    {
      var testSubDocTypes = new List<SubDocType>();

      _subDocTypeRepository.Setup(a => a.GetSubDocTypes()).Returns(testSubDocTypes);
      var subDocTypes = _subDocTypeService.GetSubDocTypes();

      subDocTypes.Should().NotBeNull();
    }

    [Test]
    public void
      GivenADocTypeId_WhenIRequestAllSubDocTypesBasedOnADocTypeId_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown
      ()
    {
      _subDocTypeRepository.Setup(a => a.GetSubDocTypes(It.IsAny<int>())).Throws<Exception>();

      Action act = () => _subDocTypeService.GetSubDocTypes(It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenADocTypeId_WhenIRequestAllSubDocTypesBasedOnADocTypeId_ThenAUnityExceptionIsThrown()
    {
      var testSubDocTypes = new List<SubDocType>();

      _subDocTypeRepository.Setup(a => a.GetSubDocTypes(It.IsAny<int>())).Returns(testSubDocTypes);
      var subDocTypes = _subDocTypeService.GetSubDocTypes(It.IsAny<int>());

      subDocTypes.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_AndAnUnavailableDatabase_WhenIUpdateAnSubDocType_ThenAnUnityExceptionIsThrown()
    {
      _subDocTypeRepository.Setup(p => p.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Throws<Exception>();
      var _subDocTypeService = new SubDocTypeService(_subDocTypeRepository.Object);

      Action act = () => _subDocTypeService.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenIUpdateASubDocType_ThenTheSubDocTypeisUpdated()
    {
      var _subDocTypeService = new SubDocTypeService(_subDocTypeRepository.Object);

      _subDocTypeService.Update(1, "code", "description");
      _subDocTypeRepository.Verify(p => p.Update(1, "code", "description"));
    }

    [Test]
    public void GivenSubDocTypeCode_WhenASubDocTypeIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _subDocTypeRepository.Setup(a => a.GetSubDocType(It.IsAny<string>())).Throws<Exception>();

      Action act = () => _subDocTypeService.GetSubDocType(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenSubDocTypeCode_WhenASubDocTypeIsRequested_ThenASubDocTypeIsReturned()
    {
      _subDocTypeRepository.Setup(a => a.GetSubDocType(It.IsAny<string>())).Returns(new SubDocType());
      SubDocType subDocType = _subDocTypeService.GetSubDocType(It.IsAny<string>());

      _subDocTypeRepository.Verify(a => a.GetSubDocType(It.IsAny<string>()), Times.Once());

      subDocType.Should().NotBeNull();
    }
  }
}
