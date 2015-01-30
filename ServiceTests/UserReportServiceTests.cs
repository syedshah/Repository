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
  public class UserReportServiceTests
  {
    private Mock<IApplicationUserRepository> _applicationUserRepository;
    private IUserReportService _userReportService;

    [SetUp]
    public void SetUp()
    {
      _applicationUserRepository = new Mock<IApplicationUserRepository>();
      _userReportService = new UserReportService(_applicationUserRepository.Object);
    }

    [Test]
    public void GivenValidAValidDomicile_WhenUserReportDataIsRequested_ThenTheDataIsReturned()
    {
      _applicationUserRepository.Setup(a => a.GetUserReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<ApplicationUser>());
      var users = _userReportService.GetUserReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
      users.Should().NotBeNull();
    }

    [Test]
    public void GivenValidAValidDomicile_WhenUserReportDataIsRequested_AndTheDataBaseIsUnavailable__ThenTheDataIsReturned()
    {
      _applicationUserRepository.Setup(a => a.GetUserReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();

      Action act = () => _userReportService.GetUserReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }
  }
}
