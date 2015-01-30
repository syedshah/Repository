namespace UnityWebTests.Routing
{
  using System.Collections.Specialized;
  using System.Web;
  using System.Web.Mvc;
  using System.Web.Routing;
  using Moq;
  using NUnit.Framework;
  using UnityWeb;
  using UnityWebTests.Helpers;

  [TestFixture]
  public class OutboundRoutingTests
  {
    private string GetOutboundUrl(object routeValues)
    {
      // Get route configuration and mock request context
      var routes = new RouteCollection();
      routes.RegisterRoutes();
      var mockHttpContext = new Mock<HttpContextBase>();
      var mockRequest = new Mock<HttpRequestBase>();
      var fakeResponse = new FakeResponse();
      mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
      mockHttpContext.Setup(x => x.Response).Returns(fakeResponse);
      mockRequest.Setup(x => x.ApplicationPath).Returns("/");

      // Generate the outbound URL
      var ctx = new RequestContext(mockHttpContext.Object, new RouteData());
      return routes.GetVirtualPath(ctx, new RouteValueDictionary(routeValues)).VirtualPath;
    }

    private static UrlHelper GetUrlHelper(string appPath = "/", RouteCollection routes = null)
    {
      if (routes == null)
      {
        routes = new RouteCollection();
        routes.RegisterRoutes();
      }

      HttpContextBase httpContext = new StubHttpContextForRouting(appPath);
      var routeData = new RouteData();
      routeData.Values.Add("controller", "defaultcontroller");
      routeData.Values.Add("action", "defaultaction");
      var requestContext = new RequestContext(httpContext, routeData);
      var helper = new UrlHelper(requestContext, routes);
      return helper;
    }

    public class StubHttpContextForRouting : HttpContextBase
    {
      private readonly StubHttpRequestForRouting _request;

      private readonly StubHttpResponseForRouting _response;

      public StubHttpContextForRouting(string appPath = "/", string requestUrl = "~/")
      {
        _request = new StubHttpRequestForRouting(appPath, requestUrl);
        _response = new StubHttpResponseForRouting();
      }

      public override HttpRequestBase Request
      {
        get
        {
          return _request;
        }
      }

      public override HttpResponseBase Response
      {
        get
        {
          return _response;
        }
      }
    }

    public class StubHttpRequestForRouting : HttpRequestBase
    {
      private readonly string _appPath;

      private readonly string _requestUrl;

      public StubHttpRequestForRouting(string appPath, string requestUrl)
      {
        _appPath = appPath;
        _requestUrl = requestUrl;
      }

      public override string ApplicationPath
      {
        get
        {
          return _appPath;
        }
      }

      public override string AppRelativeCurrentExecutionFilePath
      {
        get
        {
          return _requestUrl;
        }
      }

      public override string PathInfo
      {
        get
        {
          return "";
        }
      }

      public override NameValueCollection ServerVariables
      {
        get
        {
          return new NameValueCollection();
        }
      }
    }

    public class StubHttpResponseForRouting : HttpResponseBase
    {
      public override string ApplyAppPathModifier(string virtualPath)
      {
        return virtualPath;
      }
    }

    [Test]
    public void ActionWithSpecificControllerAndAction()
    {
      UrlHelper helper = GetUrlHelper();

      string url = helper.Action("index", "Dashboard");

      Assert.AreEqual("/", url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAGridRunUnapprovedPageIndexView_ThenIGetTheCorrectUrl()
    {
      Assert.AreEqual("/gridrun/unapproved", GetOutboundUrl(new
                                                             {
                                                               controller = "GridRun",
                                                               action = "Unapproved"
                                                             }));

    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAGridRunHouseHoldingPageIndexView_ThenIGetTheCorrectUrl()
    {
      Assert.AreEqual("/gridrun/householding", GetOutboundUrl(new
      {
        controller = "GridRun",
        action = "HouseHolding"
      }));
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAnEnvironmentPageIndexView_ThenIGetTheCorrectUrl()
    {
      Assert.AreEqual("/environment/index", GetOutboundUrl(new
                                              {
                                                controller = "Environment",
                                                action = "Index"
                                              }));
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAnAccountPageSummaryView_ThenIGetTheCorrectUrl()
    {
      Assert.AreEqual("/account/summary", GetOutboundUrl(new
      {
        controller = "Account",
        action = "Summary"
      }));
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForTheDashBoardIndexView_ThenIGetTheCorrectUrl()
    {
      Assert.AreEqual("/", this.GetOutboundUrl(new
                                                  {
                                                    controller = "Dashboard",
                                                    action = "Index"
                                                  }));
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAGridRunSearchView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/gridrun/search/1";

      string url = helper.Action("Search", "GridRun", new { grid = "1" });

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForABigZipRunSearchView_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/bigzip/search/1";

        string url = helper.Action("Search", "BigZip", new { file = "1" });

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForACartItemAddToCarthView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/cartitem/addtocart";

      string url = helper.Action("AddToCart", "CartItem");

      Assert.AreEqual(expectedurl, url);
    }


    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForACartItemDownload_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/cartitem/Download";

        string url = helper.Action("Download", "CartItem");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForACartItemExportDocumentsToZipView_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/cartitem/ExportDocumentsToZip";

        string url = helper.Action("ExportDocumentsToZip", "CartItem");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAnApprovalDocumentView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/approval/document";

      string url = helper.Action("Document", "Approval");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForARejectionDocumentView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/reject/document";

      string url = helper.Action("Document", "Reject");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForARejectionGridView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/reject/grid";

      string url = helper.Action("Grid", "Reject");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForARejectionBasketView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/reject/basket";

      string url = helper.Action("Basket", "Reject");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAnApprovalGridView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/approval/grid";

      string url = helper.Action("Grid", "Approval");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAnApprovalBasketView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/approval/basket";

      string url = helper.Action("Basket", "Approval");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForACartItemRemoveFromCartView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/cartitem/RemoveFromCart";

      string url = helper.Action("RemoveFromCart", "CartItem");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForACartItemClearCartView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/cartitem/ClearCart";

      string url = helper.Action("ClearCart", "CartItem");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForADocumentSearchView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/document/searchgrid/1";

      string url = helper.Action("SearchGrid", "Document", new { grid = "1" });

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForSearchSubDocTypeView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/search/subdoctype?subDocTypeId=1";

      string url = helper.Action("SubDocType", "Search", new { subDocTypeId = "1" });

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForADocumentShowView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/document/show/1";

      string url = helper.Action("Show", "Document", new { documentId = "1" });

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForADocumentPDFContainerView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/document/pdfcontainer/1";

      string url = helper.Action("PdfContainer", "Document", new { documentId = "1" });

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForADocumentStatusView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/document/status/1";

      string url = helper.Action("Status", "Document", new { documentId = "1" });

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAGridRunStatusView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/gridrun/status/1";

      string url = helper.Action("Status", "GridRun", new { gridrunid = "1" });

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAFileSearchView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/file/search?file=a";

      string url = helper.Action("Search", "File", new { file = "a" });

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAQuickSearchIndexView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/quicksearch/index";

      string url = helper.Action("Index", "QuickSearch");
    
      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForSearchResetView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/search/reset";

      string url = helper.Action("Reset", "Search");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForDocumentIndexView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/document/index";

      string url = helper.Action("Index", "Document");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAQuickSearchGridView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/quicksearch/grid";

      string url = helper.Action("Grid", "QuickSearch");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAQuickSearchFileView_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/quicksearch/file";

      string url = helper.Action("File", "QuickSearch");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAQuickSearchBigZipView_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/quicksearch/bigzip";

        string url = helper.Action("BigZip", "QuickSearch");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForIndexCreate_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/index/create";

      string url = helper.Action("Create", "Index");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForSubDocTypeCreate_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/subdoctype/create";

      string url = helper.Action("Create", "SubDocType");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForOtfIndex_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Otf/index";

        string url = helper.Action("Index", "Otf");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForOtfShow_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Otf/show";

        string url = helper.Action("Show", "Otf");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForOtfEdit_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Otf/edit";

        string url = helper.Action("Edit", "Otf");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForOtfDelete_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/Otf/delete";

      string url = helper.Action("Delete", "Otf");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForOtfCreate_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Otf/create";

        string url = helper.Action("Create", "Otf");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForApprovalAdminControllerCreate_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/autoApproval/create";

        string url = helper.Action("Create", "AutoApproval");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForUserReportsControllerIndex_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/userreports/index";

      string url = helper.Action("Index", "UserReports");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForUserReportsControllerRun_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/userreports/run";

      string url = helper.Action("Run", "UserReports");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForKpiReportsControllerInex_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/kpireports/index";

      string url = helper.Action("Index", "KpiReports");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForKpiReportsControllerRun_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/kpireports/run";

      string url = helper.Action("Run", "KpiReports");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForKpiReportsControllerExport_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/kpireports/export";

      string url = helper.Action("Export", "KpiReports");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForApprovalAdminControllerEdit_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/autoApproval/edit/3";

        string url = helper.Action("Edit", "AutoApproval", new { autoApprovalId = "3" });

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForApprovalAdminControllerDelete_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/autoApproval/delete/3";

      string url = helper.Action("Delete", "AutoApproval", new { autoApprovalId = "3" });

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForApprovalAdminControllerDocumentApprovals_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/autoApproval/AutoApprovals/3";

        string url = helper.Action("AutoApprovals", "AutoApproval", new { manCoId = "3" });

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForIndexEdit_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/index/edit/1";

      string url = helper.Action("Edit", "Index", new { indexId = "1" });
        

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForUserEdit_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/user/edit/1";

        string url = helper.Action("Edit", "User", new { userName = "1" });

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForUserDelete_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/user/delete/1";

        string url = helper.Action("Delete", "User", new { userName = "1" });

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForUserIndex_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/user/index";

        string url = helper.Action("Index", "User");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForForgottenPasswordPage_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Password/Forgotten";

        string url = helper.Action("forgotten", "password");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForChangePasswordPage_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Password/Change";

        string url = helper.Action("change", "password");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForChangeCurrentPasswordPage_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Password/ChangeCurrent";

        string url = helper.Action("Changecurrent", "Password");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSecurityQuestionsPage_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Password/SecurityQuestions/3";

        string url = helper.Action("SecurityQuestions", "Password", new {userId = "3"});

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForUserRetrieveManCoes_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/user/retrieveManCoes/2";

        string url = helper.Action("retrieveManCoes", "User", new {domicileId = "2"});

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForALogOut_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/session/remove";

      string url = helper.Action("Remove", "Session");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAnExpire_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/session/expired";

      string url = helper.Action("Expired", "Session");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForALockedOut_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/session/lockedout";

      string url = helper.Action("LockedOut", "Session");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForAnInactive_ThenIGetTheCorrectUrl()
    {
      UrlHelper helper = GetUrlHelper();

      string expectedurl = "/session/inactive";

      string url = helper.Action("Inactive", "Session");

      Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForSecurityIndex_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Security/Index";

        string url = helper.Action("index", "Security");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForSecurityAddAnswers_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Security/AddAnswers";

        string url = helper.Action("addanswers", "Security");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToAddOrUpdateSecurityAnswers_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Security/AddOrUpdate";

        string url = helper.Action("addOrUpdate", "Security");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskToEditSecurityAnswers_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Security/EditAnswers";

        string url = helper.Action("editAnswers", "Security");

        Assert.AreEqual(expectedurl, url);
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskPasswordComplexityView_ThenIGetTheCorrectUrl()
    {
        UrlHelper helper = GetUrlHelper();

        string expectedurl = "/Password/PasswordComplexity";

        string url = helper.Action("passwordComplexity", "Password");

        Assert.AreEqual(expectedurl, url);
    }

  }
}
