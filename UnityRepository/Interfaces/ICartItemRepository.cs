namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface ICartItemRepository : IRepository<CartItem>
  {
    CartItem GetCart(string documentId, string cartId);
    PagedResult<CartItem> GetCart(string cartId, int pageNumber, int numberOfItems);
    List<CartItem> GetCart(string shoppingCartId);
    int GetNumberOfItemsInCart(string cartId);
  }
}
