namespace EFRepositoryTests
{
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using System.Linq;
  using System.Transactions;
  using Builder;
  using Entities;
  using Microsoft.AspNet.Identity.EntityFramework;
  using NUnit.Framework;
  using FluentAssertions;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class ApplicationUserRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _applicationUserRepository = new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _manCoRepository = new ManCoRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _domicileRepository = new DomicileRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _roleAdmin = BuildMeA.IdentityRole("TestAdmin");
      _roleStandard = BuildMeA.IdentityRole("TestStandard");
      _domicile1 = BuildMeA.Domicile("code", "description");
      _domicile2 = BuildMeA.Domicile("code2", "description2");
      _domicile3 = BuildMeA.Domicile("code3", "description3");
      _domicile4 = BuildMeA.Domicile("code4", "description4");
      _applicationUser = BuildMeA.ApplicationUser("tron");
      _applicationUser2 = BuildMeA.ApplicationUser("tron2");
      _applicationUser3 = BuildMeA.ApplicationUser("flynn");
      _applicationUser4 = BuildMeA.ApplicationUser("flynn2");
      _applicationUser.Roles.Add(new IdentityUserRole()
      {
        Role = _roleAdmin,
        RoleId = _roleAdmin.Id,
        User = _applicationUser,
        UserId = _applicationUser.Id
      });
      _applicationUser2.Roles.Add(new IdentityUserRole()
      {
        Role = _roleStandard,
        RoleId = _roleStandard.Id,
        User = _applicationUser2,
        UserId = _applicationUser2.Id
      });

      _applicationUser3.Roles.Add(new IdentityUserRole()
      {
        Role = _roleStandard,
        RoleId = _roleStandard.Id,
        User = _applicationUser3,
        UserId = _applicationUser3.Id
      });

      _applicationUser4.Roles.Add(new IdentityUserRole()
      {
        Role = _roleAdmin,
        RoleId = _roleAdmin.Id,
        User = _applicationUser4,
        UserId = _applicationUser4.Id
      });

      _manCo1 = BuildMeA.ManCo("description1", "code1").WithDomicile(_domicile1); ;
      _manCo2 = BuildMeA.ManCo("description2", "code2").WithDomicile(_domicile1); ;
      _manCo3 = BuildMeA.ManCo("description3", "code3").WithDomicile(_domicile1); ;
      _manCo4 = BuildMeA.ManCo("description4", "code4").WithDomicile(_domicile1); ;
      _manCoList = new List<ManCo>();
      _domicileList = new List<Domicile>();
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private ApplicationUserRepository _applicationUserRepository;
    private DomicileRepository _domicileRepository;
    private ManCoRepository _manCoRepository;
    private ApplicationUser _applicationUser;
    private ApplicationUser _applicationUser2;
    private ApplicationUser _applicationUser3;
    private ApplicationUser _applicationUser4;
    private Domicile _domicile1;
    private Domicile _domicile2;
    private Domicile _domicile3;
    private Domicile _domicile4;
    private ManCo _manCo1;
    private ManCo _manCo2;
    private ManCo _manCo3;
    private ManCo _manCo4;
    private List<ManCo> _manCoList;
    private List<Domicile> _domicileList;
    private IdentityRole _roleAdmin;
    private IdentityRole _roleStandard;

    [Test]
    public void GivenAUserName_WhenITryToSearchForItByUserName_ItIsRetrievedFromTheDatabase()
    {
      this._applicationUserRepository.Create(this._applicationUser);

      var user = this._applicationUserRepository.GetUserByName(this._applicationUser.UserName);

      user.Should().NotBeNull();
      user.UserName.Should().Be(this._applicationUser.UserName);
    }

    [Test]
    public void GivenAUserIdAndMancoList_WhenITryToUpdateUserManco_MancosForTheUserIsSavedToDatabase()
    {
      this._applicationUserRepository.Create(this._applicationUser2);

      this._domicileRepository.Create(_domicile1);

      this._manCoRepository.Create(_manCo1);
      this._manCoRepository.Create(_manCo2);
      this._manCoRepository.Create(_manCo3);
      this._manCoRepository.Create(_manCo4);

      this._manCoList.Add(_manCo1);
      this._manCoList.Add(_manCo2);
      this._manCoList.Add(_manCo3);
      this._manCoList.Add(_manCo4);

      this._applicationUserRepository.UpdateUserMancos(this._applicationUser2.Id, this._manCoList);

      var user = this._applicationUserRepository.GetUserByName("tron2");

      user.ManCos.Count.Should().Be(4);
      user.ManCos[1].ManCo.DomicileId.Should().Be(this._domicile1.Id);

    }

