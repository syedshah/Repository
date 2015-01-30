namespace Builder
{
  using Entities;

  public class CartItemBuilder : Builder<CartItem>
  {
    public CartItemBuilder(string shoppingCartId)
    {
      Instance = new CartItem(shoppingCartId);
    }

    public CartItemBuilder WithDocument(Document document)
    {
      Instance.Document = document;
      return this;
    }
  }
}
