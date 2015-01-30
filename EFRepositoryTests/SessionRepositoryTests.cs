namespace EFRepositoryTests
{
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using System.Data.Entity.Validation;
  using System.Linq;
  using System.Transactions;
  using Builder;
  using Entities;

  using Exceptions;

  using FluentAssertions;

  using Microsoft.AspNet.Identity.EntityFramework;

  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class SessionRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _sessionRepository = new SessionRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _domicileRepository = new DomicileRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _applicationUserRepository = new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _identityRoleRepository = new IdentityRoleRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _onshoreDom = BuildMeA.Domicile("onShore", "Onshore");

      _onshoreGovUser = BuildMeA.ApplicationUser("onshoreGov");
      _onshoreReadOnly = BuildMeA.ApplicationUser("onshoreReadOnly");
      _onshoreDstAdmin = BuildMeA.ApplicationUser("onshoreDstAdmin");
      _onshoreAdmin = BuildMeA.ApplicationUser("onshore-Admin");

      _offshoreDom = BuildMeA.Domicile("offShore", "Offhore");
      _offShoreGovUser = BuildMeA.ApplicationUser("offshoreGov");;

      _session = BuildMeA.Session(Guid.NewGuid().ToString(), DateTime.Now.AddMinutes(10), DateTime.Now);

      _newSession = BuildMeA.Session("newSession", DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(-30).AddHours(1));
      _newSession1 = BuildMeA.Session("newSession1", DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(-30).AddHours(1));
      _newSession2 = BuildMeA.Session("newSession1", DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(-30).AddHours(1));
      _newSession3 = BuildMeA.Session("newSession3", DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(-30).AddHours(1));
      _oldSession = BuildMeA.Session("oldSession", DateTime.Now.Date.AddDays(-31), DateTime.Now.Date.AddDays(-31).AddHours(1));

      BuildUsers();
    }

    private void BuildUsers()
    {
      IdentityRole gov = _identityRoleRepository.GetRoles().First(r => r.Name.ToLower() == "Governor".ToLower());
      IdentityRole readOnly = _identityRoleRepository.GetRoles().First(r => r.Name.ToLower() == "Read Only".ToLower());
      IdentityRole dstadmin = _identityRoleRepository.GetRoles().First(r => r.Name.ToLower() == "dstadmin".ToLower());
      IdentityRole admin = _identityRoleRepository.GetRoles().First(r => r.Name.ToLower() == "Admin".ToLower());

      _onshoreGovUser.Roles.Add(new IdentityUserRole()
      {
        RoleId = gov.Id,
        User = _onshoreGovUser,
        UserId = _onshoreGovUser.Id
      });

      _onshoreReadOnly.Roles.Add(new IdentityUserRole()
      {
        RoleId = readOnly.Id,
        User = _onshoreReadOnly,
        UserId = _onshoreReadOnly.Id
      });

      _onshoreDstAdmin.Roles.Add(new IdentityUserRole()
      {
        RoleId = dstadmin.Id,
        User = _onshoreDstAdmin,
        UserId = _onshoreDstAdmin.Id
      });

      _onshoreAdmin.Roles.Add(new IdentityUserRole()
      {
        RoleId = admin.Id,
        User = _onshoreAdmin,
        UserId = _onshoreAdmin.Id
      });

      this._domicileRepository.Create(this._onshoreDom);
      this._domicileRepository.Create(this._offshoreDom);

      this._onshoreGovUser.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._onshoreGovUser.Id, DomicileId = this._onshoreDom.Id });
      this._onshoreDstAdmin.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._onshoreDstAdmin.Id, DomicileId = this._onshoreDom.Id });
      this._offShoreGovUser.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._offShoreGovUser.Id, DomicileId = this._offshoreDom.Id });
      this._onshoreAdmin.Domiciles.Add(new ApplicationUserDomicile() { UserId = this._onshoreAdmin.Id, DomicileId = this._onshoreDom.Id });
  
      this._applicationUserRepository.Create(this._onshoreGovUser);
      this._applicationUserRepository.Create(this._offShoreGovUser);
      this._applicationUserRepository.Create(this._onshoreDstAdmin);
      this._applicationUserRepository.Create(this._onshoreAdmin);

      this._newSession.UserId = this._onshoreGovUser.Id;
      this._oldSession.UserId = this._onshoreGovUser.Id;
      this._newSession1.UserId = this._offShoreGovUser.Id;
      this._newSession2.UserId = this._onshoreDstAdmin.Id;
      this._newSession3.UserId = this._onshoreAdmin.Id;
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private SessionRepository _sessionRepository;
    private DomicileRepository _domicileRepository;
    private ApplicationUserRepository _applicationUserRepository;
    private IdentityRoleRepository _identityRoleRepository;
    private Session _session;

    private Session _newSession;
    private Session _newSession1;
    private Session _newSession2;
    private Session _newSession3;
    private Session _oldSession;
    
    private ApplicationUser _onshoreGovUser;
    private ApplicationUser _onshoreReadOnly;
    private ApplicationUser _onshoreDstAdmin;
    private ApplicationUser _onshoreAdmin;

    private Domicile _onshoreDom;

    private ApplicationUser _offShoreGovUser;
    private Domicile _offshoreDom;

    private IdentityRole _roleReadOnly;
    private IdentityRole _roleGov;
    private IdentityRole _roleDstAdmin;

    [Test]
    public void GivenASession_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _sessionRepository.Entities.Count();
      _sessionRepository.Create(_session);

      _sessionRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void WhenIGetASpecificSession_ThenIGetTheCorrectSession()
    {
      _sessionRepository.Create(_session);

      Session newSession = _sessionRepository.GetSession(_session.Guid);

      newSession.Should().NotBeNull();
      newSession.Guid.Should().Be(newSession.Guid);
    }

    [Test]
    public void WhenIUpdateAPost_AndThePostDoesNotExist_ThenAnExceptionIsThrown()
    {
      string guid = Guid.NewGuid().ToString();

      Session session = BuildMeA.Session(guid, DateTime.Now.AddMinutes(-10), null);

      _sessionRepository.Create(session);

      Assert.Throws<UnityException>(() => _sessionRepository.Update(Guid.NewGuid().ToString(), DateTime.Now));
    }

    [Test]
    public void WhenIUpdateASession_ThenTheSessionIsUpdated()
    {
      string guid = Guid.NewGuid().ToString();
      DateTime end = DateTime.Now;

      Session session = BuildMeA.Session(guid, DateTime.Now.AddMinutes(-10), null);

      _sessionRepository.Create(session);
 
      _sessionRepository.Update(guid, end);
      session = _sessionRepository.Entities.Where(p => p.Guid == guid).FirstOrDefault();

      session.Should().NotBeNull();
      session.End.Should().Be(end);
    }

    [Test]
    public void GivenAStartDate_WhenIAskForSession_IGetTheCorrectSessionsReturned()
    {
      DateTime startDate = DateTime.Now.Date.AddDays(-30);

      _sessionRepository.Create(_newSession);
      _sessionRepository.Create(_oldSession);

      var sessions = _sessionRepository.GetSessionsByGovReadOnlyAdmin(startDate, _onshoreDom.Id);

      sessions.Should().Contain(s => s.Guid == "newSession")
        .And.NotContain(s => s.Guid == "oldSession");
    }

    [Test]
    public void GivenAStartDate_WhenIAskForSession_IOnlyGetSessionForUsersInMyDomicile()
    {
      DateTime startDate = DateTime.Now.Date.AddDays(-30);

      _sessionRepository.Create(_newSession);
      _sessionRepository.Create(_oldSession);
      _sessionRepository.Create(_newSession1);

      var sessions = _sessionRepository.GetSessionsByGovReadOnlyAdmin(startDate, _onshoreDom.Id);

      sessions.Should().Contain(s => s.Guid == "newSession")
        .And.NotContain(s => s.Guid == "oldSession")
        .And.NotContain(s => s.Guid == "newSession1");
    }

    [Test]
    public void GivenAStartDate_WhenIAskForSession_IOnlyGetSessionForReadOnlyGovenorAndAdminUsers()
    {
      DateTime startDate = DateTime.Now.Date.AddDays(-30);

      _sessionRepository.Create(_newSession);
      _sessionRepository.Create(_oldSession);
      _sessionRepository.Create(_newSession1);
      _sessionRepository.Create(_newSession2);
      _sessionRepository.Create(_newSession3);

      var sessions = _sessionRepository.GetSessionsByGovReadOnlyAdmin(startDate, _onshoreDom.Id);

      sessions.Should().Contain(s => s.Guid == "newSession")
        .And.NotContain(s => s.Guid == "oldSession")
        .And.NotContain(s => s.Guid == "newSession1")
        .And.NotContain(s => s.Guid == "newSession2")
        .And.Contain(s => s.Guid == "newSession3");
    }
  }
}
