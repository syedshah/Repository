namespace UnityWebTests.Models
{
  using System;
  using System.Web.Security;

  using Entities;

  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using UnityWeb.Models.Account;
  using UnityWebTests.Controllers;

  [TestFixture]
  public class UserSummaryViewModelTests : ControllerTestsBase
  {
    private string _sessionValue;

    [Test]
    public void GivenAMembershipUser_WhenICreateAUserSummaryViewModelViewModel_AndTheUserLogsInViaTheUserInTheCookie_TheLastLoggedInDateIsTheDateFromTheProvider()
    {
      _sessionValue = null;

      var userMock = new Mock<ApplicationUser>();
      userMock.SetupGet(u => u.UserName).Returns("test");
      userMock.SetupGet(u => u.LastLoginDate).Returns(new DateTime(2013, 1, 1, 0, 0, 0));

      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var vr = new UserSummaryViewModel(userMock.Object, MockHttpContext.Object);
      vr.LastLoggedInDate.Should().Be("01/01/2013 00:00:00");
    }

    [Test]
    public void GivenAMembershipUser_WhenICreateAUserSummaryViewModelViewModel_AndTheUserHasComeFromTheLogInPage_TheUserSummaryViewModelIsCreatedProperly()
    {
      _sessionValue = DateTime.Now.AddDays(-7).ToString();

      var userMock = new Mock<ApplicationUser>();

      userMock.SetupGet(u => u.IsApproved).Returns(true);
      userMock.SetupGet(u => u.IsLockedOut).Returns(false);

      userMock.Setup(u => u.FirstName).Returns("test");
      userMock.SetupGet(u => u.LastLoginDate).Returns(new DateTime(2013, 1, 1, 0, 0, 0));

      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var vr = new UserSummaryViewModel(userMock.Object, MockHttpContext.Object);
      vr.FirstName.Should().Be("test");
      vr.LastLoggedInDate.Should().Be(_sessionValue);
    }
  }
}
