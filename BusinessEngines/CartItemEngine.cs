namespace BusinessEngines
{
  using System.Linq;

  using BusinessEngineInterfaces;
  using Entities;
  using Exceptions;
  using UnityRepository.Interfaces;

  public class CartItemEngine : ICartItemEngine
  {
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly ICheckOutEngine _checkOutEngine;

    public CartItemEngine(ICartItemRepository cartItemRepository, IDocumentRepository documentRepository, ICheckOutEngine checkOutEngine)
    {
      _cartItemRepository = cartItemRepository;
      _documentRepository = documentRepository;
      _checkOutEngine = checkOutEngine;
    }

    public void AddItem(string documentId, string cartId, string userName, string manCo, string docType, string subDocType)
    {
      var cartItems = _cartItemRepository.GetCart(cartId);

      var cartitem = cartItems.FirstOrDefault(c => c.Document.ManCo.Code != manCo);

      if (cartitem != null)
      {
        throw new BasketContainsDifferentManCoException("Basket has documents from a different man co");
      }

      bool documentIsCheckedOut = _checkOutEngine.IsDocumentCheckedOut(documentId);

      if (documentIsCheckedOut)
      {
        throw new DocumentCurrentlyCheckedOutException(string.Format("Document {0} is already checked out", cartId));
      }

      var checkOut = _checkOutEngine.CheckOutDocument(userName, documentId, manCo, docType, subDocType);

      if (checkOut == null)
      {
        throw new UnableToCheckOutDocumentException(string.Format("Unable to check out document {0}", documentId));
      }

      var document = _documentRepository.GetDocument(documentId);

      if (document == null)
      {
        throw new UnableToRetrieveDocumentException(string.Format("Unable to retrieve document {0}", documentId));
      }

      var cartItem = new CartItem
                       {
                         CartId = cartId, 
                         DocumentId = document.Id
                       };

      _cartItemRepository.Create(cartItem);
    }
  }
}
