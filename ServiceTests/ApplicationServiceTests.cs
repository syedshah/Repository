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
  public class ApplicationServiceTests
  {
    private Mock<IApplicationRepository> _applicationRepository;
    private IApplicationService _applicationService;

    [SetUp]
    public void SetUp()
    {
      _applicationRepository = new Mock<IApplicationRepository>();
      _applicationService = new ApplicationService(_applicationRepository.Object);
    }

    [Test]
    public void GivenValidApplicationDetails_WhenAnApplicationIsAdded_ThenTheDataBaseIsUpdated()
    {
      _applicationService.CreateApplication(It.IsAny<string>(), It.IsAny<string>());
      _applicationRepository.Verify(s => s.Create(It.IsAny<Application>()), Times.Once());
    }

    [Test]
    public void GivenValidApplicationDetails_WhenAnApplicationIsAdded_AndTheDatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _applicationRepository.Setup(c => c.Create(It.IsAny<Application>())).Throws<Exception>();

      Action act = () => _applicationService.CreateApplication(It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenACode_WhenAApplicationIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _applicationRepository.Setup(a => a.GetApplication((It.IsAny<string>()))).Throws<Exception>();

      Action act = () => _applicationService.GetApplication(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAValidCode_WhenAnApplicationIsRequested_ThenAApplicationIsReturned()
    {
      _applicationRepository.Setup(a => a.GetApplication(It.IsAny<string>())).Returns(new Application());
      Application application = _applicationService.GetApplication(It.IsAny<string>());

      application.Should().NotBeNull();
    }

    [Test]
    public void GivenAnId_WhenAnApplicationIsRequested_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _applicationRepository.Setup(a => a.GetApplication((It.IsAny<int>()))).Throws<Exception>();

      Action act = () => _applicationService.GetApplication(It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAId_WhenAnApplicationIsRequested_ThenAnApplicationIsReturned()
    {
      _applicationRepository.Setup(a => a.GetApplication(It.IsAny<int>())).Returns(new Application());
      Application application = _applicationService.GetApplication(It.IsAny<int>());

      application.Should().NotBeNull();
    }

    [Test]
    public void GivenValidData_WhenIRequestAllApplications_AndTheDataBaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _applicationRepository.Setup(a => a.GetApplications()).Throws<Exception>();

      Action act = () => _applicationService.GetApplications();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAValidCode_WhenIRequestAllApplications_ThenAllApplicationAreReturned()
    {
      var testApplications = new List<Application>();

      _applicationRepository.Setup(a => a.GetApplications()).Returns(testApplications);
      var applications = _applicationService.GetApplications();

      applications.Should().NotBeNull();
    }

    [Test]
    public void GivenAValidApplication_WhenIAddAnIndex_AndTheDatabaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      _applicationRepository.Setup(p => p.AddIndex(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws
            <Exception>();
      Assert.Throws<UnityException>(() => _applicationService.AddIndex(1, "name", "archiveName", "archiveValue"));
    }

    [Test]
    public void GivenAValidPost_WhenIAddAComment_ThenTheCommentIsWrittenToTheDatabase()
    {
      _applicationService.AddIndex(1, "name", "archiveName", "archiveValue");
      _applicationRepository.Verify(p => p.AddIndex(1, "name", "archiveName" , "archiveValue"), Times.Once());
    }
  }
}
