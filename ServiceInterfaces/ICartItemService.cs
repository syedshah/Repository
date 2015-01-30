namespace ServiceInterfaces
{
  using System.Collections.Generic;

  using Entities;

  public interface ICartItemService
  {
    void AddItem(string userName, string documentId, string cartId, string manCo, string docType, string subDocType);
    void RemoveItem(string documentId, string cartId);
    PagedResult<CartItem> GetCartItems(string cartId, int pageNumber, int numberOfItems);
    IList<CartItem> GetCart(string cartId);
    int GetNumberOfItemsInCart(string cartId);
    void ClearCart(string cartId);
  }
}
