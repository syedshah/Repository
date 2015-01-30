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
  public class SecuirtyQuestionRepositoryTests
  {
    private TransactionScope _transactionScope;
    private SecurityQuestionRepository _securityQuestionRepository;
    private SecurityQuestion _securityQuestion1;
    private SecurityQuestion _securityQuestion2;
    private SecurityQuestion _securityQuestion3;
    private SecurityQuestion _securityQuestion4;
    private SecurityQuestion _securityQuestion5;
    private SecurityQuestion _securityQuestion6;

    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _securityQuestionRepository = new SecurityQuestionRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _securityQuestion1 = BuildMeA.SecurityQuestion("question 1");
      _securityQuestion2 = BuildMeA.SecurityQuestion("question 2");
      _securityQuestion3 = BuildMeA.SecurityQuestion("question 3");
      _securityQuestion4 = BuildMeA.SecurityQuestion("question 4");
      _securityQuestion5 = BuildMeA.SecurityQuestion("question 5");
      _securityQuestion6 = BuildMeA.SecurityQuestion("question 6");
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    } 

    [Test]
    public void WhenITryToRetrieveAllSecurityQuestions_AllSecurityQuestionsAreRetrieved()
    {
      this._securityQuestionRepository.Create(_securityQuestion1);
      this._securityQuestionRepository.Create(_securityQuestion2);
      this._securityQuestionRepository.Create(_securityQuestion3);
      this._securityQuestionRepository.Create(_securityQuestion4);

      var securityQuestions = this._securityQuestionRepository.GetSecurityQuestions();

      securityQuestions.Should().NotBeNull();
      securityQuestions.Should().Contain(_securityQuestion2);
      securityQuestions.Should().Contain(_securityQuestion3);
      securityQuestions.Count.Should().BeGreaterOrEqualTo(4);
    }

    [Test]
    public void WhenITryToRetrieveThreeRandomSecurityQuestions_AnyThreeRandomSecurityQuestionsAreRetrieved()
    {
        this._securityQuestionRepository.Create(_securityQuestion1);
        this._securityQuestionRepository.Create(_securityQuestion2);
        this._securityQuestionRepository.Create(_securityQuestion3);
        this._securityQuestionRepository.Create(_securityQuestion4);
        this._securityQuestionRepository.Create(_securityQuestion5);
        this._securityQuestionRepository.Create(_securityQuestion6);

        var securityQuestions = this._securityQuestionRepository.GetThreeRandomSecurityQuestions();

        securityQuestions.Should().NotBeNull();
        securityQuestions.Count.Should().BeGreaterOrEqualTo(3);
    }
  }
}
