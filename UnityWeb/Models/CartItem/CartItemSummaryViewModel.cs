namespace UnityWeb.Models.CartItem
{
  using UnityWeb.Models.Shared;

  public class CartItemSummaryViewModel
  {
    public CartItemSummaryViewModel()
    {
      DocumentWarningsViewModel = new DocumentWarningsViewModel();
    }

    public CartItemSummaryViewModel(int cartItems)
    {
      if (cartItems != 0)
      {
        Total = cartItems.ToString();
      }

      DocumentWarningsViewModel = new DocumentWarningsViewModel();
    }

    public DocumentWarningsViewModel DocumentWarningsViewModel { get; set; }

    public string Total { get; set; }
  }
}