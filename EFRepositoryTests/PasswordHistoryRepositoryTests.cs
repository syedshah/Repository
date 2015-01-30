namespace EFRepositoryTests
{
  using System;
  using System.Configuration;
  using System.Transactions;
  using Builder;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class PasswordHistoryRepositoryTests
  {
    private TransactionScope _transactionScope;
    private PasswordHistoryRepository _passwordHistoryRepository;
    private ApplicationUserRepository _applicationUserRepository;
    private ApplicationUser _user;
    private PasswordHistory _passwordHistory1;
    private PasswordHistory _passwordHistory2;
    private PasswordHistory _passwordHistory3;
    private PasswordHistory _passwordHistory4;
    private PasswordHistory _passwordHistory5;
    private PasswordHistory _passwordHistory6;
    private PasswordHistory _passwordHistory7;
    private PasswordHistory _passwordHistory8;

    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _passwordHistoryRepository = new PasswordHistoryRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _applicationUserRepository = new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _user = BuildMeA.ApplicationUser("user1");
      _passwordHistory1 = BuildMeA.PasswordHistory(_user.Id, "password1");
      _passwordHistory2 = BuildMeA.PasswordHistory(_user.Id, "password2");
      _passwordHistory3 = BuildMeA.PasswordHistory(_user.Id, "password3");
      _passwordHistory4 = BuildMeA.PasswordHistory(_user.Id, "password4");
      _passwordHistory5 = BuildMeA.PasswordHistory(_user.Id, "password5");
      _passwordHistory6 = BuildMeA.PasswordHistory(_user.Id, "password6");
      _passwordHistory7 = BuildMeA.PasswordHistory(_user.Id, "password7");
      _passwordHistory8 = BuildMeA.PasswordHistory(_user.Id, "password8");
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    } 

    [Test]
    public void WhenIWantToSavePasswordHistory_ThePasswordHistoryIsSaved()
    {
        this._applicationUserRepository.Create(_user);

        this._passwordHistoryRepository.Create(_passwordHistory1);
        this._passwordHistoryRepository.Create(_passwordHistory2);
        this._passwordHistoryRepository.Create(_passwordHistory3);

        var passwordHistory = this._passwordHistoryRepository.GetPasswordHistory(_user.Id, 3);

        passwordHistory.Should().NotBeNull();
        passwordHistory[1].PasswordHash.ShouldAllBeEquivalentTo(_passwordHistory2.PasswordHash);
        passwordHistory[0].PasswordHash.ShouldAllBeEquivalentTo(_passwordHistory3.PasswordHash);
    }

    [Test]
    public void WhenIWantToRetrievedPasswordHistoryForACertainNumberOfRecords_ThePasswordHistoryIsReceived()
    {
      this._applicationUserRepository.Create(_user);

      this._passwordHistoryRepository.Create(_passwordHistory1);
      this._passwordHistoryRepository.Create(_passwordHistory2);
      this._passwordHistoryRepository.Create(_passwordHistory3);
      this._passwordHistoryRepository.Create(_passwordHistory4);
      this._passwordHistoryRepository.Create(_passwordHistory5);
      this._passwordHistoryRepository.Create(_passwordHistory6);
      this._passwordHistoryRepository.Create(_passwordHistory7);
      this._passwordHistoryRepository.Create(_passwordHistory8);

      var passwordHistory = this._passwordHistoryRepository.GetPasswordHistory(_user.Id, 6);

      passwordHistory.Should().NotBeNull();
      passwordHistory[1].PasswordHash.ShouldAllBeEquivalentTo(_passwordHistory7.PasswordHash);
      passwordHistory[0].PasswordHash.ShouldAllBeEquivalentTo(_passwordHistory8.PasswordHash);
      passwordHistory[3].Id.ShouldBeEquivalentTo(_passwordHistory5.Id);
      passwordHistory[5].Id.ShouldBeEquivalentTo(_passwordHistory3.Id);
    }

    [Test]
    public void WhenIWantToRetrievedPasswordHistoryForACertainNumberOfMonths_ThePasswordHistoryIsReceived()
    {
        this._applicationUserRepository.Create(_user);

        this._passwordHistoryRepository.Create(_passwordHistory1);
        this._passwordHistoryRepository.Create(_passwordHistory2);
        this._passwordHistoryRepository.Create(_passwordHistory3);
        this._passwordHistoryRepository.Create(_passwordHistory4);
        this._passwordHistoryRepository.Create(_passwordHistory5);
        this._passwordHistoryRepository.Create(_passwordHistory6);
        this._passwordHistoryRepository.Create(_passwordHistory7);
        this._passwordHistoryRepository.Create(_passwordHistory8);

        this._passwordHistory1.LogDate = new DateTime(2012,10,15);
        this._passwordHistory2.LogDate = new DateTime(2012, 11, 15);
        this._passwordHistory3.LogDate = new DateTime(2012, 12, 15);

        this._passwordHistoryRepository.Update(_passwordHistory1);
        this._passwordHistoryRepository.Update(_passwordHistory2);
        this._passwordHistoryRepository.Update(_passwordHistory3);

        var passwordHistory = this._passwordHistoryRepository.GetPasswordHistoryByMonths(_user.Id, 12);

        passwordHistory.Should().NotBeNull();
        passwordHistory[1].PasswordHash.ShouldAllBeEquivalentTo(_passwordHistory7.PasswordHash);
        passwordHistory[0].PasswordHash.ShouldAllBeEquivalentTo(_passwordHistory8.PasswordHash);
        passwordHistory[3].Id.ShouldBeEquivalentTo(_passwordHistory5.Id);
        passwordHistory[4].Id.ShouldBeEquivalentTo(_passwordHistory4.Id);
        passwordHistory.Should().NotContain(_passwordHistory1);
        passwordHistory.Should().NotContain(_passwordHistory2);
        passwordHistory.Should().NotContain(_passwordHistory3);
    }
  }
}
