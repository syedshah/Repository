namespace BusinessEngineTests
{
  using System;
  using System.Collections.Generic;
  using BusinessEngineInterfaces;
  using BusinessEngines;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class CartItemEngineTests
  {
    private Mock<ICartItemRepository> _cartItemRepository;
    private Mock<IDocumentRepository> _documentRepository;
    private Mock<ICheckOutEngine> _checkOutEngine;
    private ICartItemEngine _cartItemEngine;

    [SetUp]
    public void SetUp()
    {
      _cartItemRepository = new Mock<ICartItemRepository>();
      _documentRepository = new Mock<IDocumentRepository>();
      _checkOutEngine = new Mock<ICheckOutEngine>();
      _cartItemEngine = new CartItemEngine(_cartItemRepository.Object, _documentRepository.Object, _checkOutEngine.Object);
    }

    [Test]
    public void GivenADocumentId_WhenIRequestTheDocumentToBeAddedToTheCartItem_ItIsAddedToTheCart()
    {
      _cartItemRepository.Setup(c => c.GetCart(It.IsAny<string>()))
                         .Returns(
                           new List<CartItem>());

      _checkOutEngine.Setup(c => c.IsDocumentCheckedOut(It.IsAny<string>())).Returns(false);
      _checkOutEngine.Setup(c => c.CheckOutDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CheckOut() { DocumentId = 1 });
      _documentRepository.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(new Document() { Id = 1 });

      _cartItemEngine.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      _cartItemRepository.Verify(s => s.Create(It.IsAny<CartItem>()), Times.Once());
    }

    [Test]
    public void GivenADocumentId_WhenIRequestTheDocumentToBeAddedToAndEmptyCartItem_AndTheDocumentIsCheckedOut_AnUDocumentCurrentlyCheckedOutExceptionIsThrown()
    {
      _cartItemRepository.Setup(c => c.GetCart(It.IsAny<string>()))
                         .Returns(
                           new List<CartItem>());

      _checkOutEngine.Setup(c => c.IsDocumentCheckedOut(It.IsAny<string>())).Returns(true);

      Action act = () => _cartItemEngine.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentCurrentlyCheckedOutException>();
    }

    [Test]
    public void GivenADocumentId_WhenIRequestTheDocumentToBeAddedToTheCartItem_AndThererIsAlreadyADocumentInTheBasketForADifferentManCo_ABasketContainsDifferentManCoExceptionIsThrown()
    {
      _cartItemRepository.Setup(c => c.GetCart(It.IsAny<string>()))
                         .Returns(
                           new List<CartItem>
                             {
                               new CartItem { Document = new Document { ManCo = new ManCo { Code = "manco2"} } },
                               new CartItem { Document = new Document { ManCo = new ManCo { Code = "manco1"} }}
                             });

      CheckOut checkOut = null;

      _checkOutEngine.Setup(c => c.IsDocumentCheckedOut(It.IsAny<string>())).Returns(false);
      _checkOutEngine.Setup(c => c.CheckOutDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(checkOut);

      Action act = () => _cartItemEngine.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), "manco1", It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<BasketContainsDifferentManCoException>();
    }

    [Test]
    public void GivenADocumentId_WhenIRequestTheDocumentToBeAddedToTheCartItem_AndTheDocumentCannotBeCheckedOut_AnUnableToCheckOutDocumentExceptionIsThrown()
    {
      _cartItemRepository.Setup(c => c.GetCart(It.IsAny<string>()))
                   .Returns(
                     new List<CartItem>
                             {
                               new CartItem { Document = new Document { ManCo = new ManCo { Code = "manco2"} } },
                               new CartItem { Document = new Document { ManCo = new ManCo { Code = "manco2" } }}
                             });

      CheckOut checkOut = null;

      _checkOutEngine.Setup(c => c.IsDocumentCheckedOut(It.IsAny<string>())).Returns(false);
      _checkOutEngine.Setup(c => c.CheckOutDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(checkOut);

      Action act = () => _cartItemEngine.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), "manco2", It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnableToCheckOutDocumentException>();
    }

    [Test]
    public void GivenADocumentId_WhenIRequestTheDocumentToBeAddedToTheCartItem_AndTheDocumentCannotBeRetrieved_AnUnableToRetrieveDocumentExceptionIsThrown()
    {
      _cartItemRepository.Setup(c => c.GetCart(It.IsAny<string>()))
                   .Returns(
                     new List<CartItem>
                             {
                               new CartItem { Document = new Document { ManCo = new ManCo { Code = "manco2" } } },
                               new CartItem { Document = new Document { ManCo = new ManCo { Code = "manco2" } }}
                             });

      Document document = null;

      _checkOutEngine.Setup(c => c.IsDocumentCheckedOut(It.IsAny<string>())).Returns(false);
      _checkOutEngine.Setup(c => c.CheckOutDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CheckOut() { DocumentId = 1 });
      _documentRepository.Setup(d => d.GetDocument(It.IsAny<string>())).Returns(document);

      Action act = () => _cartItemEngine.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), "manco2", It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnableToRetrieveDocumentException>();
    }
  }
}
