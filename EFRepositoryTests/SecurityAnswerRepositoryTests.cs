namespace EFRepositoryTests
{
  using System.Configuration;
  using System.Transactions;
  using Builder;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class SecurityAnswerRepositoryTests
  {
    private TransactionScope _transactionScope;
    private ApplicationUserRepository _applicationUserRepository;
    private SecurityAnswerRepository _securityAnswerRepository;
    private SecurityQuestionRepository _securityQuestionRepository;
    private ApplicationUser _user;
    private SecurityAnswer _securityAnswer1;
    private SecurityAnswer _securityAnswer2;
    private SecurityAnswer _securityAnswer3;
    private SecurityAnswer _securityAnswer4;
    private SecurityQuestion _securityQuestion1;
    private SecurityQuestion _securityQuestion2;
    private SecurityQuestion _securityQuestion3;
    private SecurityQuestion _securityQuestion4;

    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _applicationUserRepository = new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _securityAnswerRepository = new SecurityAnswerRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _securityQuestionRepository = new SecurityQuestionRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _user = BuildMeA.ApplicationUser("user1");
      _securityQuestion1 = BuildMeA.SecurityQuestion("question 1");
      _securityQuestion2 = BuildMeA.SecurityQuestion("question 2");
      _securityQuestion3 = BuildMeA.SecurityQuestion("question 3");
      _securityQuestion4 = BuildMeA.SecurityQuestion("question 4");
      _securityAnswer1 = BuildMeA.SecurityAnswer("answer 1").WithSecurityQuestionId(_securityQuestion1.Id).WithUserId(_user.Id);
      _securityAnswer2 = BuildMeA.SecurityAnswer("answer 2").WithSecurityQuestionId(_securityQuestion2.Id).WithUserId(_user.Id);
      _securityAnswer3 = BuildMeA.SecurityAnswer("answer 3").WithSecurityQuestionId(_securityQuestion3.Id).WithUserId(_user.Id);
      _securityAnswer4 = BuildMeA.SecurityAnswer("answer 4").WithSecurityQuestionId(_securityQuestion4.Id).WithUserId(_user.Id);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    } 

    [Test]
    public void WhenITryToRetrieveASecurityAnswerForAParticularUserAndQuestion_TheUsersSecurityAnswerIsRetrieved()
    {
      this._applicationUserRepository.Create(_user);

      this._securityQuestionRepository.Create(_securityQuestion1);
      this._securityQuestionRepository.Create(_securityQuestion2);
      this._securityQuestionRepository.Create(_securityQuestion3);
      this._securityQuestionRepository.Create(_securityQuestion4);

      this._securityAnswerRepository.Create(_securityAnswer1);
      this._securityAnswerRepository.Create(_securityAnswer2);
      this._securityAnswerRepository.Create(_securityAnswer3);
      this._securityAnswerRepository.Create(_securityAnswer4);

      var result = this._securityAnswerRepository.GetSecurityAnswer(_user.Id, _securityQuestion2.Id);

      result.Should().NotBeNull();
      result.SecurityQuestionId.ShouldBeEquivalentTo(_securityQuestion2.Id);
      result.SecurityQuestion.Question.ShouldBeEquivalentTo(_securityQuestion2.Question);
    }

      [Test]
      public void WhenITryToRetrieveSecurityAnswersForAParticularUser_TheUsersSecurityAnswersIsRetrieved()
      {
        this._applicationUserRepository.Create(_user);

        this._securityQuestionRepository.Create(_securityQuestion1);
        this._securityQuestionRepository.Create(_securityQuestion2);
        this._securityQuestionRepository.Create(_securityQuestion3);
        this._securityQuestionRepository.Create(_securityQuestion4);

        this._securityAnswerRepository.Create(_securityAnswer1);
        this._securityAnswerRepository.Create(_securityAnswer2);
        this._securityAnswerRepository.Create(_securityAnswer3);
        this._securityAnswerRepository.Create(_securityAnswer4);

        var result = this._securityAnswerRepository.GetSecurityAnswers(_user.Id);

        result.Should().NotBeNull();
        result.Count.ShouldBeEquivalentTo(4);
      }

      [Test]
      public void WhenITryToDeleteSecurityAnswersForAParticularUser_AllTheSecurityAnswersAreDeleted()
      {
        this._applicationUserRepository.Create(_user);

        this._securityQuestionRepository.Create(_securityQuestion1);
        this._securityQuestionRepository.Create(_securityQuestion2);
        this._securityQuestionRepository.Create(_securityQuestion3);
        this._securityQuestionRepository.Create(_securityQuestion4);

        this._securityAnswerRepository.Create(_securityAnswer1);
        this._securityAnswerRepository.Create(_securityAnswer2);
        this._securityAnswerRepository.Create(_securityAnswer3);
        this._securityAnswerRepository.Create(_securityAnswer4);

        this._securityAnswerRepository.DeleteByUserId(_user.Id);

        var result = this._securityAnswerRepository.GetSecurityAnswers(_user.Id);

        result.Should().NotBeNull();
        result.Count.ShouldBeEquivalentTo(0);
      }

      [Test]
      public void GivenIdAndAnswer_WhenIWantToUpdateASecurityAnswer_TheSecurityAnswerIsUpdated()
      {
          this._applicationUserRepository.Create(_user);

          this._securityQuestionRepository.Create(_securityQuestion1);

          this._securityAnswerRepository.Create(_securityAnswer1);

          this._securityAnswerRepository.UpdateSecurityAnswer(_securityAnswer1.Id, "this a new answer");

          var result = this._securityAnswerRepository.GetSecurityAnswer(_user.Id, _securityQuestion1.Id);

          result.Should().NotBeNull();

          result.Answer.ShouldBeEquivalentTo("this a new answer");
      }
  }
}
