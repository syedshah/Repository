namespace UnityWebTests.Helpers
{
  using System.Collections.Specialized;
  using System.Web;

  public class FakeRequest : HttpRequestBase
  {
    public readonly NameValueCollection Values = new NameValueCollection();

    public FakeRequest()
    {
    }

    public bool SecureConnection = true;

    public bool Local = true;

    private readonly HttpCookieCollection _cookies = new HttpCookieCollection();


    public override HttpCookieCollection Cookies
    {
      get
      {
        return _cookies;
      }
    }

    public override bool IsLocal
    {
      get
      {
        return Local;
      }
    }

    public override bool IsSecureConnection
    {
      get
      {
        return SecureConnection;
      }
    }

    public override string this[string key]
    {
      get
      {
        return Values[key];
      }
    }

    public override NameValueCollection Headers
    {
      get
      {
        return Values;
      }
    }
  }
}

