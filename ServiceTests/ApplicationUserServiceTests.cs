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
  public class ApplicationUserServiceTests
  {
    private Mock<IApplicationUserRepository> _applicationUserRepository;
    private IApplicationUserService _applicationUserService;

    [SetUp]
    public void SetUp()
    {
      _applicationUserRepository = new Mock<IApplicationUserRepository>();
      _applicationUserService = new ApplicationUserService(_applicationUserRepository.Object);
    }

    [Test]
    public void GivenListUserMancos_WhenMancoIsAdded_ItIssavedToDatabase()
    {
      this._applicationUserRepository.Setup(x => x.UpdateUserMancos(It.IsAny<string>(), It.IsAny<List<ManCo>>()));

      this._applicationUserService.UpdateUserMancos(It.IsAny<string>(), It.IsAny<List<ManCo>>());

      this._applicationUserRepository.Verify(x => x.UpdateUserMancos(It.IsAny<string>(), It.IsAny<List<ManCo>>()));
    }

    [Test]
    public void GivenListUserMancos_WhenMancoIsAdded_IfDatabaseIsUnAvailable_ItThrowsUnityException()
    {
      this._applicationUserRepository.Setup(x => x.UpdateUserMancos(It.IsAny<string>(), It.IsAny<List<ManCo>>()))
          .Throws<Exception>();

      Action act = () => this._applicationUserService.UpdateUserMancos(It.IsAny<string>(), It.IsAny<List<ManCo>>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenListUserDomiciles_WhenDomicileIsAdded_ItIssavedToDatabase()
    {
      this._applicationUserRepository.Setup(x => x.UpdateUserDomiciles(It.IsAny<string>(), It.IsAny<List<Domicile>>()));

      this._applicationUserService.UpdateUserDomiciles(It.IsAny<string>(), It.IsAny<List<Domicile>>());

      this._applicationUserRepository.Verify(x => x.UpdateUserDomiciles(It.IsAny<string>(), It.IsAny<List<Domicile>>()));
    }

    [Test]
    public void GivenListUserDomiciles_WhenDomicileIsAdded_IfDatabaseIsUnAvailable_ItThrowsUnityException()
    {
      this._applicationUserRepository.Setup(x => x.UpdateUserDomiciles(It.IsAny<string>(), It.IsAny<List<Domicile>>()))
          .Throws<Exception>();

      Action act = () => this._applicationUserService.UpdateUserDomiciles(It.IsAny<string>(), It.IsAny<List<Domicile>>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAuserId_WhenISearchForManCos_TheListOfManCosIsRetrievedFromdatabase()
    {
      this._applicationUserRepository.Setup(x => x.GetManCos(It.IsAny<string>())).Returns(new List<ManCo>());

      var result = this._applicationUserService.GetManCos(It.IsAny<string>());

      result.Should().NotBeNull();

      this._applicationUserRepository.Verify(x => x.GetManCos(It.IsAny<string>()));
    }

    [Test]
    public void GivenAuserId_WhenISearchForManCos_IfDatabaseIsUnavailable_ItThrowsAUnityException()
    {
      this._applicationUserRepository.Setup(x => x.GetManCos(It.IsAny<string>())).Throws<Exception>();

      Action act = () => this._applicationUserService.GetManCos(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

  }
}
