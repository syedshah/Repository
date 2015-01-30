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
  public class RejectionServiceTests
  {
    private Mock<IRejectionEngine> _rejectionEngine;
    private IRejectionService _rejectionService;

    [SetUp]
    public void SetUp()
    {
      _rejectionEngine = new Mock<IRejectionEngine>();
      _rejectionService = new RejectionService(_rejectionEngine.Object);
    }

    [Test]
    public void GivenValidData_WhenADocumentIsRejected_ThenTheDocumentIsRejected()
    {
      _rejectionService.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
      _rejectionEngine.Verify(s => s.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GivenValidData_WhenADocumentIsRejected_AndTheDocumentIsAlreadyCheckedOut_ThenADocumentCurrentlyCheckedOutExceptionIsThrown()
    {
      _rejectionService.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
      _rejectionEngine.Verify(s => s.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GivenValidData_WhenADocumentIsRejected_AndTheDocumentIsAlreadyRejected_ThenADocumentAlreadyRejectedExceptionIsThrown()
    {
      _rejectionEngine.Setup(c => c.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<DocumentAlreadyRejectedException>();

      Action act = () => _rejectionService.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentAlreadyRejectedException>();
    }

    [Test]
    public void GivenValidData_WhenADocumentIsRejected_AndTheDocumentIsAlreadyRejected_ThenADocumentAlreadyApprovedExceptionIsThrown()
    {
      _rejectionEngine.Setup(c => c.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<DocumentAlreadyApprovedException>();

      Action act = () => _rejectionService.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentAlreadyApprovedException>();
    }
  }
}
