namespace UnityWebTests.Controllers
{
  using System;
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
  public class LogInReportControllerTests
  {
    private LogInReportController _controller;
    private Mock<ILogger> _logger;
    private Mock<ISessionService> _sessionService;
    private Mock<IUserService> _userService;
    //private Mock<ApplicationUser> user1 = new Mock<ApplicationUser>();
    //private Mock<ApplicationUser> user2 = new Mock<ApplicationUser>();
    //private Mock<ApplicationUser> user3 = new Mock<ApplicationUser>();
    //private PagedResult<ApplicationUser> users;

    [SetUp]
    public void SetUp()
    {
      _logger = new Mock<ILogger>();
      _sessionService = new Mock<ISessionService>();
      _userService = new Mock<IUserService>();
      _controller = new LogInReportController(_sessionService.Object, _userService.Object, _logger.Object);//.Object, _userService.Object, _logger.Object);

      //user1.SetupGet(u => u.Roles).Returns(new List<IdentityUserRole> { new IdentityUserRole { Role = new IdentityRole { Name = "name1" } } });
      //user2.SetupGet(u => u.Roles).Returns(new List<IdentityUserRole> { new IdentityUserRole { Role = new IdentityRole { Name = "name2" } } });
      //user3.SetupGet(u => u.Roles).Returns(new List<IdentityUserRole> { new IdentityUserRole { Role = new IdentityRole { Name = "name3" } } });

      //users = new PagedResult<ApplicationUser>
      //{
      //  Results = new List<ApplicationUser>
      //              {
      //                user1.Object, user2.Object, user3.Object
      //              },
      //  CurrentPage = 1,
      //  TotalItems = 20,
      //  EndRow = 10,
      //  StartRow = 1,
      //  ItemsPerPage = 10
      //};

     // _userService.Setup(u => u.GetUserDomiciles()).Returns(domiciles);
    }

    //private readonly List<Domicile> domiciles = new List<Domicile>
    //                                              {
    //                                                new Domicile
    //                                                  {
    //                                                    Id = 1
    //                                                  }
    //                                              };

    [Test]
    public void GivenALogInReportController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
    {
      //_userReportService.Setup(u => u.GetUserReport(domiciles.First().Id, 1, 10)).Returns(users);

      var result = (ViewResult)_controller.Index();

      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenALogInReportController_WhenICallItsRunMethod_ThenItReturnsTheCorrectView()
    {
      var users = new ApplicationUser
                    {
                      Domiciles = new List<ApplicationUserDomicile>()
                                    {
                                      new ApplicationUserDomicile()
                                        {
                                          DomicileId = 1
                                        }
                                    }
                      
                    };

      _userService.Setup(u => u.GetApplicationUser()).Returns(users);
      _sessionService.Setup(u => u.GetSessionsByGovReadOnlyAdmin(It.IsAny<DateTime>(), 1))
                     .Returns(new List<Session>() { new Session()
                                                      {
                                                        ApplicationUser = new ApplicationUser()
                                                                            {
                                                                              UserName = "one",
                                                                            },
                                                                            Start = DateTime.Now.AddHours(-20),
                                                                            End = DateTime.Now.AddHours(-19)
                                                      }, 
                                                      new Session()
                                                        {
                                                          ApplicationUser = new ApplicationUser()
                                                                              {
                                                                                UserName = "two"
                                                                              },
                                                                              Start = DateTime.Now.AddHours(-12),
                                                                              End = DateTime.Now.AddHours(-11)
                                                        } });

      var result = (FileContentResult)_controller.Run();

      result.Should().BeOfType<FileContentResult>();
    }
  }
}
