namespace Entities
{
  using System;
  using System.Web;

  public class CartItem
  {
    public CartItem()
    {
    }

    public CartItem(HttpContextBase context) : this()
    {
      CartId = GetCartId(context);
    }

    public CartItem(string cartId)
    {
      CartId = cartId;
    }

    public string GetCartId(HttpContextBase context)
    {
      if (context.Session["CartId"] == null)
      {
        if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
        {
          context.Session["CartId"] = context.User.Identity.Name;
        }
        else
        {
          Guid tempCartId = Guid.NewGuid();
          context.Session["CartId"] = tempCartId.ToString();
        }
      }

      return context.Session["CartId"].ToString();
    }

    public int Id { get; set; }

    public string CartId { get;  set; }

    public int DocumentId { get; set; }

    public virtual Document Document { get; set; }
  }
}
