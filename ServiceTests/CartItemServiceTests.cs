namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using BusinessEngineInterfaces;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class CartItemServiceTests
  {
    private Mock<ICartItemEngine> _cartItemEngine;
    private Mock<ICartItemRepository> _cartItemRepository;
    private Mock<ICheckOutRepository> _checkOutRepository;
    private ICartItemService _cartItemService;

    [SetUp]
    public void SetUp()
    {
      _cartItemEngine = new Mock<ICartItemEngine>();
      _cartItemRepository = new Mock<ICartItemRepository>();
      _checkOutRepository = new Mock<ICheckOutRepository>();
      _cartItemService = new CartItemService(_cartItemEngine.Object, _cartItemRepository.Object, _checkOutRepository.Object);
    }

    [Test]
    public void GivenValidData_WhenADocumentIsAddedToACart_ThenTheCartIsUpdated()
    {
      _cartItemService.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
      _cartItemEngine.Verify(s => s.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GivenValidData_WhenADocumentIsAddedToACart_AndTheDocumentIsAlreadyCheckedOut_ThenADocumentCurrentlyCheckedOutExceptionIsThrown()
    {
      _cartItemEngine.Setup(c => c.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<DocumentCurrentlyCheckedOutException>();

      Action act = () => _cartItemService.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<DocumentCurrentlyCheckedOutException>();
    }

    [Test]
    public void GivenValidData_WhenADocumentIsAddedToACart_AndTheDocumentIsAlreadyCheckedOut_ThenAUnableToCheckOutDocumentExceptionIsThrown()
    {
      _cartItemEngine.Setup(c => c.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<UnableToCheckOutDocumentException>();

      Action act = () => _cartItemService.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<UnableToCheckOutDocumentException>();
    }

    [Test]
    public void GivenValidData_WhenADocumentIsAddedToACart_AndThereIsAlraedyADocumentInTheBasketFromADifferentManCo_ThenABasketContainsDifferentManCoExceptionIsThrown()
    {
      _cartItemEngine.Setup(c => c.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<BasketContainsDifferentManCoException>();

      Action act = () => _cartItemService.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

      act.ShouldThrow<BasketContainsDifferentManCoException>();
    }

    [Test]
    public void GivenACartId_WhenIAskToGetTotalNumberOfCartItems_IGetACountOFTheNumberOfCartItems()
    {
      _cartItemService.GetNumberOfItemsInCart(It.IsAny<string>());

      _cartItemRepository.Verify(s => s.GetNumberOfItemsInCart(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void GivenACartId_WhenIAskToGetTotalNumberOfCartItems_AndTheDatabaseIsUnavailable_IGetACountOFTheNumberOfCartItems()
    {
      _cartItemRepository.Setup(c => c.GetNumberOfItemsInCart(It.IsAny<string>())).Throws<Exception>();

      Action act = () => _cartItemService.GetNumberOfItemsInCart(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void GivenACartId_WhenIAskToGetAllItemsInTheCart_IGetAllItemsInTheCart()
    {
      _cartItemService.GetCart(It.IsAny<string>());

      _cartItemRepository.Verify(s => s.GetCart(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void GivenACartId_WhenIAskToGetAllItemsInTheCart_AndTheDatabaseIsUnavailable_IGetAllItemsInTheCart()
    {
      _cartItemRepository.Setup(c => c.GetCart(It.IsAny<string>())).Throws<Exception>();

      Action act = () => _cartItemService.GetCart(It.IsAny<string>());

      act.ShouldThrow<UnityException>();
    }

    [Test]
    public void WhenACartItemIsDeleted_AndItIsFound_ThenItIsDeleted()
    {
      _cartItemRepository.Setup(m => m.GetCart(It.IsAny<string>(), It.IsAny<string>())).Returns(new CartItem() { Document = new Document()
                                                                                                                              {
                                                                                                                                CheckOut = new CheckOut()
                                                                                                                              }});
      _checkOutRepository.Setup(m => m.GetCheckOut(It.IsAny<string>())).Returns(new CheckOut());

      _cartItemService.RemoveItem(It.IsAny<string>(), It.IsAny<string>());
      
      Assert.True(true, "No exceptions thrown");
      _cartItemRepository.Verify(s => s.Delete(It.IsAny<CartItem>()), Times.Once());
      _checkOutRepository.Verify(s => s.Delete(It.IsAny<CheckOut>()), Times.Once());
    }

    [Test]
    public void WhenACartItemIsDeleted_AndTheDatabaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      _cartItemRepository.Setup(m => m.GetCart(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
      Assert.Throws<UnityException>(() => _cartItemService.RemoveItem(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Test]
    public void WhenCartItemIsDeleted_AndTheDatabaseIsNotAvailable_ThenAnUnityExceptionIsThrown()
    {
      _cartItemRepository.Setup(m => m.GetCart(It.IsAny<string>(), It.IsAny<string>())).Returns(new CartItem());
      _cartItemRepository.Setup(m => m.Delete(It.IsAny<CartItem>())).Throws<Exception>();
      Assert.Throws<UnityException>(() => _cartItemService.RemoveItem(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Test]
    public void GivenAValidBasket_WhenIAskToClearTheBasket_TheBasketIsCleared()
    {
      _cartItemRepository.Setup(p => p.GetCart(It.IsAny<string>()))
                         .Returns(new List<CartItem>
                           {
                             new CartItem() {Id = 1, Document = new Document()},
                             new CartItem() {Id = 2, Document = new Document()},
                             new CartItem() {Id = 3, Document = new Document()}
                           });
      _cartItemService.ClearCart(It.IsAny<string>());
      _cartItemRepository.Verify(c => c.Delete(It.IsAny<CartItem>()), Times.Exactly(3));
      _checkOutRepository.Verify(c => c.Delete(It.IsAny<CheckOut>()), Times.Exactly(3));
    }
  }
}