    [Test]
    public void GivenAUserIdAndMancoList_WhenITryToUpdateExistingUserManco_MancosForTheUserIsRemovedOrAddedd()
    {
      this._applicationUserRepository.Create(this._applicationUser2);

      this._domicileRepository.Create(_domicile1);

      this._manCoRepository.Create(_manCo1);
      this._manCoRepository.Create(_manCo2);
      this._manCoRepository.Create(_manCo3);
      this._manCoRepository.Create(_manCo4);

      this._manCoList.Add(_manCo1);
      this._manCoList.Add(_manCo2);


      this._applicationUserRepository.UpdateUserMancos(this._applicationUser2.Id, this._manCoList);

      this._manCoList.Add(_manCo3);
      this._manCoList.Add(_manCo4);
      this._manCoList.Remove(_manCo2);

      this._applicationUserRepository.UpdateUserMancos(this._applicationUser2.Id, this._manCoList);

      var user = this._applicationUserRepository.GetUserByName("tron2");

      user.ManCos.Count.Should().Be(3);
      user.ManCos[1].ManCo.DomicileId.Should().Be(this._domicile1.Id);
    }

    [Test]
    public void GivenAUserIdAndDomicileList_WhenITryToUpdateUserDomiciles_DomicilesForTheUserIsSavedToDatabase()
    {
      this._applicationUserRepository.Create(this._applicationUser3);

      this._domicileRepository.Create(_domicile1);
      this._domicileRepository.Create(_domicile2);
      this._domicileRepository.Create(_domicile3);
      this._domicileRepository.Create(_domicile4);

      this._domicileList.Add(_domicile1);
      this._domicileList.Add(_domicile2);
      this._domicileList.Add(_domicile3);
      this._domicileList.Add(_domicile4);

      this._applicationUserRepository.UpdateUserDomiciles(this._applicationUser3.Id, this._domicileList);

      var user = this._applicationUserRepository.GetUserByName("flynn");

      user.Domiciles.Count.Should().Be(4);

    }

    [Test]
    public void GivenAUserIdAndDomicileList_WhenITryToUpdateExistingUserDomicile_DomicilesForTheUserIsRemovedOrAddedd()
    {
      this._applicationUserRepository.Create(this._applicationUser4);

      this._domicileRepository.Create(_domicile1);
      this._domicileRepository.Create(_domicile2);
      this._domicileRepository.Create(_domicile3);
      this._domicileRepository.Create(_domicile4);

      this._domicileList.Add(_domicile1);
      this._domicileList.Add(_domicile2);

      this._applicationUserRepository.UpdateUserDomiciles(this._applicationUser4.Id, this._domicileList);

      this._domicileList.Add(_domicile3);
      this._domicileList.Add(_domicile4);
      this._domicileList.Remove(_domicile2);

      this._applicationUserRepository.UpdateUserDomiciles(this._applicationUser4.Id, this._domicileList);

      var user = this._applicationUserRepository.GetUserByName("flynn2");

      user.Domiciles.Count.Should().Be(3);
    }

    [Test]
    public void GivenAUserId_WhenITryToGetListOfUserManCos_TheManCosAreRetrieved()
    {
      this._applicationUserRepository.Create(this._applicationUser2);

      this._domicileRepository.Create(_domicile1);

      this._manCoRepository.Create(_manCo1);
      this._manCoRepository.Create(_manCo2);
      this._manCoRepository.Create(_manCo3);
      this._manCoRepository.Create(_manCo4);

      this._manCoList.Add(_manCo1);
      this._manCoList.Add(_manCo2);
      this._manCoList.Add(_manCo3);
      this._manCoList.Add(_manCo4);

      this._applicationUserRepository.UpdateUserMancos(this._applicationUser2.Id, this._manCoList);

      var manCos = this._applicationUserRepository.GetManCos(this._applicationUser2.Id);

      manCos.Count.Should().Be(4);
    }

