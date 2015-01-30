namespace UnityWebTests.Helpers
{
  using System.Collections.Specialized;
  using System.Web;
  using Moq;

  public class FakeResponse : HttpResponseBase
  {
    // Routing calls this to account for cookieless sessions
    // It's irrelevant for the test, so just return the path unmodified
    private readonly HttpCookieCollection _cookies = new HttpCookieCollection();
    private readonly NameValueCollection _headers = new NameValueCollection();

    public override int StatusCode { get; set; }

    public override HttpCookieCollection Cookies
    {
      get { return _cookies; }
    }

    public override HttpCachePolicyBase Cache
    {
      get
      {
        var mock = new Mock<HttpCachePolicyBase>();
        return mock.Object;
      }
    }

    public override string ApplyAppPathModifier(string x)
    {
      return x;
    }

    public override void AddHeader(string name, string value)
    {
      _headers.Add(name, value);
    }

    public override NameValueCollection Headers
    {
      get
      {
        return _headers;
      }
    }
  }
}
