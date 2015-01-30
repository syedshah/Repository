namespace ServiceTests
{
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class SecurityQuestionServiceTests
  {
    private Mock<ISecurityQuestionRepository> _securityQuestionRepository;
    private ISecurityQuestionService _securityQuestionService;

    [SetUp]
    public void SetUp()
    {
      _securityQuestionRepository = new Mock<ISecurityQuestionRepository>();
      _securityQuestionService = new SecurityQuestionService(_securityQuestionRepository.Object);
    }

    [Test]
    public void WhenIWantToRetrieveAllSecurityQuestions_AndDatabaseIsAvailable_AllTheSecurityQuestionsAreRetrieved()
    {
      this._securityQuestionRepository.Setup(x => x.GetSecurityQuestions()).Returns(new List<SecurityQuestion>());

      var result = this._securityQuestionService.GetSecurityQuestions();

      this._securityQuestionRepository.Verify(x => x.GetSecurityQuestions(), Times.AtLeastOnce);
      result.Should().NotBeNull();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToRetrieveAllSecurityQuestions_AndDatabaseIsNotAvailable_AUnityExceptionIsThrown()
    {
      this._securityQuestionRepository.Setup(x => x.GetSecurityQuestions()).Throws<UnityException>();

      this._securityQuestionService.GetSecurityQuestions();

      this._securityQuestionRepository.Verify(x => x.GetSecurityQuestions(), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToRetrieveThreeRandomSecurityQuestions_AndDatabaseIsAvailable_ThreeRandomSecurityQuestionsAreRetrieved()
    {
        this._securityQuestionRepository.Setup(x => x.GetThreeRandomSecurityQuestions()).Returns(new List<SecurityQuestion>());

        var result = this._securityQuestionService.GetThreeRandomSecurityQuestions();

        this._securityQuestionRepository.Verify(x => x.GetThreeRandomSecurityQuestions(), Times.AtLeastOnce);
        result.Should().NotBeNull();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToRetrieveThreeRandomSecurityQuestions_AndDatabaseIsNotAvailable_AUnityExceptionIsThrown()
    {
        this._securityQuestionRepository.Setup(x => x.GetThreeRandomSecurityQuestions()).Throws<UnityException>();

        this._securityQuestionService.GetThreeRandomSecurityQuestions();

        this._securityQuestionRepository.Verify(x => x.GetThreeRandomSecurityQuestions(), Times.AtLeastOnce);
    }
  }
}
