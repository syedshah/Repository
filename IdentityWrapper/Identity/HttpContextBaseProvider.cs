namespace IdentityWrapper.Identity
{
  using System.Web;
  using IdentityWrapper.Interfaces;

  /// <summary>
  /// HttpContextBase Provider
  /// </summary>
  public class HttpContextBaseProvider : IHttpContextBaseProvider
  {
    public HttpContextBaseProvider()
    {
      this._context = new HttpContextWrapper(System.Web.HttpContext.Current);
    }

    private HttpContextBase _context;

    public HttpContextBase HttpContext
    {
      get
      {
        return this._context;
      }
    }
  }
}
