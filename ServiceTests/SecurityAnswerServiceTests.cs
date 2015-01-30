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
  public class SecurityAnswerServiceTests
  {
    private Mock<ISecurityAnswerRepository> _securityAnswerRepository;

    private ISecurityAnswerService _securityAnswerService;

    [SetUp]
    public void SetUp()
    {
      _securityAnswerRepository = new Mock<ISecurityAnswerRepository>();
      _securityAnswerService = new SecurityAnswerService(_securityAnswerRepository.Object);
    }

    [Test]
    public void WhenIWantToSaveSecurityAnswer_AndDatabaseIsAvailable_TheSecurityAnswerIsSaved()
    {
      this._securityAnswerRepository.Setup(x => x.Create(It.IsAny<SecurityAnswer>()));

      this._securityAnswerService.SaveSecurityAnswer(It.IsAny<SecurityAnswer>());

      this._securityAnswerRepository.Verify(x => x.Create(It.IsAny<SecurityAnswer>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToSaveSecurityAnswer_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._securityAnswerRepository.Setup(x => x.Create(It.IsAny<SecurityAnswer>())).Throws<UnityException>();

      this._securityAnswerService.SaveSecurityAnswer(It.IsAny<SecurityAnswer>());

      this._securityAnswerRepository.Verify(x => x.Create(It.IsAny<SecurityAnswer>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToUpdateSecurityAnswer_AndDatabaseIsAvailable_TheSecurityAnswerIsUpdated()
    {
      this._securityAnswerRepository.Setup(x => x.Update(It.IsAny<SecurityAnswer>()));

      this._securityAnswerService.UpdateSecurityAnswer(It.IsAny<SecurityAnswer>());

      this._securityAnswerRepository.Verify(x => x.Update(It.IsAny<SecurityAnswer>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToUpdateSecurityAnswer_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._securityAnswerRepository.Setup(x => x.Update(It.IsAny<SecurityAnswer>())).Throws<UnityException>();

      this._securityAnswerService.UpdateSecurityAnswer(It.IsAny<SecurityAnswer>());

      this._securityAnswerRepository.Verify(x => x.Update(It.IsAny<SecurityAnswer>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToGetSecurityAnswerByUserIdAndQuestionId_AndDatabaseIsAvailable_TheSecurityAnswerIsRetrieved()
    {
      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()))
          .Returns(new SecurityAnswer());

      var result = this._securityAnswerService.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>());

      this._securityAnswerRepository.Verify(
        x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);

      result.Should().NotBeNull();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToGetSecurityAnswerByUserIdAndQuestionId_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()))
          .Throws<UnityException>();

      this._securityAnswerService.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>());

      this._securityAnswerRepository.Verify(
        x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void
      WhenIWantToValidateASecurityAnswerByUserIdAndQuestionId_AndDatabaseIsAvailable_AndTheSecurityAnswerIsRight_TheSecurityAnswerShouldBeValidatedAndReturnTrue
      ()
    {
      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()))
          .Returns(new SecurityAnswer("Answer right", "22"));

      var result = this._securityAnswerService.SecurityAnswerIsValid(It.IsAny<string>(), "22", "Answer right");

      this._securityAnswerRepository.Verify(
        x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);

      result.ShouldBeEquivalentTo(true);
    }

    [Test]
    public void
      WhenIWantToValidateASecurityAnswerByUserIdAndQuestionId_AndDatabaseIsAvailable_AndTheSecurityAnswerIsWrong_TheSecurityAnswerShouldBeValidatedAndReturnFalse
      ()
    {
      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()))
          .Returns(new SecurityAnswer("answer wrong", "22"));

      var result = this._securityAnswerService.SecurityAnswerIsValid(It.IsAny<string>(), "22", "Answer right");

      this._securityAnswerRepository.Verify(
        x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);

      result.ShouldBeEquivalentTo(false);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToValidateASecurityAnswerByUserIdAndQuestionId_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown
      ()
    {
      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()))
          .Throws<UnityException>();

      this._securityAnswerService.SecurityAnswerIsValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      this._securityAnswerRepository.Verify(
        x => x.GetSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }


    [Test]
    public void
      WhenIWantToCheckIfAUserHasSecurityAnswers_AndDatabaseIsAvailable_AndHasTheRightNumberOfSecurityAnswers_TheResultShouldBeTrue
      ()
    {
      var securityAnswers = new List<SecurityAnswer>();
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());

      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswers(It.IsAny<string>())).Returns(securityAnswers);

      var result = this._securityAnswerService.HasSecurityAnswers(It.IsAny<string>());

      this._securityAnswerRepository.Verify(x => x.GetSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);

      result.ShouldBeEquivalentTo(true);
    }

    [Test]
    public void
      WhenIWantToCheckIfAUserHasSecurityAnswers_AndDatabaseIsAvailable_AndDoesNotHaveTheRightNumberOfSecurityAnswers_TheResultShouldBeFalse
      ()
    {
      var securityAnswers = new List<SecurityAnswer>();
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());
      securityAnswers.Add(new SecurityAnswer());

      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswers(It.IsAny<string>())).Returns(securityAnswers);

      var result = this._securityAnswerService.HasSecurityAnswers(It.IsAny<string>());

      this._securityAnswerRepository.Verify(x => x.GetSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);

      result.ShouldBeEquivalentTo(false);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToCheckIfAUserHasSecurityAnswers_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswers(It.IsAny<string>())).Throws<UnityException>();

      this._securityAnswerService.HasSecurityAnswers(It.IsAny<string>());

      this._securityAnswerRepository.Verify(x => x.GetSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToSaveSecurityAnswersByUserId_AndDatabaseIsAvailable_TheSecurityAnswerAreSaved()
    {

      var listSecurityAnswers = new List<SecurityAnswer>();

      listSecurityAnswers.Add(new SecurityAnswer());
      listSecurityAnswers.Add(new SecurityAnswer());
      listSecurityAnswers.Add(new SecurityAnswer());
      listSecurityAnswers.Add(new SecurityAnswer());

      this._securityAnswerRepository.Setup(x => x.Create(It.IsAny<SecurityAnswer>()));

      this._securityAnswerService.SaveSecurityAnswers(listSecurityAnswers, It.IsAny<string>());

      this._securityAnswerRepository.Verify(x => x.Create(It.IsAny<SecurityAnswer>()), Times.AtLeastOnce);

    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToSaveSecurityAnswersByUserId_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._securityAnswerRepository.Setup(x => x.Create(It.IsAny<SecurityAnswer>())).Throws<UnityException>();

      this._securityAnswerService.SaveSecurityAnswers(It.IsAny<List<SecurityAnswer>>(), It.IsAny<string>());

      this._securityAnswerRepository.Verify(x => x.Create(It.IsAny<SecurityAnswer>()), Times.AtLeastOnce);

    }


    [Test]
    public void WhenIWantToGetSecurityAnswersByUserId_AndDatabaseIsAvailable_TheSecurityAnswerAreRetrieved()
    {
      var listSecurityAnswers = new List<SecurityAnswer>();

      listSecurityAnswers.Add(new SecurityAnswer());
      listSecurityAnswers.Add(new SecurityAnswer());
      listSecurityAnswers.Add(new SecurityAnswer());
      listSecurityAnswers.Add(new SecurityAnswer());

      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswers(It.IsAny<string>())).Returns(listSecurityAnswers);

      var result = this._securityAnswerService.GetSecurityAnswers(It.IsAny<string>());

      this._securityAnswerRepository.Verify(x => x.GetSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);

      result.Should().NotBeNull();
      result.Count.ShouldBeEquivalentTo(4);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToGetSecurityAnswersByUserId_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._securityAnswerRepository.Setup(x => x.GetSecurityAnswers(It.IsAny<string>())).Throws<UnityException>();

      this._securityAnswerService.GetSecurityAnswers(It.IsAny<string>());

      this._securityAnswerRepository.Verify(x => x.GetSecurityAnswers(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToUpdateSecurityAnswersByList_AndDatabaseIsAvailable_TheSecurityAnswerAreUpdated()
    {
      var listSecurityAnswers = new List<SecurityAnswer>();

      listSecurityAnswers.Add(new SecurityAnswer());
      listSecurityAnswers.Add(new SecurityAnswer());
      listSecurityAnswers.Add(new SecurityAnswer());
      listSecurityAnswers.Add(new SecurityAnswer());

      this._securityAnswerRepository.Setup(x => x.UpdateSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()));

      this._securityAnswerService.UpdateSecurityAnswers(listSecurityAnswers);

      this._securityAnswerRepository.Verify(
        x => x.UpdateSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToUpdateSecurityAnswersByList_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._securityAnswerRepository.Setup(x => x.UpdateSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()))
          .Throws<UnityException>();

      this._securityAnswerService.UpdateSecurityAnswers(It.IsAny<List<SecurityAnswer>>());

      this._securityAnswerRepository.Verify(
        x => x.UpdateSecurityAnswer(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }
  }
}
