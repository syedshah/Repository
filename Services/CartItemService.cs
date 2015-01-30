namespace Services
{
  using System;
  using System.Collections.Generic;
  using BusinessEngineInterfaces;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class CartItemService : ICartItemService
  {
    private readonly ICartItemEngine _cartItemEngine;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly ICheckOutRepository _checkOutRepository;

    public CartItemService(ICartItemEngine cartItemEngine, ICartItemRepository cartItemRepository, ICheckOutRepository checkOutRepository)
    {
      _cartItemEngine = cartItemEngine;
      _cartItemRepository = cartItemRepository;
      _checkOutRepository = checkOutRepository;
    }

    public void AddItem(string userName, string documentId, string cartId, string manCo, string docType, string subDocType)
    {
      try
      {
        _cartItemEngine.AddItem(documentId, cartId, userName, manCo, docType, subDocType);
      }
      catch (BasketContainsDifferentManCoException e)
      {
        throw new BasketContainsDifferentManCoException("Basket contains documents from a different manco");
      }
      catch (DocumentCurrentlyCheckedOutException e)
      {
        throw new DocumentCurrentlyCheckedOutException(
          string.Format("Unable to add document guid {0} to baset", documentId), e);
      }
      catch (UnableToCheckOutDocumentException e)
      {
        throw new UnableToCheckOutDocumentException(string.Format("Unable to check out document {0}", documentId));
      }
      catch (Exception e)
      {
        throw new UnityException(string.Format("Unable to add document guid {0} to basket", documentId), e);
      }
    }

    public void RemoveItem(string documentId, string cartId)
    {
      CartItem cartItem;
      CheckOut checkOut;
      try
      {
        cartItem = _cartItemRepository.GetCart(documentId, cartId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve cart item" , e);
      }

      try
      {
        checkOut = _checkOutRepository.GetCheckOut(cartItem.Document.DocumentId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get check out", e);
      }

      try
      {
        _cartItemRepository.Delete(cartItem);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to delete cart item", e);
      }

      try
      {
        _checkOutRepository.Delete(checkOut);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to delete check out");
      }
    }

    public IList<CartItem> GetCart(string cartId)
    {
      try
      {
        return _cartItemRepository.GetCart(cartId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get cart items", e);
      }
    }

    public int GetNumberOfItemsInCart(string cartId)
    {
      try
      {
        return _cartItemRepository.GetNumberOfItemsInCart(cartId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get the numnber cart items", e);
      }
    }

    public void ClearCart(string cartId)
    {
      IEnumerable<CartItem> cartItems;
      try
      {
        cartItems = _cartItemRepository.GetCart(cartId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get cart item", e);
      }

      foreach (var cartItem in cartItems)
      {
        CheckOut checkOut;
        try
        {
          checkOut = _checkOutRepository.GetCheckOut(cartItem.Document.DocumentId);
        }
        catch (Exception e)
        {
          throw new UnityException("Unable to get check out", e);
        }

        try
        {
          _cartItemRepository.Delete(cartItem);
        }
        catch (Exception e)
        {
          throw new UnityException("Unable to get delete cart item", e);
        }

        try
        {
          _checkOutRepository.Delete(checkOut);
        }
        catch (Exception e)
        {
          throw new UnityException("Unable to delete check out");
        }
      }
    }

    public PagedResult<CartItem> GetCartItems(string cartId, int pageNumber, int numberOfItems)
    {
      try
      {
        return _cartItemRepository.GetCart(cartId, pageNumber, numberOfItems);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get cart items", e);
      }
    }
  }
}
