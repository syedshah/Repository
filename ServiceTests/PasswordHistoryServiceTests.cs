namespace ServiceTests
{
  using System.Collections.Generic;

  using Encryptions;

  using Entities;
  using Exceptions;
  using FluentAssertions;

  using IdentityWrapper.Interfaces;

  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class PasswordHistoryServiceTests
  {
    private Mock<IPasswordHistoryRepository> _passwordHistoryRepository;
    private Mock<IUserManagerProvider> _userManagerProvider;
    private IPasswordHistoryService _passwordHistoryService;

    [SetUp]
    public void SetUp()
    {
      _passwordHistoryRepository = new Mock<IPasswordHistoryRepository>();
      this._userManagerProvider = new Mock<IUserManagerProvider>();
      _passwordHistoryService = new PasswordHistoryService(_passwordHistoryRepository.Object, _userManagerProvider.Object);
    }

    [Test]
    public void WhenIWantToGetPasswordHistoryByLastNumberofRecords_AndDatabaseIsAvailable_ThePasswordHistoryIsRetrieved()
    {
      this._passwordHistoryRepository.Setup(x => x.GetPasswordHistory(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(new List<PasswordHistory>());

      var result = this._passwordHistoryService.GetPasswordHistory(It.IsAny<string>(), It.IsAny<int>());

      this._passwordHistoryRepository.Verify(x => x.GetPasswordHistory(It.IsAny<string>(), It.IsAny<int>()),Times.AtLeastOnce);
      result.Should().NotBeNull();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToGetPasswordHistoryByLastNumberofRecords_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._passwordHistoryRepository.Setup(x => x.GetPasswordHistory(It.IsAny<string>(), It.IsAny<int>()))
            .Throws<UnityException>();

      this._passwordHistoryService.GetPasswordHistory(It.IsAny<string>(), It.IsAny<int>());

      this._passwordHistoryRepository.Verify(x => x.GetPasswordHistory(It.IsAny<string>(), It.IsAny<int>()), Times.AtLeastOnce);   
    }


    [Test]
    public void WhenIWantToGetPasswordHistorByLastNumberOfMonths_AndDatabaseIsAvailable_ThePasswordHistoryIsRetrieved()
    {
        this._passwordHistoryRepository.Setup(x => x.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>()))
              .Returns(new List<PasswordHistory>());

        var result = this._passwordHistoryService.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>());

        this._passwordHistoryRepository.Verify(x => x.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>()), Times.AtLeastOnce);
        result.Should().NotBeNull();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToGetPasswordHistoryByLastNumberOfMonths_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
        this._passwordHistoryRepository.Setup(x => x.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>()))
              .Throws<UnityException>();

        this._passwordHistoryService.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>());

        this._passwordHistoryRepository.Verify(x => x.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToSavePasswordHistory_AndDatabaseIsAvailable_ThePasswordHistoryIsSaved()
    {
      this._passwordHistoryRepository.Setup(x => x.Create(It.IsAny<PasswordHistory>()));

      this._passwordHistoryService.SavePasswordHistory(It.IsAny<string>(), It.IsAny<string>());

      this._passwordHistoryRepository.Verify(x => x.Create(It.IsAny<PasswordHistory>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToSavePasswordHistory_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._passwordHistoryRepository.Setup(x => x.Create(It.IsAny<PasswordHistory>())).Throws<UnityException>();

      this._passwordHistoryService.SavePasswordHistory(It.IsAny<string>(), It.IsAny<string>());

      this._passwordHistoryRepository.Verify(x => x.Create(It.IsAny<PasswordHistory>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToCheckIfPasswordIsInHistory_AndDatabaseIsAvailable_IfPasswordIsInHistory_ItShouldReturnTrue()
    {
      var passwordHistoryList = new List<PasswordHistory>();

      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = UnityEncryption.Encrypt("gogo") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = UnityEncryption.Encrypt("gogi") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = UnityEncryption.Encrypt("gaga") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = UnityEncryption.Encrypt("gigi") });

      this._passwordHistoryRepository.Setup(x => x.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>()))
              .Returns(passwordHistoryList);

      var result = this._passwordHistoryService.IsPasswordInHistory("2w3", "gigi");

      result.Should().BeTrue();
      this._passwordHistoryRepository.Verify(x => x.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToCheckIfPasswordIsInHistory_AndDatabaseIsAvailable_IfPasswordIsNotInHistory_ItShouldReturnFalse()
    {
      var passwordHistoryList = new List<PasswordHistory>();

      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = UnityEncryption.Encrypt("gogo") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = UnityEncryption.Encrypt("gogi") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = UnityEncryption.Encrypt("gaga") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = UnityEncryption.Encrypt("gigi") });

      this._passwordHistoryRepository.Setup(x => x.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(passwordHistoryList);

      var result = this._passwordHistoryService.IsPasswordInHistory("2w3", "gugi");

      result.Should().BeFalse();
      this._passwordHistoryRepository.Verify(x => x.GetPasswordHistoryByMonths(It.IsAny<string>(), It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void WhenIWantToCheckIfPasswordIsInHistory_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this._passwordHistoryRepository.Setup(x => x.GetPasswordHistory(It.IsAny<string>(), It.IsAny<int>()))
                .Throws<UnityException>();

      this._passwordHistoryService.IsPasswordInHistory("2w3", "gugi");

      this._passwordHistoryRepository.Verify(x => x.GetPasswordHistory(It.IsAny<string>(), It.IsAny<int>()), Times.AtLeastOnce);
    }
  }
}
