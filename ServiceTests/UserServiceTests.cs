namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using System.Security.Claims;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using IdentityWrapper.Interfaces;
  using Microsoft.AspNet.Identity.EntityFramework;
  using Moq;
  using NUnit.Framework;
  using Services;
  using UnityRepository.Interfaces;
  using Microsoft.AspNet.Identity;
  using Microsoft.Owin.Security;

  [TestFixture]
  public class UserServiceTests
  {
    private Mock<IUserManagerProvider> _userManagerProvider;
    private Mock<IRoleManagerProvider> _roleManagerProvider;
    private Mock<IAuthenticationManagerProvider> _authenticationManagerProvider;
    private Mock<IApplicationUserRepository> _applicationUserRepository;
    private Mock<IManCoRepository> _manCoRepository;
    private Mock<IPasswordHistoryRepository> _passwordHistoryRepository;
    private Mock<IGlobalSettingRepository> _globalSettingRepository;
    private Mock<ISessionRepository> _sessionRepository;
    private UserService _userService;

    [SetUp]
    public void SetUp()
    {
      this._userManagerProvider = new Mock<IUserManagerProvider>();
      this._roleManagerProvider = new Mock<IRoleManagerProvider>();
      this._authenticationManagerProvider = new Mock<IAuthenticationManagerProvider>();
      _applicationUserRepository = new Mock<IApplicationUserRepository>();
      _passwordHistoryRepository = new Mock<IPasswordHistoryRepository>();
      _globalSettingRepository = new Mock<IGlobalSettingRepository>();
      _manCoRepository = new Mock<IManCoRepository>();
      _sessionRepository = new Mock<ISessionRepository>();
      _userService = new UserService(
          this._userManagerProvider.Object,
          this._roleManagerProvider.Object,
          this._authenticationManagerProvider.Object,
          _applicationUserRepository.Object,
          _passwordHistoryRepository.Object,
          _globalSettingRepository.Object,
          _manCoRepository.Object,
          _sessionRepository.Object);
    }

    [Test]
    public void WhenIWantToGetAllUsers_AndDatabaseIsAvailable_AllUsersAreRetrieved()
    {
      this._applicationUserRepository.Setup(x => x.GetUsers()).Returns(new List<ApplicationUser>());

      this._userService.GetAllUsers();

      this._applicationUserRepository.Verify(x => x.GetUsers(), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToGetAllUsers_AndDatabaseIsUnAvailable_AUnityExceptionShouldBeThrown()
    {
      this._applicationUserRepository.Setup(x => x.GetUsers()).Throws(new UnityException());

      this._userService.GetAllUsers();

      this._applicationUserRepository.Verify(x => x.GetUsers(), Times.AtLeastOnce);
    }

    [Test]
    public void WithNoParameter_AndDatabaseIsAvailable_WhenIWantToFindTheCurrentuser_TheUserIsRetrieved()
    {
      var identity = new ClaimsIdentity { };
      identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "broth"));
      var user = new ClaimsPrincipal(identity);

      this._authenticationManagerProvider.SetupGet(x => x.User).Returns(user);

      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>()))
            .Returns(new ApplicationUser());

      var result = this._userService.GetApplicationUser();

      this._authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);

      result.Should().NotBeNull();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WithNoParameter_AndDatabaseIsUnAvailable_WhenIWantToFindTheCurrentuser_AUnityExceptionIsThrown()
    {
      this._authenticationManagerProvider.SetupGet(x => x.User).Returns(new ClaimsPrincipal());

      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>())).Throws(new UnityException());

      this._userService.GetApplicationUser();

      this._authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

      this._userManagerProvider.Verify(x => x.Find(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Test]
    public void GivenNoParameters_AndDatabaseIsAvailable_WhenIWantToGetCurrentUsersMancoIds_TheManCoIdsAreRetrieved()
    {
      var identity = new ClaimsIdentity { };
      identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "broth"));
      var user = new ClaimsPrincipal(identity);

      this._authenticationManagerProvider.SetupGet(x => x.User).Returns(user);

      var appUser = new ApplicationUser();
      appUser.ManCos.Add(new ApplicationUserManCo() { ManCo = new ManCo("code1", "description1") });
      appUser.ManCos.Add(new ApplicationUserManCo() { ManCo = new ManCo("code2", "description2") });

      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>()))
            .Returns(appUser);

      var result = this._userService.GetUserManCoIds();

      result.Should().NotBeNull();

      this._authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);

    }


    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenNoParameters_AndDatabaseIsUnAvailable_WhenIWantToGetCurrentUsersMancoIds_AUnityExceptionIsThrown()
    {
      var identity = new ClaimsIdentity { };
      identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "broth"));
      var user = new ClaimsPrincipal(identity);

      this._authenticationManagerProvider.SetupGet(x => x.User).Returns(user);

      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>()))
            .Throws(new UnityException());

      this._userService.GetUserManCoIds();

      this._authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);

    }


    [Test]
    public void GivenNoParameters_AndDatabaseIsAvailable_WhenIWantToGetCurrentUsersMancos_TheManCosAreRetrieved()
    {
      var identity = new ClaimsIdentity { };
      identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "broth"));
      var user = new ClaimsPrincipal(identity);

      this._authenticationManagerProvider.SetupGet(x => x.User).Returns(user);

      var appUser = new ApplicationUser();
      appUser.ManCos.Add(new ApplicationUserManCo() { ManCo = new ManCo("code1", "description1") });
      appUser.ManCos.Add(new ApplicationUserManCo() { ManCo = new ManCo("code2", "description2") });

      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>()))
            .Returns(appUser);

      var result = this._userService.GetUserManCos();

      result.Should().NotBeNull();

      this._authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);

    }


    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenNoParameters_AndDatabaseIsUnAvailable_WhenIWantToGetCurrentUsersMancos_AUnityExceptionIsThrown()
    {
      var identity = new ClaimsIdentity { };
      identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "broth"));
      var user = new ClaimsPrincipal(identity);

      this._authenticationManagerProvider.SetupGet(x => x.User).Returns(user);

      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>()))
            .Throws(new UnityException());

      this._userService.GetUserManCos();

      this._authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);

    }

    [Test]
    public void GivenNoParameters_AndDatabaseIsAvailable_WhenIWantToGetCurrentUsersDomiciles_TheDomicilesAreRetrieved()
    {
      var identity = new ClaimsIdentity { };
      identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "broth"));
      var user = new ClaimsPrincipal(identity);

      this._authenticationManagerProvider.SetupGet(x => x.User).Returns(user);

      var appUser = new ApplicationUser();
      appUser.Domiciles.Add(new ApplicationUserDomicile() { Domicile = new Domicile("code1", "desc 1") });
      appUser.Domiciles.Add(new ApplicationUserDomicile() { Domicile = new Domicile("code2", "desc 2") });

      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>()))
            .Returns(appUser);

      var result = this._userService.GetUserDomiciles();

      result.Should().NotBeNull();

      this._authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);
    }


    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenNoParameters_AndDatabaseIsUnAvailable_WhenIWantToGetCurrentUsersDomiciles_AUnityExceptionIsThrown()
    {
      var identity = new ClaimsIdentity { };
      identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "broth"));
      var user = new ClaimsPrincipal(identity);

      this._authenticationManagerProvider.SetupGet(x => x.User).Returns(user);

      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>()))
            .Throws(new UnityException());

      this._userService.GetUserDomiciles();

      this._authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenAUserName_AndDatabaseIsAvailable_WhenIWantToFindAUserByUserNameAndPassword_TheUserIsRetrieved()
    {
      this._userManagerProvider.Setup(x => x.Find(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ApplicationUser());

      this._applicationUserRepository.Setup(x => x.IsLockedOut(It.IsAny<string>())).Returns(false);

      var result = this._userService.GetApplicationUser(It.IsAny<string>(), It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.Find(It.IsAny<string>(), It.IsAny<string>()));
      result.Should().NotBeNull();
      result.Should().BeOfType<ApplicationUser>();
    }

    [Test]
    public void GivenAUserName_AndDatabaseIsAvailable_WhenIWantToFindAUserByUserNameAndWrongPassword_TheUserIsNotRetrieved()
    {
      this._userManagerProvider.Setup(x => x.Find(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(It.IsAny<ApplicationUser>());

      this._applicationUserRepository.Setup(x => x.IsLockedOut(It.IsAny<string>())).Returns(false);

      var result = this._userService.GetApplicationUser(It.IsAny<string>(), It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.Find(It.IsAny<string>(), It.IsAny<string>()));

      this._applicationUserRepository.Verify(x => x.IsLockedOut(It.IsAny<string>()), Times.Never);

      result.Should().BeNull();
    }

    [Test]
    public void GivenAUserNameAndPassword_AndDatabaseIsAvailable_AndUserIsLockedOut_WhenIWantToFindAUserByUserNameAndPassword_NullIsRetrieved()
    {
      this._userManagerProvider.Setup(x => x.Find(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ApplicationUser { IsLockedOut = true });

      this._applicationUserRepository.Setup(x => x.IsLockedOut(It.IsAny<string>())).Returns(true);

      var result = this._userService.GetApplicationUser(It.IsAny<string>(), It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.Find(It.IsAny<string>(), It.IsAny<string>()));

      result.IsLockedOut.Should().BeTrue();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAUserNameAndPassword_AndDatabaseIsUnAvailable_WhenIWantToFindAUserByUserNameAndPassword_AUnityExceptionIsThrown()
    {
      this._userManagerProvider.Setup(x => x.Find(It.IsAny<string>(), It.IsAny<string>())).Throws(new UnityException());

      this._userService.GetApplicationUser(It.IsAny<string>(), It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.Find(It.IsAny<string>(), It.IsAny<string>()));
    }


    [Test]
    public void GivenAUserName_AndDatabaseIsAvailable_AndUserIsNotLockedOut_WhenIWantToFindAUserByUserName_TheUserIsRetrieved()
    {
      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>()))
            .Returns(new ApplicationUser { IsLockedOut = false });

      var result = this._userService.GetApplicationUser(It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()));

      result.Should().BeOfType<ApplicationUser>();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAUserName_AndDatabaseIsUnAvailable_WhenIWantToFindAUserByUserName_AUnityExceptionIsThrown()
    {
      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>())).Throws(new UnityException());

      this._userService.GetApplicationUser(It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()));
    }

    [Test]
    public void GivenAUserName_AndDatabaseIsAvailable_WhenIWantToFindAUser_TheUserIsRetrieved()
    {
      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>()))
            .Returns(new ApplicationUser());

      var result = this._userService.GetApplicationUser(It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()));

      result.Should().NotBeNull();
      result.Should().BeOfType<ApplicationUser>();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAUserName_AndDatabaseIsUnAvailable_WhenIWantToFindAUser_ThenAUnityExceptionIsThrown()
    {
      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>())).Throws(new UnityException());

      this._userService.GetApplicationUser(It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()));
    }


    [Test]
    public void GivenAUserId_AndDatabaseIsAvailable_WhenIWantToGetRolesAUserIsIn_ThenTheRolesIsRetrieved()
    {
      this._userManagerProvider.Setup(x => x.GetRoles(It.IsAny<string>()))
            .Returns(new List<string>());

      var result = this._userService.GetRoles(It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.GetRoles(It.IsAny<string>()));

      result.Should().NotBeNull();
      result.Should().BeOfType<List<string>>();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAUserId_AndDatabaseIsUnAvailable_WhenIWantToGetRolesAUserIsIn_ThenAUnityExceptionIsThrown()
    {
      this._userManagerProvider.Setup(x => x.GetRoles(It.IsAny<string>())).Throws(new UnityException());

      this._userService.GetRoles(It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.GetRoles(It.IsAny<string>()));
    }

    [Test]
    public void WhenIWanSignInAUser_TheUserIsSignedIn()
    {
      this._authenticationManagerProvider.Setup(x => x.SignOut(It.IsAny<string>()));

      this._userManagerProvider.Setup(x => x.CreateIdentity(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
          .Returns(new ClaimsIdentity());

      this._authenticationManagerProvider.Setup(x => x.SignIn(It.IsAny<AuthenticationProperties>(), It.IsAny<ClaimsIdentity>()));

      this._userService.SignIn(It.IsAny<ApplicationUser>(), It.IsAny<bool>());

      this._authenticationManagerProvider.Verify(x => x.SignOut(It.IsAny<string>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.CreateIdentity(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.AtLeastOnce);

      this._authenticationManagerProvider.Verify(x => x.SignIn(It.IsAny<AuthenticationProperties>(), It.IsAny<ClaimsIdentity>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenValidParameters_AndDatabaseIsAvailable_WhenIWantToCreateUser_TheUserIsCreated()
    {

      var listManCos = new List<ManCo>();

      listManCos.Add(new ManCo { Code = "code1", Description = "descr 1", Id = 1 });
      listManCos.Add(new ManCo { Code = "code2", Description = "descr 2", Id = 2 });
      listManCos.Add(new ManCo { Code = "code3", Description = "descr 3", Id = 3 });

      this._manCoRepository.Setup(x => x.GetManCos(It.IsAny<List<int>>())).Returns(listManCos);

      string[] errors = new string[0];
      var idenResult = new IdentityResult(errors);

      this._userManagerProvider.Setup(x => x.Create(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .Returns(idenResult);

      this._passwordHistoryRepository.Setup(x => x.Create(It.IsAny<PasswordHistory>()));

      var listRoles = new List<string>();

      listRoles.Add("Admin");
      listRoles.Add("Governor");

      this._roleManagerProvider.Setup(x => x.FindById(It.IsAny<string>())).Returns(new IdentityRole { Id = "jfew", Name = "Admin" });

      this._userManagerProvider.Setup(x => x.AddToRole(It.IsAny<string>(), It.IsAny<string>()));

      var listMancoIds = new List<int>();

      listMancoIds.Add(1);
      listMancoIds.Add(3);
      listMancoIds.Add(6);

      var listIdentityRoleId = new List<string>();
      listIdentityRoleId.Add("tdgdg");
      listIdentityRoleId.Add("tddeeegdg");

      this._userService.CreateUser("broth", "ageless", "bertrand", "roth", "broth@gmail.com", listMancoIds, 6, listIdentityRoleId);

      this._manCoRepository.Verify(x => x.GetManCos(It.IsAny<List<int>>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.Create(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.AtLeastOnce);

      this._roleManagerProvider.Verify(x => x.FindById(It.IsAny<string>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.AddToRole(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenValidParameters_AndDatabaseIsAvailable_WhenIWantToUpdateUser_TheUserIsUpdated()
    {
      var user = new ApplicationUser("broth");

      user.FirstName = "Bertrand";
      user.LastName = "Roth";
      user.Email = "broth@gmail.com";

      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>())).Returns(user);

      var listManCos = new List<ManCo>();

      listManCos.Add(new ManCo { Code = "code1", Description = "descr 1", Id = 1 });
      listManCos.Add(new ManCo { Code = "code2", Description = "descr 2", Id = 2 });
      listManCos.Add(new ManCo { Code = "code3", Description = "descr 3", Id = 3 });

      this._manCoRepository.Setup(x => x.GetManCos(It.IsAny<List<int>>())).Returns(listManCos);

      this._applicationUserRepository.Setup(x => x.UpdateUserMancos(user.Id, listManCos));

      string[] errors = new string[0];
      var idenResult = new IdentityResult(errors);

      this._userManagerProvider.Setup(x => x.Update(It.IsAny<ApplicationUser>())).Returns(idenResult);

      this._userManagerProvider.Setup(x => x.RemovePassword(It.IsAny<string>())).Returns(idenResult);

      this._userManagerProvider.Setup(x => x.AddPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(idenResult);

      this._passwordHistoryRepository.Setup(x => x.Create(It.IsAny<PasswordHistory>()));

      var listRoles = new List<string>();

      listRoles.Add("Admin");
      listRoles.Add("Governor");

      this._userManagerProvider.Setup(x => x.GetRoles(It.IsAny<string>())).Returns(listRoles);

      this._userManagerProvider.Setup(x => x.RemoveFromRole(It.IsAny<string>(), It.IsAny<string>()));

      this._roleManagerProvider.Setup(x => x.FindById(It.IsAny<string>())).Returns(new IdentityRole { Id = "jfew", Name = "Admin" });

      this._userManagerProvider.Setup(x => x.AddToRole(It.IsAny<string>(), It.IsAny<string>()));

      var listMancoIds = new List<int>();

      listMancoIds.Add(1);
      listMancoIds.Add(3);
      listMancoIds.Add(6);

      var listIdentityRoleId = new List<string>();
      listIdentityRoleId.Add("tdgdg");
      listIdentityRoleId.Add("tddeeegdg");

      this._userService.Updateuser("broth", "ageless", "bertrand", "roth", "broth@gmail.com", listMancoIds, listIdentityRoleId, true);

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);

      this._manCoRepository.Verify(x => x.GetManCos(It.IsAny<List<int>>()), Times.AtLeastOnce);

      this._applicationUserRepository.Verify(x => x.UpdateUserMancos(user.Id, listManCos), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.Update(It.IsAny<ApplicationUser>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.RemovePassword(It.IsAny<string>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.AddPassword(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.GetRoles(It.IsAny<string>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.RemoveFromRole(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);

      this._roleManagerProvider.Verify(x => x.FindById(It.IsAny<string>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.AddToRole(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenAValidUserId_WhenIWantToUpdateUserLastLoginDate_AndDatabaseIsAvailable_TheLastLoginDateIsUpdated()
    {
      this._applicationUserRepository.Setup(x => x.UpdateUserlastLogindate(It.IsAny<string>()));

      this._userService.UpdateUserLastLogindate(It.IsAny<string>());

      this._applicationUserRepository.Verify(x => x.UpdateUserlastLogindate(It.IsAny<string>()));
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAValidUserId_WhenIWantToUpdateUserLastLoginDate_AndDatabaseIsUnAvailable_ThenAunityExceptionIsThrown()
    {
      this._applicationUserRepository.Setup(x => x.UpdateUserlastLogindate(It.IsAny<string>()))
            .Throws<UnityException>();

      this._userService.UpdateUserLastLogindate(It.IsAny<string>());

      this._applicationUserRepository.Verify(x => x.UpdateUserlastLogindate(It.IsAny<string>()));
    }

    [Test]
    public void GivenAValidUserId_WhenIWantToUnlockTheUser_TheUserIsUnlocked()
    {
      this._applicationUserRepository.Setup(x => x.UnlockUser(It.IsAny<string>()));

      this._userService.UnlockUser(It.IsAny<string>());

      this._applicationUserRepository.Verify(x => x.UnlockUser(It.IsAny<string>()));
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAValidUserId_WhenIWantToUnlockTheUser_AndDatabaseUnAvailable_TheUserIsUnlocked()
    {
      this._applicationUserRepository.Setup(x => x.UnlockUser(It.IsAny<string>()))
            .Throws<UnityException>();

      this._userService.UnlockUser(It.IsAny<string>());

      this._applicationUserRepository.Verify(x => x.UnlockUser(It.IsAny<string>()));
    }

    [Test]
    public void GivenValidData_WhenMyPasswordHasBeenRenewedWithinTheLast30Days_ThenThePasswordDoesntNeedResetting()
    {
      _globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { PasswordExpDays = 30, NewUserPasswordReset = true });
      bool result = _userService.CheckForPassRenewal(DateTime.Now.AddDays(-29), DateTime.Now.AddDays(-1));

      result.Should().BeFalse();
    }

    [Test]
    public void GivenValidData_WhenMyPasswordHasntBeenRenewedFor30Days_ThenThePasswordNeedsResetting()
    {
      _globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { PasswordExpDays = 30, NewUserPasswordReset = true });
      bool result = _userService.CheckForPassRenewal(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-1));

      result.Should().BeTrue();
    }

    [Test]
    public void GivenValidData_WhenIAmANewUserAndPasswordRenewalIsSetForNewUsers_ThenThePasswordNeedsResetting()
    {
      _globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { PasswordExpDays = 30, NewUserPasswordReset = true });
      bool result = _userService.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>());

      result.Should().BeTrue();
    }

    [Test]
    public void GivenValidDomicileIds_WhenItryToRetrieveUsers_AndDatabaseIsUnavailable_ThenAnUnityExceptionIsThrown()
    {
      _applicationUserRepository.Setup(m => m.GetUsersByDomicile(It.IsAny<List<int>>())).Throws<Exception>();
      Action act = () => _userService.GetUsersByDomicile(It.IsAny<List<int>>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidDomicileIds_WhenItryToRetrieveUsersFromTheDatabase_theusersAreRetireved()
    {
      _applicationUserRepository.Setup(m => m.GetUsersByDomicile(It.IsAny<List<int>>()))
                                  .Returns(new List<ApplicationUser>());
      var result = _userService.GetUsersByDomicile(It.IsAny<List<int>>());

      result.Should().NotBeNull();
    }

    [Test]
    public void GivenValidDomicileIdsPageNumberAndNumberOfItems_WhenItryToRetrievePagedUsers_AndDatabaseIsUnavailable_ThenAnUnityExceptionIsThrown()
    {
      _applicationUserRepository.Setup(m => m.GetUsersByDomicile(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();
      Action act = () => _userService.GetUsersByDomicile(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidDomicileIdsPageNumberAndNumberOfItems_WhenItryToRetrievePagedUsersFromTheDatabase_ThePagedUsersAreRetireved()
    {
      _applicationUserRepository.Setup(m => m.GetUsersByDomicile(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()))
                                  .Returns(new PagedResult<ApplicationUser>());
      var result = _userService.GetUsersByDomicile(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>());

      result.Should().NotBeNull();
    }


    [Test]
    public void GivenAuserIdAndPassword_WhenITryToChangePassword_AndDatabaseIsAvailable_ThenTheUserPasswordIsChanged()
    {
      this._userManagerProvider.Setup(x => x.RemovePassword("jddj"));

      this._userManagerProvider.Setup(x => x.AddPassword("jddj", "newpassword"));

      this._applicationUserRepository.Setup(x => x.UpdateUserLastPasswordChangedDate(It.IsAny<string>()));

      this._passwordHistoryRepository.Setup(x => x.Create(It.IsAny<PasswordHistory>()));

      this._userService.ChangePassword("jddj", "newpassword");

      this._userManagerProvider.Verify(x => x.RemovePassword(It.IsAny<string>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.AddPassword(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);

      this._applicationUserRepository.Verify(x => x.UpdateUserLastPasswordChangedDate(It.IsAny<string>()));

      this._passwordHistoryRepository.Verify(x => x.Create(It.IsAny<PasswordHistory>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAuserIdAndPassword_WhenITryToChangePassword_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._userManagerProvider.Setup(x => x.RemovePassword(It.IsAny<string>()));

      this._userManagerProvider.Setup(x => x.AddPassword(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

      this._userService.ChangePassword(It.IsAny<string>(), It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.RemovePassword(It.IsAny<string>()), Times.AtLeastOnce);

      this._userManagerProvider.Verify(x => x.AddPassword(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);

    }

    [Test]
    public void GivenAuserIdCurrentPasswordAndNewPassword_WhenITryToChangePassword_AndDatabaseIsAvailable_ThenTheUserPasswordIsChanged()
    {
      this._userManagerProvider.Setup(x => x.ChangePassword("jddj", "currentpassword", "newpassword"));

      this._applicationUserRepository.Setup(x => x.UpdateUserLastPasswordChangedDate(It.IsAny<string>()));

      this._passwordHistoryRepository.Setup(x => x.Create(It.IsAny<PasswordHistory>()));

      this._userService.ChangePassword("jddj", "currentpassword", "newpassword");

      this._userManagerProvider.Verify(x => x.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);

      this._applicationUserRepository.Verify(x => x.UpdateUserLastPasswordChangedDate(It.IsAny<string>()));

      this._passwordHistoryRepository.Verify(x => x.Create(It.IsAny<PasswordHistory>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAuserIdCurrentPasswordAndNewPassword_WhenITryToChangePassword_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._userManagerProvider.Setup(x => x.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

      this._userService.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenAuserId_WhenITryToRetrieveAUser_AndDatabaseIsAvailable_ThenTheUserIsRetrieved()
    {
      this._userManagerProvider.Setup(x => x.FindById(It.IsAny<string>())).Returns(new ApplicationUser());

      var result = this._userService.GetApplicationUserById(It.IsAny<string>());

      result.Should().NotBeNull();

      this._userManagerProvider.Verify(x => x.FindById(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAuserId_WhenITryToRetrieveAUser_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._userManagerProvider.Setup(x => x.FindById(It.IsAny<string>())).Throws<Exception>();

      this._userService.GetApplicationUserById(It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.FindById(It.IsAny<string>()), Times.AtLeastOnce);
    }


    [Test]
    public void GivenAuserName_WhenITryToDeactivateAUser_AndDatabaseIsAvailable_ThenTheUserIsDeactivated()
    {
      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>())).Returns(new ApplicationUser());

      this._applicationUserRepository.Setup(x => x.DeactivateUser(It.IsAny<string>()));

      this._userService.DeactivateUser(It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);

      this._applicationUserRepository.Verify(x => x.DeactivateUser(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenAuserName_WhenITryToDeactivateAUser_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>())).Returns(new ApplicationUser());

      this._applicationUserRepository.Setup(x => x.DeactivateUser(It.IsAny<string>())).Throws<Exception>();

      this._userService.DeactivateUser(It.IsAny<string>());

      this._userManagerProvider.Verify(x => x.FindByName(It.IsAny<string>()), Times.AtLeastOnce);

      this._applicationUserRepository.Verify(x => x.DeactivateUser(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheFailedLogInCount_AndNotReachedTheMaxAttempts_ThenTheFailedLogInCountIsUpdatedAndTheUserIsNotLockedOut()
    {
      _applicationUserRepository.Setup(c => c.UpdateUserFailedLogin(It.IsAny<string>())).Returns(new ApplicationUser() { FailedLogInCount = 2 });
      _globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { MaxLogInAttempts = 3 });
      _userService.UpdateUserFailedLogin(It.IsAny<string>());
      _applicationUserRepository.Verify(s => s.DeactivateUser(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheFailedLogInCount_AndTheDatabaseIsUnavailable_ThenTheFailedLogInCountIsUpdated()
    {
      _globalSettingRepository.Setup(c => c.Get()).Throws<Exception>();
      Action act = () => _userService.UpdateUserFailedLogin(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheFailedLogInCount_AndTheUserHasReachedTheMaxAttempts_ThenTheUserIsLockedOut()
    {
      _applicationUserRepository.Setup(c => c.UpdateUserFailedLogin(It.IsAny<string>())).Returns(new ApplicationUser() { FailedLogInCount = 3 });
      _globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { MaxLogInAttempts = 3 });
      _userService.UpdateUserFailedLogin(It.IsAny<string>());
      _applicationUserRepository.Verify(s => s.DeactivateUser(It.IsAny<string>()), Times.Once());
    }
  }
}
