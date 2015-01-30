namespace ServiceTests
{
  using System;
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
  public class AutoApprovalServiceTests
  {
    private Mock<IAutoApprovalRepository> _autoApprovalRepository;

    private IAutoApprovalService autoApprovalService;

    [SetUp]
    public void SetUp()
    {
      _autoApprovalRepository = new Mock<IAutoApprovalRepository>();
      this.autoApprovalService = new AutoApprovalService(_autoApprovalRepository.Object);
    }

    [Test]
    public void GivenMancoDocTypeSubDocType_WhenADocumentApprovalIsRequested_ThenAnApprovalStatusIsRetrieved()
    {
      _autoApprovalRepository.Setup(
        a => a.GetAutoApproval(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new AutoApproval());
      var result = this.autoApprovalService.GetAutoApproval(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

      _autoApprovalRepository.Verify(
        a => a.GetAutoApproval(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());

      result.Should().NotBeNull();
    }

    [Test]
    public void GivenManCoDocTypeSubDocType_WhenADocumentApprovalIsRequestedAndTheDatabaseIsUnavailable_ThenUnityExceptionIsThrown()
    {
      _autoApprovalRepository.Setup(
        a => a.GetAutoApproval(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws<UnityException>();
      Action act =
        () => this.autoApprovalService.GetAutoApproval(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void WhenITryToGetAllDocumentApprovals_AndDatabaseIsAvailable_AllDocumentApprovalsAreRetrievedFromTheDatabase()
    {

      this._autoApprovalRepository.Setup(d => d.GetAutoApprovals()).Returns(new List<AutoApproval>
                                                                                        {
                                                                                            new AutoApproval { Manco = new ManCo(), DocType = new DocType(), SubDocType = new SubDocType()},
                                                                                            new AutoApproval { Manco = new ManCo(), DocType = new DocType(), SubDocType = new SubDocType()},
                                                                                            new AutoApproval { Manco = new ManCo(), DocType = new DocType(), SubDocType = new SubDocType()}
                                                                                        });
      var result = this.autoApprovalService.GetAutoApprovals();

      this._autoApprovalRepository.Verify(d => d.GetAutoApprovals(), Times.Once());
      result.Should().NotBeNull();
      result.Count.Should().BeGreaterOrEqualTo(3);
    }

    [Test]
    public void WhenITryToGetAllDocumentApprovals_AndDatabaseIsUnAvailable_ThenUnityExceptionIsThrown()
    {
      this._autoApprovalRepository.Setup(d => d.GetAutoApprovals()).Throws<UnityException>();

      Action act = () => this.autoApprovalService.GetAutoApprovals();

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void WhenITryToGetAllDocumentApprovalsByManCoId_AndDatabaseIsAvailable_AllDocumentApprovalsByManCoIdAreRetrievedFromTheDatabase()
    {

      this._autoApprovalRepository.Setup(d => d.GetAutoApprovals(It.IsAny<int>())).Returns(new List<AutoApproval>
                                                                                        {
                                                                                            new AutoApproval { Manco = new ManCo(), DocType = new DocType(), SubDocType = new SubDocType()},
                                                                                            new AutoApproval { Manco = new ManCo(), DocType = new DocType(), SubDocType = new SubDocType()},
                                                                                            new AutoApproval { Manco = new ManCo(), DocType = new DocType(), SubDocType = new SubDocType()}
                                                                                        });
      var result = this.autoApprovalService.GetAutoApprovals(It.IsAny<int>());

        this._autoApprovalRepository.Verify(d => d.GetAutoApprovals(It.IsAny<int>()), Times.Once());
        result.Should().NotBeNull();
        result.Count.Should().BeGreaterOrEqualTo(3);
    }

    [Test]
    public void WhenITryToGetAllDocumentApprovalsByManCoId_AndDatabaseIsUnAvailable_ThenUnityExceptionIsThrown()
    {
      this._autoApprovalRepository.Setup(d => d.GetAutoApprovals(It.IsAny<int>())).Throws<UnityException>();

      Action act = () => this.autoApprovalService.GetAutoApprovals(It.IsAny<int>());

        act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenADocumentApproval_WhenITryToAddItTodatabase_ItIsSavedToDatabase()
    {
      this._autoApprovalRepository.Setup(d => d.Create(It.IsAny<AutoApproval>()));
      this._autoApprovalRepository.Setup(d => d.GetAutoApproval(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new AutoApproval());

      Action act = () => this.autoApprovalService.AddAutoApproval(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

      act.ShouldThrow<UnityAutoApprovalAlreadyExistsException>();
    }

    [Test]
    public void GivenAnAutoApproval_WhenITryToAddItTodatabase_AndTheAutoApprovalAlreadyExists_ItIsSavedToDatabase()
    {
      this._autoApprovalRepository.Setup(d => d.Create(It.IsAny<AutoApproval>()));

      this.autoApprovalService.AddAutoApproval(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

      this._autoApprovalRepository.Verify(d => d.Create(It.IsAny<AutoApproval>()), Times.Once());
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenADocumentApproval_WhenITryToAddItTodatabase_AndDatabaseIsUnavailable_TheUnityExceptionIsThrown()
    {
      this._autoApprovalRepository.Setup(d => d.Create(It.IsAny<AutoApproval>())).Throws<UnityException>();

      this.autoApprovalService.AddAutoApproval(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

      this._autoApprovalRepository.Verify(d => d.Create(It.IsAny<AutoApproval>()), Times.Once());
    }

    [Test]
    public void GivenADocumentApproval_WhenITryToUpdate_ItIsSavedToDatabase()
    {
      this._autoApprovalRepository.Setup(d => d.Update(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

      this.autoApprovalService.Update(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

      this._autoApprovalRepository.Verify(d => d.Update(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenADocumentApproval_WhenITryToUpdate_AndDatabaseIsUnavailable_TheUnityExceptionIsThrown()
    {
      this._autoApprovalRepository.Setup(d => d.Update(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws<UnityException>();

      this.autoApprovalService.Update(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

      this._autoApprovalRepository.Verify(d => d.Update(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
    }

    [Test]
    public void GivenADocumentApprovalId_WhenITryToGetTheDocumentApproval_ADocumentApprovalIsReturned()
    {
      this._autoApprovalRepository.Setup(d => d.GetAutoApproval(It.IsAny<int>())).Returns(new AutoApproval
                                                                                                      {
                                                                                                          Id = 1,
                                                                                                          Manco = new ManCo {Id = 3},
                                                                                                          DocType = new DocType { Id = 2},
                                                                                                          SubDocType = new SubDocType { Id = 1}
                                                                                                      });

      var result = this.autoApprovalService.GetAutoApproval(It.IsAny<int>());

      this._autoApprovalRepository.Verify(d => d.GetAutoApproval(It.IsAny<int>()), Times.Once());

      result.Should().NotBeNull();
      Assert.That(result.Id, Is.EqualTo(1));
      Assert.That(result.Manco.Id, Is.EqualTo(3));
      result.SubDocType.Should().NotBeNull();
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void GivenADocumentApprovalId_WhenITryToGetTheDocumentApproval_AndDatabaseIsUnavailable_TheUnityExceptionIsThrown()
    {
      this._autoApprovalRepository.Setup(d => d.GetAutoApproval(It.IsAny<int>())).Throws<UnityException>();

      this.autoApprovalService.GetAutoApproval(It.IsAny<int>());

      this._autoApprovalRepository.Verify(d => d.GetAutoApproval(It.IsAny<int>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenAnAutoApprovalIsDeleted_AndTheDatabaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      _autoApprovalRepository.Setup(p => p.Delete(It.IsAny<AutoApproval>())).Throws<Exception>();
      Action act = () => this.autoApprovalService.Delete(It.IsAny<int>());
      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenAPostIsDeleted_ThenThatPostIsReturned()
    {
      autoApprovalService.Delete(It.IsAny<int>());
      _autoApprovalRepository.Verify(p => p.Delete(It.IsAny<AutoApproval>()), Times.Once());
    }

    [Test]
    public void GivenValidData_WhenAnAutoApprovalsAreDeletedByDocTypeCode_AndTheDatabaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      this._autoApprovalRepository.Setup(d => d.GetAutoApprovals()).Throws<Exception>();
      Action act = () => this.autoApprovalService.Delete(It.IsAny<string>());
      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenValidData_WhenAnAutoApprovalsAreDeletedByDocTypeCode_ThenThatPostIsReturned()
    {
      this._autoApprovalRepository.Setup(d => d.GetAutoApprovals(It.IsAny<string>())).Returns(new List<AutoApproval>
                                                                            {
                                                                                new AutoApproval { Manco = new ManCo(), DocType = new DocType(), SubDocType = new SubDocType()},
                                                                                new AutoApproval { Manco = new ManCo(), DocType = new DocType(), SubDocType = new SubDocType()},
                                                                                new AutoApproval { Manco = new ManCo(), DocType = new DocType(), SubDocType = new SubDocType()}
                                                                            });

      autoApprovalService.Delete(It.IsAny<string>());
      _autoApprovalRepository.Verify(p => p.Delete(It.IsAny<AutoApproval>()), Times.Exactly(3));
    }
  }
}
