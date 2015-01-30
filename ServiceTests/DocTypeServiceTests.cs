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
  public class DocTypeServiceTests
  {
    private Mock<IDocTypeRepository> _docTypeRepository;
    private IDocTypeService _docTypeService;

    [SetUp]
    public void SetUp()
    {
      _docTypeRepository = new Mock<IDocTypeRepository>();
      _docTypeService = new DocTypeService(_docTypeRepository.Object);
    }

    [Test]
    public void GivenValidDocTypeDetails_WhenADocTypeIsAdded_ThenTheDataBaseIsUpdated()
    {
      _docTypeService.CreateDocType(It.IsAny<string>(), It.IsAny<string>());
      _docTypeRepository.Verify(s => s.Create(It.IsAny<DocType>()), Times.Once());
    }

    [Test]
    public void GivenValidDocTypeDetails_WhenADocTypeIsAdded_AndTheDatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _docTypeService.CreateDocType(It.IsAny<string>(), It.IsAny<string>());
      _docTypeRepository.Setup(c => c.Create(It.IsAny<DocType>())).Throws<Exception>();

      Action act = () => _docTypeService.CreateDocType(It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAnId_WhenAnDocTypeIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _docTypeRepository.Setup(a => a.GetDocType((It.IsAny<int>()))).Throws<Exception>();

      Action act = () => _docTypeService.GetDocType(It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAId_WhenADocTypeIsRequested_ThenADocTypeIsReturned()
    {
      _docTypeRepository.Setup(a => a.GetDocType(It.IsAny<int>())).Returns(new DocType());
      DocType doctpye = _docTypeService.GetDocType(It.IsAny<int>());

      doctpye.Should().NotBeNull();
    }

    [Test]
    public void GivenACode_WhenAnDocTypeIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _docTypeRepository.Setup(a => a.GetDocType((It.IsAny<string>()))).Throws<Exception>();

      Action act = () => _docTypeService.GetDocType(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenACode_WhenADocTypeIsRequested_ThenADocTypeIsReturned()
    {
      _docTypeRepository.Setup(a => a.GetDocType(It.IsAny<string>())).Returns(new DocType());
      DocType doctpye = _docTypeService.GetDocType(It.IsAny<string>());

      doctpye.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_WhenIRequestAllDocTypes_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _docTypeRepository.Setup(a => a.GetDocTypes()).Throws<Exception>();

      Action act = () => _docTypeService.GetDocTypes();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAValidData_WhenIRequestAllDocTypes_ThenAllDocTypesReturned()
    {
      var testDocTypes = new List<DocType>();

      _docTypeRepository.Setup(a => a.GetDocTypes()).Returns(testDocTypes);
      var doctypes = _docTypeService.GetDocTypes();

      doctypes.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_AndAnUnavailableDatabase_WhenIUpdateAnSocType_ThenAnUnityExceptionIsThrown()
    {
      _docTypeRepository.Setup(p => p.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws
          <Exception>();
      var _docTypeService = new DocTypeService(_docTypeRepository.Object);

      Action act = () => _docTypeService.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenIUpdateADocType_ThenTheDocTypeisUpdated()
    {
      var _docTypeService = new DocTypeService(_docTypeRepository.Object);

      _docTypeService.Update(1, "code", "description");
      _docTypeRepository.Verify(p => p.Update(1, "code", "description"));
    }

    [Test]
    public void GivenAValidDocType_WhenIAddASubDocType_AndTheDatabaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      _docTypeRepository.Setup(p => p.AddSubDocType(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws
            <Exception>();
      Assert.Throws<UnityException>(() => _docTypeService.AddSubDocType(1, "code", "description"));
    }

    [Test]
    public void GivenAValidPost_WhenIAddAComment_ThenTheCommentIsWrittenToTheDatabase()
    {
      _docTypeService.AddSubDocType(1, "code", "description");
      _docTypeRepository.Verify(p => p.AddSubDocType(1, "code", "description"), Times.Once());
    }
  }
}
