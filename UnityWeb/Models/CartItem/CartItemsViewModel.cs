namespace UnityWeb.Models.CartItem
{
  using System.Collections.Generic;
  using UnityWeb.Models.Helper;

  public class CartItemsViewModel
  {
    public CartItemsViewModel()
    {
      PagingInfo = new PagingInfoViewModel();
    }

    private List<CartItemViewModel> _cartItems = new List<CartItemViewModel>();

    public int CartTotal
    {
      get
      {
        return _cartItems.Count;
      }
    }

    public List<CartItemViewModel> CartItems
    {
      get { return _cartItems; }
      set { _cartItems = value; }
    }

    public PagingInfoViewModel PagingInfo { get; set; }

    public string CurrentPage { get; set; }

    public void AddCartItems(Entities.PagedResult<Entities.CartItem> cartItems)
    {
      foreach (Entities.CartItem cartItem in cartItems.Results)
      {
        var civm = new CartItemViewModel(cartItem);
        CartItems.Add(civm);
      }

        PagingInfo = new PagingInfoViewModel
                  {
                    CurrentPage = cartItems.CurrentPage,
                    TotalItems = cartItems.TotalItems,
                    ItemsPerPage = cartItems.ItemsPerPage,
                    TotalPages = cartItems.TotalPages,
                    StartRow = cartItems.StartRow,
                    EndRow = cartItems.EndRow
                  };

      CurrentPage = PagingInfo.CurrentPage.ToString();
    }
  }
}