    [Test]
    public void GivenAListOfDomicileIds_WhenITryToGetUsers_TheUsersAreRetrieved()
    {
      this._domicileRepository.Create(_domicile1);
      this._domicileRepository.Create(_domicile2);
      this._domicileRepository.Create(_domicile3);
      this._domicileRepository.Create(_domicile4);

      this._domicileList.Add(_domicile1);
      this._domicileList.Add(_domicile2);
      this._domicileList.Add(_domicile3);

      this._applicationUser.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser.Id, DomicileId = _domicile1.Id });
      this._applicationUser2.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser2.Id, DomicileId = _domicile2.Id });
      this._applicationUser3.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser3.Id, DomicileId = _domicile3.Id });
      this._applicationUser4.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser4.Id, DomicileId = _domicile4.Id });

      this._applicationUserRepository.Create(this._applicationUser);
      this._applicationUserRepository.Create(this._applicationUser2);
      this._applicationUserRepository.Create(this._applicationUser3);
      this._applicationUserRepository.Create(this._applicationUser4);

      var listDomicileIds = new List<int>();

      this._domicileList.ForEach(x => listDomicileIds.Add(x.Id));

      var users = this._applicationUserRepository.GetUsersByDomicile(listDomicileIds);

      users.Should().NotBeNull();
      users.Count.Should().Be(3);
    }

    [Test]
    public void GivenAListOfDomicileIds_WhenITryToGetUserReport_TheUserReportIsRetrieved()
    {
      this._domicileRepository.Create(_domicile1);
      this._domicileRepository.Create(_domicile2);
      this._domicileRepository.Create(_domicile3);
      this._domicileRepository.Create(_domicile4);

      this._domicileList.Add(_domicile1);
      this._domicileList.Add(_domicile2);
      this._domicileList.Add(_domicile3);

      this._applicationUser.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser.Id, DomicileId = _domicile1.Id });
      this._applicationUser2.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser2.Id, DomicileId = _domicile2.Id });
      this._applicationUser3.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser3.Id, DomicileId = _domicile3.Id });
      this._applicationUser4.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser4.Id, DomicileId = _domicile4.Id });

      this._applicationUserRepository.Create(this._applicationUser);
      this._applicationUserRepository.Create(this._applicationUser2);
      this._applicationUserRepository.Create(this._applicationUser3);
      this._applicationUserRepository.Create(this._applicationUser4);

      var users = this._applicationUserRepository.GetUserReport(_domicile1.Id, 1, 10);

      users.Should().NotBeNull();
      users.Results.Count.Should().Be(1);
      var user1 = users.Results.Where(x => x.UserName == _applicationUser.UserName).FirstOrDefault();
      user1.Roles.ToList()[0].Role.Name.ShouldBeEquivalentTo("TestAdmin");

      users.Results.Should()
           .NotContain(c => c.UserName == _applicationUser2.UserName)
           .And.NotContain(c => c.UserName == _applicationUser3.UserName)
           .And.NotContain(c => c.UserName == _applicationUser3.UserName)
           .And.NotContain(c => c.UserName == _applicationUser4.UserName)
           .And.Contain(c => c.UserName == _applicationUser.UserName);
    }

    [Test]
    public void GivenAListOfDomicileIds_WhenITryToGetPagedUsers_ThePagedUsersAreRetrieved()
    {
      var pageNumber = 1;
      var numOfItems = 2;

      this._domicileRepository.Create(_domicile1);
      this._domicileRepository.Create(_domicile2);
      this._domicileRepository.Create(_domicile3);
      this._domicileRepository.Create(_domicile4);

      this._domicileList.Add(_domicile1);
      this._domicileList.Add(_domicile2);
      this._domicileList.Add(_domicile3);

      this._applicationUser.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser.Id, DomicileId = _domicile1.Id });
      this._applicationUser2.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser2.Id, DomicileId = _domicile2.Id });
      this._applicationUser3.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser3.Id, DomicileId = _domicile3.Id });
      this._applicationUser4.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._applicationUser4.Id, DomicileId = _domicile4.Id });

      this._applicationUserRepository.Create(this._applicationUser);
      this._applicationUserRepository.Create(this._applicationUser2);
      this._applicationUserRepository.Create(this._applicationUser3);
      this._applicationUserRepository.Create(this._applicationUser4);

      var listDomicileIds = new List<int>();

      this._domicileList.ForEach(x => listDomicileIds.Add(x.Id));

      var users = this._applicationUserRepository.GetUsersByDomicile(listDomicileIds, pageNumber, numOfItems);

      users.Should().NotBeNull();
      users.TotalItems.ShouldBeEquivalentTo(3);
      users.CurrentPage.ShouldBeEquivalentTo(pageNumber);
      users.ItemsPerPage.ShouldBeEquivalentTo(numOfItems);
      users.EndRow.ShouldBeEquivalentTo(2);

    }

    [Test]
    public void GivenAUserId_WhenITryToDeactivateAUser_TheUserIsLockedOutOfTheSystem()
    {
      this._applicationUserRepository.Create(this._applicationUser);

      this._applicationUserRepository.DeactivateUser(this._applicationUser.Id);

      var user = this._applicationUserRepository.GetUserByName(this._applicationUser.UserName);

      user.Should().NotBeNull();
      user.IsLockedOut.Should().BeTrue();
    }

    [Test]
    public void GivenAUserId_WhenAUserIsDeactivated_ItReturnsTrueWhenTryingToCheckTheValueOfIsLockedOut()
    {
      this._applicationUserRepository.Create(this._applicationUser);

      this._applicationUserRepository.DeactivateUser(this._applicationUser.Id);

      var result = this._applicationUserRepository.IsLockedOut(_applicationUser.Id);

      result.Should().BeTrue();
    }

    [Test]
    public void GivenAUserId_WhenAUserIsNotDeactivated_ItReturnsFalseWhenTryingToCheckTheValueOfIsLockedOut()
    {
      this._applicationUserRepository.Create(this._applicationUser);

      var result = this._applicationUserRepository.IsLockedOut(_applicationUser.Id);

      result.Should().BeFalse();
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheLastLoginDate_TheLastLogindateIsUpdated()
    {
      ApplicationUser user;

      user = BuildMeA.ApplicationUser("usertron");
      user.IsLockedOut = true;

      this._applicationUserRepository.Create(user);
      this._applicationUserRepository.UnlockUser(user.Id);
      var result = this._applicationUserRepository.GetUserByName(user.UserName);

      result.IsLockedOut.Should().BeFalse();
    }

    [Test]
    public void GivenAUserId_WhenITryToUnlockAUser_TheUserIsUnlocked()
    {
      ApplicationUser user;

      user = BuildMeA.ApplicationUser("usertron");

      user.LastLoginDate = new DateTime(2013, 12, 10);

      this._applicationUserRepository.Create(user);

      this._applicationUserRepository.UpdateUserlastLogindate(user.Id);

      var result = this._applicationUserRepository.GetUserByName(user.UserName);

      result.Should().NotBeNull();
      result.LastLoginDate.Value.ToShortDateString().ShouldBeEquivalentTo(DateTime.Now.ToShortDateString());
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheLastLoginDate_TheFailedLogInCountIsSetToZero()
    {
      ApplicationUser user = BuildMeA.ApplicationUser("usertron");

      this._applicationUserRepository.Create(user);

      this._applicationUserRepository.UpdateUserlastLogindate(user.Id);

      var result = this._applicationUserRepository.GetUserByName(user.UserName);

      result.Should().NotBeNull();
      result.FailedLogInCount.Should().Be(0);
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheLastPasswordChangedDate_TheLastPasswordChangedDateIsUpdated()
    {
      ApplicationUser user;

      user = BuildMeA.ApplicationUser("usertron");

      user.LastPasswordChangedDate = new DateTime(2013, 12, 10);

      this._applicationUserRepository.Create(user);

      this._applicationUserRepository.UpdateUserLastPasswordChangedDate(user.Id);

      var result = this._applicationUserRepository.GetUserByName(user.UserName);

      result.Should().NotBeNull();
      result.LastPasswordChangedDate.ToShortDateString().ShouldBeEquivalentTo(DateTime.Now.ToShortDateString());
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheFailedLoginCount_TheFailedLogInCountIsIncremented()
    {
      ApplicationUser user = BuildMeA.ApplicationUser("usertron");
      _applicationUserRepository.Create(user);

      int initialCount = user.FailedLogInCount;
      _applicationUserRepository.UpdateUserFailedLogin(user.Id);

      var result = _applicationUserRepository.GetUserByName(user.UserName);
      result.FailedLogInCount.Should().Be(initialCount + 1);
    }
  }
}
