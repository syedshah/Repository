namespace UnityWebTests.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Microsoft.AspNet.Identity.EntityFramework;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.UserReport;

  [TestFixture]
  public class UserReportsControllerTests
  {
    private UserReportsController _controller;
    private Mock<ILogger> _logger;
    private Mock<IUserReportService> _userReportService;
    private Mock<IUserService> _userService;
    private Mock<ApplicationUser> user1 = new Mock<ApplicationUser>();
    private Mock<ApplicationUser> user2 = new Mock<ApplicationUser>();
    private Mock<ApplicationUser> user3 = new Mock<ApplicationUser>();
    private PagedResult<ApplicationUser> users;

    [SetUp]
    public void SetUp()
    {
      _logger = new Mock<ILogger>();
      _userReportService = new Mock<IUserReportService>();
      _userService = new Mock<IUserService>();
      _controller = new UserReportsController(_userReportService.Object, _userService.Object, _logger.Object);

      user1.SetupGet(u => u.Roles).Returns(new List<IdentityUserRole> { new IdentityUserRole { Role = new IdentityRole { Name = "name1" } } });
      user2.SetupGet(u => u.Roles).Returns(new List<IdentityUserRole> { new IdentityUserRole { Role = new IdentityRole { Name = "name2" } } });
      user3.SetupGet(u => u.Roles).Returns(new List<IdentityUserRole> { new IdentityUserRole { Role = new IdentityRole { Name = "name3" } } });

      users = new PagedResult<ApplicationUser>
      {
        Results = new List<ApplicationUser>
                    {
                      user1.Object, user2.Object, user3.Object
                    },
        CurrentPage = 1,
        TotalItems = 20,
        EndRow = 10,
        StartRow = 1,
        ItemsPerPage = 10
      };

      _userService.Setup(u => u.GetUserDomiciles()).Returns(domiciles);
    }

    private readonly List<Domicile> domiciles = new List<Domicile>
                                                  {
                                                    new Domicile
                                                      {
                                                        Id = 1
                                                      }
                                                  };

    [Test]
    public void GivenAUserReportsController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectNumberOfUsers()
    {
      _userReportService.Setup(u => u.GetUserReport(domiciles.First().Id, 1, 10)).Returns(users);
      var result = (ViewResult)_controller.Index();

      var model = (UserReportsViewModel)result.Model;

      model.Users.Count.Should().Be(users.Results.Count);
    }

    [Test]
    public void GivenAUserReportsController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
    {
      _userReportService.Setup(u => u.GetUserReport(domiciles.First().Id, 1, 10)).Returns(users);

      var result = (ViewResult)_controller.Index();

      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAUserReportsController_WhenICallItsRunMethod_ThenItReturnsTheCorrectView()
    {
      _userReportService.Setup(u => u.GetUserReport(domiciles.First().Id, 1, 100)).Returns(users);

      var result = (FileContentResult)_controller.Run();

      result.Should().BeOfType<FileContentResult>();
    }
  }
}
