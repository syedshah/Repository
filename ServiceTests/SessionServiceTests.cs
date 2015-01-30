// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Tests for session service tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceTests
{
  using System;
  using System.Collections.Generic;

  using Entities;

  using Exceptions;

  using FluentAssertions;

  using Moq;

  using NUnit.Framework;

  using Services;

  using UnityRepository.Interfaces;

  [TestFixture]
  public class SessionServiceTests
  {
    private Mock<ISessionRepository> _sessionRepository;

    private SessionService _sessionService;

    [SetUp]
    public void SetUp()
    {
      _sessionRepository = new Mock<ISessionRepository>();
      _sessionService = new SessionService(_sessionRepository.Object);
    }

    [Test]
    public void GivenValidData_AndAnUnavailableDatabase_WhenIUpdateASession_ThenAUnityExceptionIsThrown()
    {
      _sessionRepository.Setup(p => p.Update(It.IsAny<string>(), It.IsAny<DateTime>())).Throws<Exception>();

      Action act = () => _sessionService.Update(It.IsAny<string>(), It.IsAny<DateTime>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenIUpdateASession_ThenTheSessionIsUpdated()
    {
      DateTime end = DateTime.Now;

      _sessionService.Update("guid", end);
      _sessionRepository.Verify(b => b.Update("guid", end), Times.Once());
    }

    [Test]
    public void GivenValidData_AndAnUnavailableDatabase_WhenIAskForSessions_ThenAUnityExceptionIsThrown()
    {
      _sessionRepository.Setup(p => p.GetSessionsByGovReadOnlyAdmin(It.IsAny<DateTime>(), It.IsAny<int>())).Throws<Exception>();

      Action act = () => _sessionService.GetSessionsByGovReadOnlyAdmin(It.IsAny<DateTime>(), It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenIAskForSessions_ThenSessionsAreReturned()
    {
      _sessionRepository.Setup(p => p.GetSessionsByGovReadOnlyAdmin(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new List<Session>());

      _sessionService.GetSessionsByGovReadOnlyAdmin(It.IsAny<DateTime>(), It.IsAny<int>());
      _sessionRepository.Verify(b => b.GetSessionsByGovReadOnlyAdmin(It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
    }
  }
}
