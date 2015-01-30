namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class CartItemRepository : BaseEfRepository<CartItem>, ICartItemRepository
  {
    public CartItemRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public CartItem GetCart(string documentId, string cartId)
    {
      return (from c in Entities
                .Include(d => d.Document)
              where c.Document.DocumentId == documentId
                    && c.CartId == cartId
              select c).FirstOrDefault();
    }

    public PagedResult<CartItem> GetCart(string cartId, int pageNumber, int numberOfItems)
    {
      var cartItems = Entities as DbSet<CartItem>;

      return new PagedResult<CartItem>
               {
                 CurrentPage = pageNumber,
                 ItemsPerPage = numberOfItems,
                 TotalItems = Entities.Count(c => c.CartId == cartId),
                 Results =
                   cartItems.Include(d => d.Document)
                           .Include(d => d.Document.DocType)
                           .Include(d => d.Document.ManCo)
                           .Include(d => d.Document.SubDocType)
                           .Include(d => d.Document.Approval)
                           .Include(d => d.Document.Rejection)
                           .Where(c => c.CartId == cartId)
                           .OrderBy(c => c.Id)
                           .Skip((pageNumber - 1) * numberOfItems)
                           .Take(numberOfItems)
                           .ToList(),
                 StartRow = ((pageNumber - 1) * numberOfItems) + 1,
                 EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
               };
    }

    public List<CartItem> GetCart(string shoppingCartId)
    {
      return Entities
        .Include(d => d.Document)
        .Where(cart => cart.CartId == shoppingCartId).ToList();
    }

    public int GetNumberOfItemsInCart(string cartId)
    {
      return Entities.Count(cart => cart.CartId == cartId);
    }
  }
}
