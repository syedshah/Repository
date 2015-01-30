namespace ServiceTests
{
  using System;
  using BusinessEngineInterfaces;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;

  [TestFixture]
  public class ApprovalServiceTests
  {
    private Mock<IApprovalEngine> _approvalEngine;
    private IApprovalService _approvalService;

    [SetUp]
    public void SetUp()
    {
      _approvalEngine = new Mock<IApprovalEngine>();
      _approvalService = new ApprovalService(_approvalEngine.Object);
    }

    [Test]
    public void GivenValidData_WhenADocumentIsApproved_ThenTheDocumentIsApproved()
    {
      _approvalService.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
      _approvalEngine.Verify(s => s.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GivenValidData_WhenADocumentIsApproved_AndTheDocumentIsAlreadyCheckedOut_ThenTheDocumentIsApproved()
    {
      _approvalService.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
      _approvalEngine.Verify(s => s.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GivenValidData_WhenADocumentIsApproved_AndTheDocumentIsAlreadyApproved_ThenAdocumentAlreadyApprovedExceptionIsThrown()
    {
      _approvalEngine.Setup(c => c.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<DocumentAlreadyApprovedException>();

      Action act = () => _approvalService.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentAlreadyApprovedException>();
    }

    [Test]
    public void GivenValidData_WhenADocumentIsApproved_AndTheDocumentIsAlreadyRejected_ThenADocumentAlreadyRejectedExceptionIsThrown()
    {
      _approvalEngine.Setup(c => c.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<DocumentAlreadyRejectedException>();

      Action act = () => _approvalService.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentAlreadyRejectedException>();
    }
  }
}
