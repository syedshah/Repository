namespace IdentityWrapperTests.Identity
{
  using System.Collections.Generic;
  using System.IO;
  using System.Web;
  using Microsoft.Owin;
  using Moq;
  using NUnit.Framework;
 
  public class BaseIdentityProviderTest
  {
    protected Mock<HttpRequestBase> MockedRequest;
    protected Mock<HttpResponseBase> MockedResponse;
    protected Mock<HttpContextBase> MockedHttpContext;
    protected IOwinContext _owinContext;
    protected HttpContextBase context;
    protected HttpContext xContext;

    [SetUp]
    public void HttpContextSetUp()
    {
        this.MockedRequest = new Mock<HttpRequestBase>();
        this.MockedResponse = new Mock<HttpResponseBase>();
        this.MockedHttpContext = new Mock<HttpContextBase>();
        
        this.xContext = new System.Web.HttpContext(new HttpRequest("FILENAME", "http://localhost:27815/Session/New", "y=treat"), new HttpResponse(new XWriter()));
        
        this.context = new HttpContextWrapper(xContext);

        var itemDict = new Dictionary<string,object>();
       //itemDict.Add("owin.Environment");
       
        this.MockedHttpContext.Setup(m => m.Request).Returns(this.MockedRequest.Object);
        this.MockedHttpContext.Setup(m => m.Response).Returns(this.MockedResponse.Object);
        this.MockedHttpContext.Setup(m => m.Items).Returns(itemDict);

    }
  }

    public class XWriter : TextWriter
    {
        public XWriter()
        {
            
        }

        public override System.Text.Encoding Encoding
        {
            get { throw new System.NotImplementedException(); }
        }
    }
    
}
