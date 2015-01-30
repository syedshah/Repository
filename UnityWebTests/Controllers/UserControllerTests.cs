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
  using UnityWeb.Models.Shared;
  using UnityWeb.Models.User;

  [TestFixture]
  public class UserControllerTests : ControllerTestsBase
  {
    private Mock<IUserService> _userService;
    private Mock<IManCoService> _manCoService;
    private Mock<IIdentityRoleService> _identityRoleService;
    private Mock<IPasswordHistoryService> _passwordHistoryService;
    private Mock<ILogger> _logger;
    private UserController _controller;
    private List<Domicile> listDomiciles;
    private List<ApplicationUserDomicile> listUserDomiciles;
    private int pageNumber;
    private int numberOfItems;

    [SetUp]
    public void SetUp()
    {
      _userService = new Mock<IUserService>();
      _manCoService = new Mock<IManCoService>();
      _identityRoleService = new Mock<IIdentityRoleService>();
      _passwordHistoryService = new Mock<IPasswordHistoryService>();
      _logger = new Mock<ILogger>();

      pageNumber = 1;
      numberOfItems = 2;

      _userService.Setup(x => x.GetApplicationUser().Domiciles).Returns(new List<ApplicationUserDomicile>());

      var listDomicileIds = new List<int>();

      var listUsers = new List<ApplicationUser>();

      listUsers.Add(new ApplicationUser());
      listUsers.Add(new ApplicationUser());
      listUsers.Add(new ApplicationUser());
      listUsers.Add(new ApplicationUser());

      var pagedUsers = new PagedResult<ApplicationUser>
      {
        CurrentPage = pageNumber,
        ItemsPerPage = numberOfItems,
        TotalItems = listUsers.Count(),
        Results = listUsers,
        StartRow = ((pageNumber - 1) * numberOfItems) + 1,
        EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
      };

      _userService.Setup(x => x.GetUsersByDomicile(listDomicileIds, It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(pagedUsers);

      listDomiciles = new List<Domicile>();

      listDomiciles.Add(new Domicile { Id = 3 });
      listDomiciles.Add(new Domicile { Id = 4 });
      listDomiciles.Add(new Domicile { Id = 5 });
      listDomiciles.Add(new Domicile { Id = 6 });

      listUserDomiciles = new List<ApplicationUserDomicile>();

      listUserDomiciles.Add(new ApplicationUserDomicile { DomicileId = 3 });

      _controller = new UserController(
          _userService.Object,
          _manCoService.Object,
          _identityRoleService.Object,
          _logger.Object);
    }

    [Test]
    public void GivenAUserController_WhenTheIndexPageIsAccessed_AndAjaxCallIsFalse_ThenTheIndexVieWModelIsReturned()
    {
      var result = _controller.Index(It.IsAny<int>(), false);
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAUserController_WhenTheIndexPageIsAccessed_AndAjaxCallIsTrue_ThenTheIndexVieWModelIsReturned()
    {
      var result = _controller.Index(It.IsAny<int>(), true);
      result.Should().BeOfType<PartialViewResult>();

      var partialViewResult = result as PartialViewResult;

      partialViewResult.Model.Should().BeOfType<UsersViewModel>();
      partialViewResult.ViewName.Should().BeEquivalentTo("_PagedUserResults");
    }

    [Test]
    public void GivenAUserController_WhenTheIndexPageIsAccessed_ThenTheIndexViewShouldContainTheModel()
    {
      var result = (ViewResult)_controller.Index(It.IsAny<int>(), It.IsAny<bool>());
      result.Model.Should().BeOfType<UsersViewModel>();
    }

    [Test]
    public void GivenAUserController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfUsers()
    {
      var result = _controller.Index(It.IsAny<int>(), It.IsAny<bool>()) as ViewResult;

      var model = result.Model as UsersViewModel;

      model.Users.Should().HaveCount(4);
    }

    [Test]
    public void GivenAValidUser_WhenITryAndEditAUser_ThenIGetTheCorrectView()
    {

      var listManCos = new List<ManCo>();

      listManCos.Add(new ManCo());
      listManCos.Add(new ManCo());
      listManCos.Add(new ManCo());
      listManCos.Add(new ManCo());

      var listIdentityRoles = new List<IdentityRole>();

      listIdentityRoles.Add(new IdentityRole());
      listIdentityRoles.Add(new IdentityRole());
      listIdentityRoles.Add(new IdentityRole());

      this._userService.Setup(x => x.GetApplicationUser(It.IsAny<string>())).Returns(new ApplicationUser
      {
        UserName = "roth",
        FirstName = "ryan",
        LastName = "Otherman",
        Email = "roth@google.com"
      });

      this._userService.Setup(x => x.GetApplicationUser().Domiciles).Returns(listUserDomiciles);

      this._manCoService.Setup(x => x.GetManCos(It.IsAny<int>())).Returns(listManCos);

      this._userService.Setup(x => x.GetUserDomiciles()).Returns(listDomiciles);

      this._identityRoleService.Setup(x => x.GetRoles()).Returns(listIdentityRoles);

      this._userService.Setup(x => x.GetRoles(It.IsAny<string>())).Returns(new List<string>());

      ActionResult result = _controller.Edit("roth");
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAValidEditUserViewModel_WhenITryToPerformAnEdit_ThenItEditsAndReturnsTheIndexView()
    {
      var editModel = new EditUserViewModel
      {
        DomicileId = 3,
        Email = "roth@google.com",
        FirstName = "ryan",
        LastName = "Otherman",
        UserName = "roth"
      };

      this._userService.Setup(
          x =>
          x.Updateuser(
              It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<List<int>>(),
              It.IsAny<List<string>>(),
              It.IsAny<Boolean>()));

      ActionResult result = _controller.Edit("Save", editModel);
      result.Should().BeOfType<RedirectToRouteResult>();

      this._userService.Verify(
         x =>
         x.Updateuser(
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<List<int>>(),
             It.IsAny<List<string>>(),
             It.IsAny<Boolean>()), Times.Once);
    }

    [Test]
    public void GivenAnInValidEditUserViewModel_WhenITryToPerformAnEdit_ThenItEditsAndReturnsTheEditView()
    {
      var editModel = new EditUserViewModel();

      _controller.ModelState.AddModelError("FirstName", "Firtsname is required");

      this._userService.Setup(x => x.GetUserDomiciles()).Returns(listDomiciles);

      this._identityRoleService.Setup(x => x.GetRoles()).Returns(new List<IdentityRole>());

      this._manCoService.Setup(x => x.GetManCos(It.IsAny<int>())).Returns(new List<ManCo>());

      editModel.PostedRolesCheckBox = new PostedCheckBox();

      this._userService.Setup(x => x.GetApplicationUser(It.IsAny<string>())).Returns(new ApplicationUser
      {
        UserName = "roth",
        FirstName = "ryan",
        LastName = "Otherman",
        Email = "roth@google.com",
        ManCos = new List<ApplicationUserManCo>()
      });

      ActionResult result = _controller.Edit("Save", editModel);

      result.Should().BeOfType<ViewResult>();

      this._userService.Verify(x => x.GetUserDomiciles(), Times.AtLeastOnce);

      this._identityRoleService.Verify(x => x.GetRoles(), Times.AtLeastOnce);

      this._manCoService.Verify(x => x.GetManCos(It.IsAny<int>()), Times.AtLeastOnce());
    }

    [Test]
    public void GivenAUserController_WhenCreatePageIsAccessed_ThenTheCreateViewModelIsAccessed()
    {
      var userMock = new Mock<ApplicationUser>();
      userMock.SetupGet(u => u.Id).Returns("1");
      userMock.SetupGet(u => u.Domiciles).Returns(listUserDomiciles);

      this._userService.Setup(x => x.GetUserDomiciles()).Returns(listDomiciles);

      this._identityRoleService.Setup(x => x.GetRoles())
          .Returns(
            new List<IdentityRole>()
              {
                new IdentityRole() { Name = "Admin" },
                new IdentityRole() { Name = "Read Only" },
                new IdentityRole() { Name = "Governor" },
                new IdentityRole() { Name = "dstadmin" }
              });

      //this._userService.Setup(x => x.GetApplicationUser().Domiciles).Returns(listUserDomiciles);
      _userService.Setup(m => m.GetApplicationUser()).Returns(userMock.Object);
      this._userService.Setup(s => s.GetRoles(It.IsAny<string>())).Returns(new List<string>() { "Admin" });

      this._manCoService.Setup(x => x.GetManCos(It.IsAny<int>())).Returns(new List<ManCo>());

      var result = _controller.Create();
      result.Should().BeOfType<ViewResult>();

    }

    [Test]
    public void GivenAUserController_WhenAnNTAdminCreatesAser_TheyShouldNotBeAbleToCreateADSTAdminUser()
    {
      var userMock = new Mock<ApplicationUser>();
      userMock.SetupGet(u => u.Id).Returns("1");
      userMock.SetupGet(u => u.Domiciles).Returns(listUserDomiciles);

      this._userService.Setup(x => x.GetUserDomiciles()).Returns(listDomiciles);

      this._identityRoleService.Setup(x => x.GetRoles())
          .Returns(
            new List<IdentityRole>()
              {
                new IdentityRole() { Name = "Admin" },
                new IdentityRole() { Name = "Read Only" },
                new IdentityRole() { Name = "Governor" },
                new IdentityRole() { Name = "dstadmin" }
              });

      //this._userService.Setup(x => x.GetApplicationUser().Domiciles).Returns(listUserDomiciles);
      _userService.Setup(m => m.GetApplicationUser()).Returns(userMock.Object);
      this._userService.Setup(s => s.GetRoles(It.IsAny<string>())).Returns(new List<string>() { "Admin" });

      this._manCoService.Setup(x => x.GetManCos(It.IsAny<int>())).Returns(new List<ManCo>());

      var result = _controller.Create() as ViewResult;
      var model = result.Model as AddUserViewModel;

      model.AvailableRoleItems.Should().NotContain(d => d.Text == "dstadmin");
    }

    [Test]
    public void GivenAUserController_WhenAnNTAdminEditsAser_TheyShouldNotBeAbleToCreateADSTAdminUser()
    {

      var listManCos = new List<ManCo>();

      listManCos.Add(new ManCo());
      listManCos.Add(new ManCo());
      listManCos.Add(new ManCo());
      listManCos.Add(new ManCo());

      var listIdentityRoles = new List<IdentityRole>();

      listIdentityRoles.Add(new IdentityRole());
      listIdentityRoles.Add(new IdentityRole());
      listIdentityRoles.Add(new IdentityRole());

      this._userService.Setup(x => x.GetApplicationUser(It.IsAny<string>())).Returns(new ApplicationUser
      {
        UserName = "roth",
        FirstName = "ryan",
        LastName = "Otherman",
        Email = "roth@google.com"
      });

      this._userService.Setup(x => x.GetApplicationUser().Domiciles).Returns(listUserDomiciles);

      this._manCoService.Setup(x => x.GetManCos(It.IsAny<int>())).Returns(listManCos);

      this._userService.Setup(x => x.GetUserDomiciles()).Returns(listDomiciles);

      this._identityRoleService.Setup(x => x.GetRoles())
    .Returns(
      new List<IdentityRole>()
              {
                new IdentityRole() { Name = "Admin" },
                new IdentityRole() { Name = "Read Only" },
                new IdentityRole() { Name = "Governor" },
                new IdentityRole() { Name = "dstadmin" }
              });

      this._userService.Setup(s => s.GetRoles(It.IsAny<string>())).Returns(new List<string>() { "Admin" });

      //this._userService.Setup(x => x.GetRoles(It.IsAny<string>())).Returns(new List<string>());

      var result = _controller.Edit("roth") as ViewResult;
      var model = result.Model as EditUserViewModel;

      model.AvailableRoleItems.Should().NotContain(d => d.Text == "dstadmin");
    }

    [Test]
    public void GivenAUserController_WhenCreatePageIsAccessed_ThenTheCreateViewShouldContainTheModel()
    {
      var userMock = new Mock<ApplicationUser>();
      userMock.SetupGet(u => u.Id).Returns("1");
      userMock.SetupGet(u => u.Domiciles).Returns(listUserDomiciles);

      this._userService.Setup(x => x.GetUserDomiciles()).Returns(listDomiciles);

      this._identityRoleService.Setup(x => x.GetRoles())
          .Returns(
            new List<IdentityRole>()
              {
                new IdentityRole() { Name = "Admin" },
                new IdentityRole() { Name = "Read Only" },
                new IdentityRole() { Name = "Governor" },
                new IdentityRole() { Name = "dstadmin" }
              });

      //this._userService.Setup(x => x.GetApplicationUser().Domiciles).Returns(listUserDomiciles);
      _userService.Setup(m => m.GetApplicationUser()).Returns(userMock.Object);
      this._userService.Setup(s => s.GetRoles(It.IsAny<string>())).Returns(new List<string>() { "Admin" });

      this._manCoService.Setup(x => x.GetManCos(It.IsAny<int>())).Returns(new List<ManCo>());

      var result = (ViewResult)_controller.Create();
      result.Model.Should().BeOfType<AddUserViewModel>();
    }

    [Test]
    public void GivenAValidAddUserViewModel_WhenITryToPerformACreateUser_ThenItCreatesAndReturnsTheIndexView()
    {
      var addModel = new AddUserViewModel { };

      this._userService.Setup(x => x.CreateUser(
           It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<List<int>>(),
              It.IsAny<int>(),
              It.IsAny<List<string>>()));

      ActionResult result = _controller.Create(addModel);
      result.Should().BeOfType<RedirectToRouteResult>();

      this._userService.Verify(x => x.CreateUser(
          It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<List<int>>(),
             It.IsAny<int>(),
             It.IsAny<List<string>>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenAnInValidAddUserViewModel_WhenITryToPerformACreateUser_ThenItCreatesAndReturnsTheIndexView()
    {
      var addModel = new AddUserViewModel { };

      _controller.ModelState.AddModelError("FirstName", "Firtsname is required");

      addModel.UserManCoViewModel = new UserManCoViewModel();

      addModel.PostedRolesCheckBox = new PostedCheckBox();

      this._userService.Setup(x => x.GetApplicationUser().Domiciles).Returns(listUserDomiciles);

      this._manCoService.Setup(x => x.GetManCos(It.IsAny<int>())).Returns(new List<ManCo>());

      this._identityRoleService.Setup(x => x.GetRoles()).Returns(new List<IdentityRole>());

      ActionResult result = _controller.Create(addModel);
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAValidDomicileId_WhenITryToRetrieveMancos_ApartialViewOfMancoCheckBoxListIsReturned()
    {
      this._manCoService.Setup(x => x.GetManCos(It.IsAny<int>())).Returns(new List<ManCo>());
      this._userService.Setup(x => x.GetApplicationUser(It.IsAny<string>())).Returns(new ApplicationUser
      {
        UserName = "roth",
        FirstName = "ryan",
        LastName = "Otherman",
        Email = "roth@google.com",
        ManCos = new List<ApplicationUserManCo>()
      });

      var result = this._controller.RetrieveManCoes(It.IsAny<string>(), It.IsAny<string>());

      result.Should().BeOfType<PartialViewResult>();
    }

    [Test]
    public void GivenAnInValidDomicileId_WhenITryToRetrieveMancos_AJsonResultWithErrorIsReturned()
    {
      this._manCoService.Setup(x => x.GetManCos(It.IsAny<int>())).Throws<Exception>();

      var result = this._controller.RetrieveManCoes(It.IsAny<string>(), It.IsAny<string>());

      result.Should().BeOfType<JsonResult>();
    }

    [Test]
    public void GivenAnInValidUserName_WhenITryToDeleteAuser_AUserIsDeleted()
    {
      this._userService.Setup(x => x.DeactivateUser(It.IsAny<string>()));

      var result = this._controller.Delete(It.IsAny<string>());

      var redirectToRouteResult = result as RedirectToRouteResult;

      result.Should().BeOfType<RedirectToRouteResult>();
      redirectToRouteResult.RouteValues["Action"] = "Index";
      redirectToRouteResult.RouteValues["Controller"] = "User";
    }
  }
}
