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
  public class ApprovalEngineTests
  {
    private Mock<IDocumentService> _documentService;
    private Mock<IApprovalRepository> _approvalRepository;
    private Mock<IRejectionRepository> _rejectionRepository;
    private IApprovalEngine _approvalEngine;

    [SetUp]
    public void SetUp()
    {
      _documentService = new Mock<IDocumentService>();
      _approvalRepository = new Mock<IApprovalRepository>();
      _rejectionRepository = new Mock<IRejectionRepository>();
      _approvalEngine = new ApprovalEngine(_documentService.Object, _approvalRepository.Object);
    }

    [Test]
    public void GivenADocumentId_WhenITryToApproveThedocument_AndTheDocumentIsCheckedOut_TheDocumentIsApproved()
    {
      _documentService.Setup(c => c.GetDocument(It.IsAny<string>())).Returns(new Document
                          {
                            CheckOut =new CheckOut
                            {
                              CheckOutBy = "person",
                              CheckOutDate = DateTime.Now.AddHours(-1)
                            }
                          });

      _approvalEngine.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      _approvalRepository.Verify(s => s.Create(It.IsAny<Approval>()), Times.Once());
    }

    [Test]
    public void GivenADocumentId_WhenITryToApproveThedocument_AndTheDocumentIsAlreadyApproved_ADocumentAlreadyApprovedExceptionIsThrown()
    {
      _documentService.Setup(c => c.GetDocument(It.IsAny<string>())).Returns(new Document
      {
        Approval = new Approval
        {
          ApprovedBy = "person",
          ApprovedDate = DateTime.Now.AddHours(-1)
        }
      });

      Action act = () => _approvalEngine.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentAlreadyApprovedException>();
    }

    [Test]
    public void GivenADocumentId_WhenITryToApproveTheDocument_AndTheDocumentIsAlreadyRejected_ADocumentAlreadyRejectedExceptionIsThrown()
    {
      _documentService.Setup(c => c.GetDocument(It.IsAny<string>())).Returns(new Document
      {
        Rejection = new Rejection()
        {
          RejectedBy = "person",
          RejectionDate = DateTime.Now.AddHours(-1)
        }
      });

      Action act = () => _approvalEngine.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentAlreadyRejectedException>();
    }

    [Test]
    public void GivenADocumentId_WhenITryToApproveThedocument_TheDocumentIsApproved()
    {
      _documentService.Setup(c => c.GetDocument(It.IsAny<string>())).Returns(new Document());

      _approvalEngine.ApproveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      _approvalRepository.Verify(s => s.Create(It.IsAny<Approval>()), Times.Once());
    }
  }
}
