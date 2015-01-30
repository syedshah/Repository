namespace UnityWeb
{
  using System.Web.Mvc;
  using System.Web.Routing;

  public static class RouteConfig
  {
    public static void RegisterRoutes(this RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(name: "Error", url: "Error", defaults: new { controller = "Error", action = "Index" });

      routes.MapRoute(
        name: "QuickSearch-grid",
        url: "quicksearch/grid/{term}",
        defaults: new { controller = "QuickSearch", action = "Grid", term = UrlParameter.Optional });

      routes.MapRoute(
        name: "QuickSearch-file",
        url: "quicksearch/file/{term}",
        defaults: new { controller = "QuickSearch", action = "File", term = UrlParameter.Optional });

      routes.MapRoute(
        name: "QuickSearch-bigZip",
        url: "quicksearch/bigzip/{term}",
        defaults: new { controller = "QuickSearch", action = "BigZip", term = UrlParameter.Optional });

      routes.MapRoute(
        name: "BigZip-search",
        url: "bigzip/search/{file}",
        defaults: new { controller = "BigZip", action = "Search", file = UrlParameter.Optional });

      routes.MapRoute(
        name: "GridRun-search",
        url: "gridrun/search/{grid}",
        defaults: new { controller = "GridRun", action = "Search", grid = UrlParameter.Optional });

      routes.MapRoute(
        name: "Document-search-criteria",
        url: "document/search/{searchViewModel}",
        defaults: new { controller = "Document", action = "Search", searchViewModel = UrlParameter.Optional });

      routes.MapRoute(
        name: "Document-show",
        url: "document/show/{documentid}",
        defaults: new { controller = "Document", action = "Show", documentId = UrlParameter.Optional });

      routes.MapRoute(
        name: "Document-pdfcontainer",
        url: "document/pdfcontainer/{documentid}",
        defaults: new { controller = "Document", action = "PdfContainer", documentId = UrlParameter.Optional });

      routes.MapRoute(
        name: "Document-status",
        url: "document/status/{documentid}",
        defaults: new { controller = "Document", action = "Status", documentId = UrlParameter.Optional });

      routes.MapRoute(name: "Otf-index", url: "Otf/index", defaults: new { controller = "Otf", action = "Index" });

      routes.MapRoute(
        name: "Otf-edit",
        url: "Otf/edit/{appManCoEmailId}",
        defaults: new { controller = "Otf", action = "Edit", appManCoEmailId = UrlParameter.Optional });

      routes.MapRoute(
        name: "Otf-delete",
        url: "Otf/delete/{appManCoEmailId}",
        defaults: new { controller = "Otf", action = "Delete", appManCoEmailId = UrlParameter.Optional });

      routes.MapRoute(
        name: "Otf-show",
        url: "Otf/show/{OtfParameterModel}",
        defaults: new { controller = "Otf", action = "Show", OtfParameterModel = UrlParameter.Optional });

      routes.MapRoute(
        name: "Otf-edit-post",
        url: "Otf/edit/{EditAppManCoEmailViewModel}",
        defaults: new { controller = "Otf", action = "Edit", EditAppManCoEmailViewModel = UrlParameter.Optional });

      routes.MapRoute(
        name: "Otf-create",
        url: "Otf/create/{AddAppManCoEmailViewModel}",
        defaults: new { controller = "Otf", action = "Create", AddAppManCoEmailViewModel = UrlParameter.Optional });

      routes.MapRoute(
        name: "GridRun-status",
        url: "gridrun/status/{gridrunid}",
        defaults: new { controller = "GridRun", action = "Status", gridRunId = UrlParameter.Optional });

      routes.MapRoute(
        name: "File-search",
        url: "file/search/{filename}",
        defaults: new { controller = "file", action = "Search", filename = UrlParameter.Optional });

      routes.MapRoute(name: "Admin-index", url: "admin/index", defaults: new { controller = "Admin", action = "Index" });

      routes.MapRoute(
        name: "QuickSearch-index",
        url: "quicksearch/index",
        defaults: new { controller = "QuickSearch", action = "Index" });

      routes.MapRoute(
        name: "Dashboard-ManCoDropDown",
        url: "dashboard/mancodropdown",
        defaults: new { controller = "Dashboard", action = "ManCoDropDown" });

      routes.MapRoute(
        name: "Search-index", url: "search/index", defaults: new { controller = "Search", action = "Index" });

      routes.MapRoute(
        name: "Document-index", url: "document/index", defaults: new { controller = "Document", action = "Index" });

      routes.MapRoute(
        name: "Search-reset", url: "search/reset", defaults: new { controller = "Search", action = "Reset" });

      routes.MapRoute(
        name: "Search-SubDocType",
        url: "search/subdoctype/{docTypeId}",
        defaults: new { controller = "Search", action = "SubDocType", docTypeId = UrlParameter.Optional });

      routes.MapRoute(
        name: "GridRun-unapproved",
        url: "gridrun/unapproved",
        defaults: new { controller = "GridRun", action = "Unapproved" });

      routes.MapRoute(
        name: "GridRun-householding-grid",
        url: "gridrun/householding/{houseHoldingGrid}",
        defaults: new { controller = "GridRun", action = "HouseHolding", houseHoldingGrid = UrlParameter.Optional });

      routes.MapRoute(
        name: "Document-searchgrid-grid",
        url: "document/searchgrid/{grid}",
        defaults: new { controller = "Document", action = "SearchGrid", grid = UrlParameter.Optional });

      routes.MapRoute(
        name: "Environment-index",
        url: "environment/index",
        defaults: new { controller = "Environment", action = "Index" });

      routes.MapRoute(
        name: "NewsTicker-getNewsTicker",
        url: "newsticker/getnewsticker",
        defaults: new { controller = "NewsTicker", action = "GetNewsTicker" });

      routes.MapRoute(
        name: "Account-summary", url: "account/summary", defaults: new { controller = "Account", action = "Summary" });

      routes.MapRoute(
        name: "Session-new",
        url: "session/new",
        defaults: new { controller = "Session", action = "New" },
        constraints: new { httpMethod = new HttpMethodConstraint("GET") });

      routes.MapRoute(
        name: "Session-create",
        url: "session/create",
        defaults: new { controller = "Session", action = "Create" },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "Session-remove", url: "session/remove", defaults: new { controller = "Session", action = "Remove" });

      routes.MapRoute(
        name: "Session-expired", url: "session/expired", defaults: new { controller = "Session", action = "Expired" });

      routes.MapRoute(
        name: "Session-lockedout", url: "session/lockedout", defaults: new { controller = "Session", action = "LockedOut" });

      routes.MapRoute(
        name: "Session-inactive", url: "session/inactive", defaults: new { controller = "Session", action = "InActive" });

      routes.MapRoute(
        name: "Index-create",
        url: "index/create",
        defaults: new { controller = "Index", action = "Create" },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "SubDocType-create",
        url: "subdoctype/create",
        defaults: new { controller = "SubdocType", action = "Create" },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "DocType-edit",
        url: "doctype/edit",
        defaults: new { controller = "DocType", action = "Edit", },
        constraints: new { httpMethod = new HttpMethodConstraint("GET") });

      routes.MapRoute(
        name: "NewsTicker-edit",
        url: "newsticker/edit",
        defaults: new { controller = "NewsTicker", action = "Edit", },
        constraints: new { httpMethod = new HttpMethodConstraint("GET") });

      routes.MapRoute(
        name: "SubDocType-edit",
        url: "subdoctype/edit",
        defaults: new { controller = "SubDocType", action = "Edit", },
        constraints: new { httpMethod = new HttpMethodConstraint("GET") });

      routes.MapRoute(
        name: "ManCo-edit",
        url: "manco/edit",
        defaults: new { controller = "ManCo", action = "Edit", },
        constraints: new { httpMethod = new HttpMethodConstraint("GET") });

      routes.MapRoute(
        name: "Account-changepassword",
        url: "account/changepassword",
        defaults: new { controller = "Account", action = "ChangePassword" },
        constraints: new { httpMethod = new HttpMethodConstraint("GET") });

      routes.MapRoute(
        name: "Account-updatepassword",
        url: "account/updatepassword",
        defaults: new { controller = "Account", action = "UpdatePassword" },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "Application-index",
        url: "application/index",
        defaults: new { controller = "Application", action = "Index" });

      routes.MapRoute(
        name: "Index-edit",
        url: "index/edit/{indexId}",
        defaults: new { controller = "Index", action = "Edit", indexId = UrlParameter.Optional });

      routes.MapRoute(
        name: "Index-update",
        url: "index/update",
        defaults: new { controller = "Index", action = "Update", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(name: "Default-Dashboard", url: "", defaults: new { controller = "DashBoard", action = "Index" });

      routes.MapRoute(
        name: "autoApproval-index",
        url: "autoapproval/index",
        defaults: new { controller = "AutoApproval", action = "Index" });

      routes.MapRoute(
        name: "userreports-index",
        url: "userreports/index",
        defaults: new { controller = "UserReports", action = "Index" });

      routes.MapRoute(
        name: "loginreport-index",
        url: "loginreport/index",
        defaults: new { controller = "LogInReport", action = "Index" });

      routes.MapRoute(
        name: "userreports-run",
        url: "userreports/run",
        defaults: new { controller = "UserReports", action = "Run" });

      routes.MapRoute(
        name: "loginreport-run",
        url: "loginreport/run",
        defaults: new { controller = "LogInReport", action = "Run" });

      routes.MapRoute(
        name: "kpireports-index",
        url: "kpireports/index",
        defaults: new { controller = "KpiReports", action = "Index" });

      routes.MapRoute(
        name: "kpireports-run",
        url: "kpireports/run",
        defaults: new { controller = "KpiReports", action = "Run" },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "kpireports-export",
        url: "kpireports/export",
        defaults: new { controller = "KpiReports", action = "Export" },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "autoAapproval-AutoApprovals",
        url: "autoApproval/AutoApprovals/{manCoId}",
        defaults: new { controller = "AutoApproval", action = "AutoApprovals", manCoId = UrlParameter.Optional });

      routes.MapRoute(
        name: "autoApproval-create",
        url: "autoApproval/create",
        defaults: new { controller = "AutoApproval", action = "Create" });

      routes.MapRoute(
        name: "autoApproval-edit",
        url: "autoApproval/edit/{autoApprovalId}",
        defaults: new { controller = "AutoApproval", action = "Edit", autoApprovalId = UrlParameter.Optional });

      routes.MapRoute(
        name: "autoApproval-delete",
        url: "autoApproval/delete/{autoApprovalId}",
        defaults: new { controller = "AutoApproval", action = "Delete", autoApprovalId = UrlParameter.Optional });

      routes.MapRoute(
        name: "autoApproval-update",
        url: "autoApproval/update",
        defaults: new { controller = "AutoApproval", action = "Update" });

      routes.MapRoute(
        name: "DocType-index", url: "doctype/index", defaults: new { controller = "DocType", action = "Index" });

      routes.MapRoute(
        name: "SubDocType-index", url: "subdoctype/index", defaults: new { controller = "SubDocType", action = "Index" });

      routes.MapRoute(
        name: "docType-update",
        url: "doctype/update",
        defaults: new { controller = "DocType", action = "Update", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "newsTicker-update",
        url: "newsticker/update",
        defaults: new { controller = "NewsTicker", action = "Update", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });


      routes.MapRoute(
        name: "subdocType-update",
        url: "subdoctype/update",
        defaults: new { controller = "SubDocType", action = "Update", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(name: "user-index", url: "user/index", defaults: new { controller = "User", action = "Index" });

      routes.MapRoute(name: "user-create", url: "user/create", defaults: new { controller = "User", action = "Create" });

      routes.MapRoute(
        name: "user-edit", url: "user/edit/{userName}", defaults: new { controller = "User", action = "Edit" });

      routes.MapRoute(
        name: "user-delete",
        url: "user/delete/{userName}",
        defaults: new { controller = "User", action = "Delete", userName = UrlParameter.Optional });

      routes.MapRoute(
        name: "user-retrieveManCoes",
        url: "user/retrieveManCoes/{domicileId}",
        defaults: new { controller = "User", action = "RetrieveManCoes", domicileId = UrlParameter.Optional });

      routes.MapRoute(name: "manCo-index", url: "manco/index", defaults: new { controller = "ManCo", action = "Index" });

      routes.MapRoute(
        name: "manCo-update",
        url: "manco/update",
        defaults: new { controller = "ManCo", action = "Update", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "cartitem-index", url: "cartitem/index", defaults: new { controller = "CartItem", action = "Index", });

      routes.MapRoute(
        name: "cartitem-Summary",
        url: "cartitem/Summary",
        defaults: new { controller = "CartItem", action = "Summary", });

      routes.MapRoute(
        name: "cartitem-RemoveFromCart",
        url: "cartitem/RemoveFromCart",
        defaults: new { controller = "CartItem", action = "RemoveFromCart", });

      routes.MapRoute(
        name: "cartitem-ClearCart",
        url: "cartitem/ClearCart",
        defaults: new { controller = "CartItem", action = "ClearCart", });

      routes.MapRoute(
        name: "cartitem-AddToCart",
        url: "cartitem/addtocart",
        defaults: new { controller = "CartItem", action = "AddToCart", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "cartitem-AddGridToCart",
        url: "cartitem/addGridToCart",
        defaults: new { controller = "CartItem", action = "AddGridToCart", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "cartitem-Download",
        url: "cartitem/Download",
        defaults: new { controller = "CartItem", action = "Download", });

      routes.MapRoute(
        name: "cartitem-ExportDocumentsToZip",
        url: "cartitem/ExportDocumentsToZip",
        defaults: new { controller = "CartItem", action = "ExportDocumentsToZip", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "cartitem-ExportBasketToZip",
        url: "cartitem/ExportBasketToZip",
        defaults: new { controller = "CartItem", action = "ExportBasketToZip", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "approval-document",
        url: "approval/document",
        defaults: new { controller = "Approval", action = "Document", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "reject-document",
        url: "reject/document",
        defaults: new { controller = "Reject", action = "Document", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "reject-grid",
        url: "reject/grid",
        defaults: new { controller = "Reject", action = "Grid", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "reject-basket",
        url: "reject/basket",
        defaults: new { controller = "Reject", action = "Basket", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "approval-basket",
        url: "approval/basket",
        defaults: new { controller = "Approval", action = "Basket", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "approval-grid",
        url: "approval/grid",
        defaults: new { controller = "Approval", action = "Grid", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "Dashboard-Indx", url: "Dashboard/Index", defaults: new { controller = "Dashboard", action = "Index", });

      routes.MapRoute(
        name: "Password-Forgotten",
        url: "Password/Forgotten",
        defaults: new { controller = "Password", action = "Forgotten", });

      routes.MapRoute(
        name: "Password-Forgotten2",
        url: "Password/Forgotten",
        defaults: new { controller = "Password", action = "Forgotten", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "Password-Change", url: "Password/Change", defaults: new { controller = "Password", action = "Change", });

      routes.MapRoute(
        name: "Password-ChangeCurrent",
        url: "Password/ChangeCurrent",
        defaults: new { controller = "Password", action = "ChangeCurrent" });

      routes.MapRoute(
        name: "Password-Change2",
        url: "Password/Change",
        defaults: new { controller = "Password", action = "Change", },
        constraints: new { httpMethod = new HttpMethodConstraint("POST") });

      routes.MapRoute(
        name: "Password-PasswordComplexity",
        url: "Password/PasswordComplexity",
        defaults: new { controller = "Password", action = "PasswordComplexity" });

      routes.MapRoute(
        name: "Password-SecurityQuestions",
        url: "Password/SecurityQuestions/{userId}",
        defaults: new { controller = "Password", action = "SecurityQuestions", userId = UrlParameter.Optional });

      routes.MapRoute(
        name: "Security-AddAnswers",
        url: "Security/AddAnswers",
        defaults: new { controller = "Security", action = "AddAnswers" });

      routes.MapRoute(
        name: "Security-Index", url: "Security/Index", defaults: new { controller = "Security", action = "Index" });

      routes.MapRoute(
        name: "Security-AddOrUpdate",
        url: "Security/AddOrUpdate",
        defaults: new { controller = "Security", action = "AddOrUpdate" });

      routes.MapRoute(
        name: "Security-EditAnswers",
        url: "Security/EditAnswers",
        defaults: new { controller = "Security", action = "EditAnswers" });

      //routes.MapRoute(
      //          "Default",                                              // Route name
      //          "{controller}/{action}/{id}",                           // URL with parameters
      //          new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
      //      );

    }
  }
}