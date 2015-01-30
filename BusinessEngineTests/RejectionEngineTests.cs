namespace BusinessEngineTests
{
  using System;
  using BusinessEngineInterfaces;
  using BusinessEngines;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class RejectionEngineTests
  {
    private Mock<IDocumentService> _documentService;
    private Mock<IRejectionRepository> _rejectionRepository;
    private IRejectionEngine _rejectionEngine;

    [SetUp]
    public void SetUp()
    {
      _documentService = new Mock<IDocumentService>();
      _rejectionRepository = new Mock<IRejectionRepository>();
      _rejectionEngine = new RejectionEngine(_documentService.Object, _rejectionRepository.Object);
    }

    [Test]
    public void GivenADocumentId_WhenITryToRejectTheDocument_AndTheDocumentIsCheckedOut_TheDocumentIsRejected()
    {
      _documentService.Setup(c => c.GetDocument(It.IsAny<string>())).Returns(new Document
                          {
                            CheckOut =new CheckOut
                            {
                              CheckOutBy = "person",
                              CheckOutDate = DateTime.Now.AddHours(-1)
                            }
                          });

      _rejectionEngine.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
      _rejectionRepository.Verify(s => s.Create(It.IsAny<Rejection>()), Times.Once());
    }

    [Test]
    public void GivenADocumentId_WhenITryToRejectThedocument_AndTheDocumentIsAlreadyApproved_ADocumentAlreadyApprovedExceptionIsThrown()
    {
      _documentService.Setup(c => c.GetDocument(It.IsAny<string>())).Returns(new Document
      {
        Approval = new Approval
        {
          ApprovedBy = "person",
          ApprovedDate = DateTime.Now.AddHours(-1)
        }
      });

      Action act = () => _rejectionEngine.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentAlreadyApprovedException>();
    }

    [Test]
    public void GivenADocumentId_WhenITryToRejectTheDocument_AndTheDocumentIsAlreadyRejected_ADocumentAlreadyRejectedExceptionIsThrown()
    {
      _documentService.Setup(c => c.GetDocument(It.IsAny<string>())).Returns(new Document
      {
        Rejection = new Rejection()
        {
          RejectedBy = "person",
          RejectionDate = DateTime.Now.AddHours(-1)
        }
      });

      Action act = () => _rejectionEngine.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentAlreadyRejectedException>();
    }

    [Test]
    public void GivenADocumentId_WhenITryToRejectThedocument_TheDocumentIsRejected()
    {
      _documentService.Setup(c => c.GetDocument(It.IsAny<string>())).Returns(new Document());

      _rejectionEngine.RejectDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      _rejectionRepository.Verify(s => s.Create(It.IsAny<Rejection>()), Times.Once());
    }
  }
}
