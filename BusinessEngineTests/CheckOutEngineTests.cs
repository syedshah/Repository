namespace BusinessEngineTests
{
  using System;
  using BusinessEngineInterfaces;
  using BusinessEngines;
  using Entities;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class CheckOutEngineTests
  {
    private Mock<ICheckOutRepository> _checkOutRepository;
    private Mock<IDocumentService> _documentService;
    private ICheckOutEngine _checkOutEngine;

    [SetUp]
    public void SetUp()
    {
      _checkOutRepository = new Mock<ICheckOutRepository>();
      _documentService = new Mock<IDocumentService>();
      _checkOutEngine = new CheckOutEngine(_checkOutRepository.Object, _documentService.Object);
    }

    [Test]
    public void GivenADocumentId_WhenTheDocumentIsCheckedOut_IAmNotifiedThatTheDocumentIsCheckedOut()
    {
      _checkOutRepository.Setup(c => c.GetCheckOut("1")).Returns(new CheckOut { DocumentId = 1 });

      bool isCheckedOut = _checkOutEngine.IsDocumentCheckedOut("1");

      isCheckedOut.Should().BeTrue();
    }

    [Test]
    public void GivenADocumentId_WhenTheDocumentIsNotCheckedOut_IAmNotifiedThatTheDocumentIsNotCheckedOut()
    {
      CheckOut checkOut = null;

      _checkOutRepository.Setup(c => c.GetCheckOut("2")).Returns(checkOut);

      bool isCheckedOut = _checkOutEngine.IsDocumentCheckedOut("2");

      isCheckedOut.Should().BeFalse();
    }

    [Test]
    public void GivenAUserAndDocumentIdWhenITryToCheckout_AndTheDocumentIsNotCheckedOut_AndTheDocumentExists_TheDocumentIsCheckedOut()
    {
      CheckOut checkOut = null;

      _checkOutRepository.Setup(c => c.GetCheckOut("1")).Returns(checkOut);
      _documentService.Setup(d => d.GetDocument("1")).Returns(new Document() { Id = 1 });

      var checkOutResult = _checkOutEngine.CheckOutDocument(It.IsAny<string>(), "1", "manCo", "docType", "subDocType");

      checkOutResult.Should().NotBeNull();
    }

    //PAUL - NEED TO ADD THIS TEST WHEN DAVe VOSE DOES HIS ARCHIVE WORK
   /* [Test]
    public void GivenAUserAndDocumentIdWhenITryToCheckout_AndTheDocumentIsNotCheckedOut_AndTheDocumentDoesNotExists_ThenAUnityExceptionIsThrown()
    {
      CheckOut checkOut = null;
      Document document = null;

      _checkOutRepository.Setup(c => c.GetCurrentlyCheckedOutByDocument("1")).Returns(checkOut);
      _documentRepository.Setup(d => d.GetDocument("1")).Returns(document);

      Action act = () => _checkOutEngine.CheckOutDocument(It.IsAny<string>(), "1");

      act.ShouldThrow<UnityException>();
    }*/
  }
}
