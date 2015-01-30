namespace EFRepositoryTests
{
  using System.Configuration;
  using System.Linq;
  using System.Transactions;
  using Builder;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class AppManCoEmailRepositoryTests
  {
      [SetUp]
      public void Setup()
      {
        _transactionScope = new TransactionScope();
        _appManCoEmailRepository = new AppManCoEmailRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
        _manCoRepository = new ManCoRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
        _applicationRepository = new ApplicationRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
        _docTypeRepository = new DocTypeRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
        _manCo1 = BuildMeA.ManCo("description", "code");
        _manCo2 = BuildMeA.ManCo("description2", "code2");
        _application1 = BuildMeA.Application("code", "description");
        _docType1 = BuildMeA.DocType("code", "description");

        _manCoRepository.Create(_manCo1);
        _manCoRepository.Create(_manCo2);
        _applicationRepository.Create(_application1);
        _docTypeRepository.Create(_docType1);

        _appManCoEmail = BuildMeA.AppManCoEmail(_application1.Id, _manCo1.Id, _docType1.Id, "accountNumber", "email");
        _appManCoEmail2 = BuildMeA.AppManCoEmail(_application1.Id, _manCo2.Id, _docType1.Id, "accountNumber2", "email2");
        _appManCoEmail3 = BuildMeA.AppManCoEmail(_application1.Id, _manCo1.Id, _docType1.Id, "accountNumber3", "email3");
        _appManCoEmail4 = BuildMeA.AppManCoEmail(_application1.Id, _manCo2.Id, _docType1.Id, "accountNumber4", "email4");  
      }

      [TearDown]
      public void TearDown()
      {
          _transactionScope.Dispose();
      }

      private TransactionScope _transactionScope;
      private AppManCoEmailRepository _appManCoEmailRepository;
      private ManCoRepository _manCoRepository;
      private ApplicationRepository _applicationRepository;
      private DocTypeRepository _docTypeRepository;
      private AppManCoEmail _appManCoEmail;
      private AppManCoEmail _appManCoEmail2;
      private AppManCoEmail _appManCoEmail3;
      private AppManCoEmail _appManCoEmail4;
      private ManCo _manCo1;
      private ManCo _manCo2;
      private Application _application1;
      private DocType _docType1;

      [Test]
      public void GivenAnApplicationManCoEmail_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
      {
        int initialCount = _appManCoEmailRepository.Entities.Count();
        _appManCoEmailRepository.Create(_appManCoEmail);

        _appManCoEmailRepository.Entities.Count().Should().Be(initialCount + 1);
      }

      [Test]
      public void GivenAnAppManCoEmail_WhenITryToSearchForItById_ItIsRetrievedFromTheDatabase()
      {
        _appManCoEmailRepository.Create(_appManCoEmail);

        var result = _appManCoEmailRepository.GetAppManCoEmail(_appManCoEmail.Id);

        result.Should().NotBeNull();
        result.Id.ShouldBeEquivalentTo(_appManCoEmail.Id);
      }

      [Test]
      public void GivenAnAppIdAndManCoId_WhenITryToSearchByAppIdAndManCoId_AppManCoEmailsAreRetrievedFromTheDB()
      {
          _appManCoEmailRepository.Create(_appManCoEmail);
          _appManCoEmailRepository.Create(_appManCoEmail2);
          _appManCoEmailRepository.Create(_appManCoEmail3);
          _appManCoEmailRepository.Create(_appManCoEmail4);

          var result = _appManCoEmailRepository.GetAppManCoEmails(_appManCoEmail2.ApplicationId,_appManCoEmail2.ManCoId);

          result.Should().NotBeNull();
          result.Count.ShouldBeEquivalentTo(2);
      }

      [Test]
      public void GivenANullAppIdAndManCoId_WhenITryToSearchByNullAppIdAndManCoId_AppManCoEmailsAreRetrievedFromTheDB()
      {
          _appManCoEmailRepository.Create(_appManCoEmail);
          _appManCoEmailRepository.Create(_appManCoEmail2);
          _appManCoEmailRepository.Create(_appManCoEmail3);
          _appManCoEmailRepository.Create(_appManCoEmail4);

          var result = _appManCoEmailRepository.GetAppManCoEmails(null, _appManCoEmail2.ManCoId);

          result.Should().NotBeNull();
          result.Count.ShouldBeEquivalentTo(2);
      }

      [Test]
      public void GivenAnAppIdWithZerovalueAndManCoId_WhenITryToSearchByNullAppIdAndManCoId_AppManCoEmailsAreRetrievedFromTheDB()
      {
          _appManCoEmailRepository.Create(_appManCoEmail);
          _appManCoEmailRepository.Create(_appManCoEmail2);
          _appManCoEmailRepository.Create(_appManCoEmail3);
          _appManCoEmailRepository.Create(_appManCoEmail4);

          var result = _appManCoEmailRepository.GetAppManCoEmails(0, _appManCoEmail2.ManCoId);

          result.Should().NotBeNull();
          result.Count.ShouldBeEquivalentTo(2);
      }


      [Test]
      public void GivenAnAppManCoEmail_WhenITryToUpdateIt_TheAppManCoEmailIsUpdated()
      {
        _appManCoEmailRepository.Create(_appManCoEmail);

        var result = _appManCoEmailRepository.GetAppManCoEmail(_appManCoEmail.Id);

        var retrievedAccountNumber = result.AccountNumber;
        var retrievedEmail = result.Email;
 
        this._appManCoEmailRepository.UpdateAppManCoEmail(_appManCoEmail.Id, _appManCoEmail.ApplicationId, _appManCoEmail.ManCoId, _appManCoEmail.DocTypeId, "account number updated","jean@northtrust.com", "userName");

        var result2 = _appManCoEmailRepository.GetAppManCoEmail(_appManCoEmail.Id);

        result.Should().NotBeNull();
        result2.Should().NotBeNull();
        result.Id.ShouldBeEquivalentTo(result2.Id);
        result.ManCoId.ShouldBeEquivalentTo(result2.ManCoId);
        result2.AccountNumber.Should().NotBe(retrievedAccountNumber);
        result2.Email.Should().NotBe(retrievedEmail);
      }

      [Test]
      public void WhenIDeleteAnAppManCoEmail_ThenItIsDeleted()
      {
        _appManCoEmailRepository.Create(_appManCoEmail);

        AppManCoEmail retrievedAppManCoEmail = _appManCoEmailRepository.GetAppManCoEmail(_appManCoEmail.Id);
        _appManCoEmailRepository.Delete(retrievedAppManCoEmail);
        AppManCoEmail deletedAppManCoEmail = _appManCoEmailRepository.GetAppManCoEmail(retrievedAppManCoEmail.Id);
        deletedAppManCoEmail.Should().BeNull();
      }
  }
}
