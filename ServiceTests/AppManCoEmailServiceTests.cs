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
  public class AppManCoEmailServiceTests
  {
    private Mock<IAppManCoEmailRepository> _appManCoEmailRepository;
    private IAppManCoEmailService _appManCoEmailService;

    [SetUp]
    public void SetUp()
    {
      _appManCoEmailRepository = new Mock<IAppManCoEmailRepository>();  
      _appManCoEmailService = new AppManCoEmailService(_appManCoEmailRepository.Object);
    }

    [Test]
    public void GivenValidAppManCoEmailId_WhenISearchById_ThenTheAppManCoEmailIsRetrievedFromTheDatabase()
    {
      this._appManCoEmailRepository.Setup(x => x.GetAppManCoEmail(It.IsAny<int>())).Returns(new AppManCoEmail());

      var result = this._appManCoEmailService.GetAppManCoEmail(It.IsAny<int>());

      this._appManCoEmailRepository.Verify(x => x.GetAppManCoEmail(It.IsAny<int>()), Times.AtLeastOnce);
      result.Should().NotBeNull();
      result.Should().BeOfType<AppManCoEmail>();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenValidAppManCoEmailId_WhenISearchById_AnddatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      this._appManCoEmailRepository.Setup(x => x.GetAppManCoEmail(It.IsAny<int>())).Throws<UnityException>();

      this._appManCoEmailService.GetAppManCoEmail(It.IsAny<int>());

      this._appManCoEmailRepository.Verify(x => x.GetAppManCoEmail(It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantGetAllAppManCoEmails_ThenTheAppManCoEmailsIsRetrievedFromTheDatabase()
    {
      this._appManCoEmailRepository.Setup(x => x.GetAppManCoEmails()).Returns(new List<AppManCoEmail>());

      var result = this._appManCoEmailService.GetAppManCoEmails();

      this._appManCoEmailRepository.Verify(x => x.GetAppManCoEmails(), Times.AtLeastOnce);
      result.Should().NotBeNull();
      result.Should().BeOfType<List<AppManCoEmail>>();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantGetAllAppManCoEmails_AnddatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      this._appManCoEmailRepository.Setup(x => x.GetAppManCoEmails()).Throws<UnityException>();

      this._appManCoEmailService.GetAppManCoEmails();

      this._appManCoEmailRepository.Verify(x => x.GetAppManCoEmails(), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantGetAppManCoEmailsByAppIdAndManCoId_ThenTheAppManCoEmailsIsRetrievedFromTheDatabase()
    {
        this._appManCoEmailRepository.Setup(x => x.GetAppManCoEmails(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<AppManCoEmail>());

        var result = this._appManCoEmailService.GetAppManCoEmails(It.IsAny<int>(), It.IsAny<int>());

        this._appManCoEmailRepository.Verify(x => x.GetAppManCoEmails(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
        result.Should().NotBeNull();
        result.Should().BeOfType<List<AppManCoEmail>>();
    }

    [Test]
    public void WhenIWantGetPagedAppManCoEmailsByAppIdAndManCoId_ThenTheAppManCoEmailsIsRetrievedFromTheDatabase()
    {
        this._appManCoEmailRepository.Setup(x => x.GetPagedAppManCoEmails(It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<AppManCoEmail>());

        var result = this._appManCoEmailService.GetPagedAppManCoEmails(It.IsAny<int>(), It.IsAny<int>());

        this._appManCoEmailRepository.Verify(x => x.GetPagedAppManCoEmails(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResult<AppManCoEmail>>();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantGetAppManCoEmailsByAppIdAndManCoId_AnddatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
        this._appManCoEmailRepository.Setup(x => x.GetAppManCoEmails(It.IsAny<int>(), It.IsAny<int>())).Throws<UnityException>();

        this._appManCoEmailService.GetAppManCoEmails(It.IsAny<int>(), It.IsAny<int>());

        this._appManCoEmailRepository.Verify(x => x.GetAppManCoEmails(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantGetPagedAppManCoEmailsByAppIdAndManCoId_AnddatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
        this._appManCoEmailRepository.Setup(x => x.GetPagedAppManCoEmails(It.IsAny<int>(), It.IsAny<int>())).Throws<UnityException>();

        this._appManCoEmailService.GetPagedAppManCoEmails(It.IsAny<int>(), It.IsAny<int>());

        this._appManCoEmailRepository.Verify(x => x.GetPagedAppManCoEmails(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenValidAppManCoEmailDetails_WhenAnAppManCoEmailIsAdded_ThenTheDatabaseIsUpdated()
    {
      this._appManCoEmailRepository.Setup(x => x.Create(It.IsAny<AppManCoEmail>()));

      this._appManCoEmailService.CreateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());

      this._appManCoEmailRepository.Verify(x => x.Create(It.IsAny<AppManCoEmail>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenValidAppManCoEmailDetails_WhenAnAppManCoEmailIsAdded_AnddatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      this._appManCoEmailRepository.Setup(x => x.Create(It.IsAny<AppManCoEmail>())).Throws<UnityException>();

      this._appManCoEmailService.CreateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());

      this._appManCoEmailRepository.Verify(x => x.Create(It.IsAny<AppManCoEmail>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenValidAppManCoEmailDetails_WhenAnAppManCoEmailIsUpdated_ThenTheDatabaseIsUpdated()
    {
        this._appManCoEmailRepository.Setup(x => x.UpdateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        this._appManCoEmailService.UpdateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

        this._appManCoEmailRepository.Verify(x => x.UpdateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenValidAppManCoEmailDetails_WhenAnAppManCoEmailIsUpdated_AnddatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
        this._appManCoEmailRepository.Setup(x => x.UpdateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<UnityException>();

        this._appManCoEmailService.UpdateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

        this._appManCoEmailRepository.Verify(x => x.UpdateAppManCoEmail(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenAppManCoEmailIsDeleted_ThenItIsDeleted()
    {
      _appManCoEmailRepository.Setup(m => m.GetAppManCoEmail(It.IsAny<int>())).Returns(new AppManCoEmail());
      _appManCoEmailService.DeleteAppManCoEmail(It.IsAny<int>());
      _appManCoEmailRepository.Verify(x => x.Delete(It.IsAny<AppManCoEmail>()), Times.Once);
    }

    [Test]
    public void WhenAppManCoEmailIsDeleted_AndTheDatabaseIsNotAvailable_ThenAUnityExceptionIsThrown()
    {
      _appManCoEmailRepository.Setup(m => m.GetAppManCoEmail(It.IsAny<int>())).Throws<Exception>();
      Assert.Throws<UnityException>(
          () =>
          _appManCoEmailService.DeleteAppManCoEmail(It.IsAny<int>()));
    }
  }
}
