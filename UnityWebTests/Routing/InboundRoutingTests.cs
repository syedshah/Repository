namespace UnityWebTests.Routing
{
  using System.Web.Routing;
  using NUnit.Framework;
  using UnityWebTests.Extensions;

  [TestFixture]
  public class InboundRoutingTests
  {
    private void TestRoute(string url, object expectedValues, string httpMethod)
    {
      RouteData routeData = url.GetRouteData(httpMethod);

      // Assert: Test the route values against expectations
      Assert.That(routeData, Is.Not.Null);
      var routeValueDictionaryExpected = new RouteValueDictionary(expectedValues);
      foreach (var expectedRouteValue in routeValueDictionaryExpected)
      {
        if (expectedRouteValue.Value == null)
        {
          Assert.That(routeData.Values[expectedRouteValue.Key], Is.Null);
        }
        else
        {
          Assert.That(expectedRouteValue.Value.ToString(), Is.EqualTo(
              routeData.Values[expectedRouteValue.Key].ToString()).IgnoreCase);
        }
      }
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSlash_ThenIGetTheDashboardControllerIndexView()
    {
      TestRoute("~/", new
                        {
                          controller = "Dashboard", 
                          action = "Index"
                        }, 
                        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForDashboardIndex_ThenIGetTheDashboardControllerIndexView()
    {
        TestRoute("~/Dashboard/Index", new
        {
            controller = "Dashboard",
            action = "Index"
        },
                          "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForUnapprovedForGridRun_ThenIGetTheDashboardControllerUnapprovedView()
    {
      TestRoute("~/GridRun/Unapproved", new
                                {
                                  controller = "GridRun",
                                  action = "Unapproved"
                                },
                              "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForHouseHoldingForGridRun_ThenIGetTheGridRunControllerHouseHoldingView()
    {
      TestRoute("~/GridRun/HouseHolding", new
      {
        controller = "GridRun",
        action = "HouseHolding"
      },
                              "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForAdmin_ThenIGetTheEnvironmentControllerIndexView()
    {
      TestRoute("~/Admin/Index", new
      {
        controller = "Admin",
        action = "Index"
      },
       "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForEnvironment_ThenIGetTheEnvironmentControllerIndexView()
    {
      TestRoute("~/Environment/Index", new
                              {
                                controller = "Environment",
                                action = "Index"
                              },
                              "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForCartItem_ThenIGetTheCartItemControllerIndexView()
    {
      TestRoute("~/CartItem/Index", new
      {
        controller = "CartItem",
        action = "Index"
      },
                              "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForCartItemSummary_ThenIGetTheCartItemControllerSummaryView()
    {
      TestRoute("~/CartItem/Summary", new
      {
        controller = "CartItem",
        action = "Summary"
      },
                              "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForCartItemRemoveCartItem_ThenIGetTheCartItemControllerRemoveFromCartView()
    {
      TestRoute("~/CartItem/RemoveFromCart", new
      {
        controller = "CartItem",
        action = "RemoveFromCart"
      },
                              "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForCartItemClearCartItem_ThenIGetTheCartItemControllerClearCartCartView()
    {
      TestRoute("~/CartItem/ClearCart", new
      {
        controller = "CartItem",
        action = "ClearCart"
      },
                              "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForCartItemAddToCart_ThenIGetTheCartItemControllerAddToCartView()
    {
      TestRoute("~/CartItem/AddToCart", new
      {
        controller = "CartItem",
        action = "AddToCart"
      },
                              "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForCartAddGridToCart_ThenIGetTheCartItemControllerAddGridToCartiew()
    {
      TestRoute("~/CartItem/AddGridToCart", new
      {
        controller = "CartItem",
        action = "AddGridToCart"
      },
                              "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForCartItemExportBasketToZip_ThenDocumentsAreExported()
    {
      TestRoute("~/CartItem/ExportBasketToZip", new
        {
            controller = "CartItem",
            action = "ExportBasketToZip"
        },
                                "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForCartItemExportDocumentsToZip_ThenDocumentsAreExported()
    {
      TestRoute("~/CartItem/ExportDocumentsToZip", new
      {
        controller = "CartItem",
        action = "ExportDocumentsToZip"
      },
                              "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForCartItemDownload_ThenDocumentsAreDownloaded()
    {
        TestRoute("~/CartItem/Download", new
        {
            controller = "CartItem",
            action = "Download"
        },
                                "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForApprovalDocument_ThenIGetTheApprovalControllerDocumentView()
    {
      TestRoute("~/Approval/Document", new
      {
        controller = "Approval",
        action = "Document"
      },
                              "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForRejectionDocument_ThenIGetTheRejectionControllerDocumentView()
    {
      TestRoute("~/Reject/Document", new
      {
        controller = "Reject",
        action = "Document"
      },
                              "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForRejectionGrid_ThenIGetTheRejectionControllerGridView()
    {
      TestRoute("~/Reject/Grid", new
      {
        controller = "Reject",
        action = "Grid"
      },
                              "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForRejectionBasket_ThenIGetTheRejectionControllerBasketView()
    {
      TestRoute("~/Reject/Basket", new
      {
        controller = "Reject",
        action = "Basket"
      },
                              "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForApprovalBasket_ThenIGetTheApprovalControllerBaksetView()
    {
      TestRoute("~/Approval/Basket", new
      {
        controller = "Approval",
        action = "Basket"
      },
                              "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForApprovalGrid_ThenIGetTheApprovalControllerGridView()
    {
      TestRoute("~/Approval/Grid", new
      {
        controller = "Approval",
        action = "Grid"
      },
                              "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSummaryForAccount_ThenIGetTheAccountControllerSummaryView()
    {
      TestRoute("~/Account/Summary", new
      {
        controller = "Account",
        action = "Summary"
      },
                              "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForTheErrorPage_ThenIGetTheErrorIndex()
    {
      TestRoute("~/Error", new
      {
        controller = "Error",
        action = "Index"
      },
                "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSearchForGridRun_ThenIGetTheGridRunControllerSearchView()
    {
      TestRoute("~/gridrun/search/grid", new
      {
        controller = "GridRun",
        action = "Search"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSearchForBigZip_ThenIGetTheBigZipControllerSearchView()
    {
        TestRoute("~/bigzip/search/bigzip", new
        {
            controller = "BigZip",
            action = "Search"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSearchForCriteria_ThenIGetTheGridRunControllerSearchView()
    {
      TestRoute("~/gridrun/search/searchViewModel", new
      {
        controller = "GridRun",
        action = "Search"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForAOtfIndex_ThenIGetTheOtfControllerIndexView()
    {
        TestRoute("~/Otf/index", new
        {
            controller = "Otf",
            action = "Index"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForOtfEdit_ThenIGetTheOtfControllerEditView()
    {
        TestRoute("~/Otf/edit", new
        {
            controller = "Otf",
            action = "Edit"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForOtfDelete_ThenIGetTheOtfControllerDeleteView()
    {
      TestRoute("~/Otf/delete", new
      {
        controller = "Otf",
        action = "Delete"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForOtfShow_ThenIGetTheOtfControllerShowView()
    {
        TestRoute("~/Otf/show/parameterModel", new
        {
            controller = "Otf",
            action = "Show"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForOtfEditPost_ThenIGetTheOtfControllerEditPost()
    {
        TestRoute("~/Otf/edit/EditAppManCoEmailViewModel", new
        {
            controller = "Otf",
            action = "Edit"
        },
        "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForOtfCreatePost_ThenIGetTheAppManCoEmailControllerCreatePost()
    {
        TestRoute("~/Otf/create/AddAppManCoEmailViewModel", new
        {
            controller = "Otf",
            action = "Create"
        },
        "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSearchForDocument_ThenIGetTheDocumentControllerSearchView()
    {
      TestRoute("~/document/searchgrid/grid", new
      {
        controller = "Document",
        action = "SearchGrid"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForShowForDocument_ThenIGetTheDocumentControllerShowView()
    {
      TestRoute("~/document/show/documentid", new
      {
        controller = "Document",
        action = "Show"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForPdfContainerForDocument_ThenIGetTheDocumentControllerPdfContainerView()
    {
      TestRoute("~/document/pdfcontainer/documentid", new
      {
        controller = "Document",
        action = "PdfContainer"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForStatusForDocument_ThenIGetTheDocumentControllerStatusView()
    {
      TestRoute("~/document/status/documentid", new
      {
        controller = "Document",
        action = "Status"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForStatusForGridRun_ThenIGetTheGridRunControllerStatusView()
    {
      TestRoute("~/gridrun/status/documentid", new
      {
        controller = "GridRun",
        action = "Status"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForQuickSearch_ThenIGetTheQuickSearchIndexView()
    {
      TestRoute("~/quicksearch/index", new
      {
        controller = "QuickSearch",
        action = "Index"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForSearch_ThenIGetTheSearchIndexView()
    {
      TestRoute("~/search/index", new
      {
        controller = "Search",
        action = "Index"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForResetForSearch_ThenIGetTheSearchResetView()
    {
      TestRoute("~/search/reset", new
      {
        controller = "Search",
        action = "Reset"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskFoIndexForDocument_ThenIGetTheDocumentIndexView()
    {
      TestRoute("~/document/index", new
      {
        controller = "Document",
        action = "Index"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForApplicaiton_ThenIGetTheApplicationControllerIndexView()
    {
      TestRoute("~/application/index", new
      {
        controller = "Application",
        action = "Index"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForEditForIndex_ThenIGetTheIndexControllerEditView()
    {
      TestRoute("~/index/edit/1", new
      {
        controller = "Index",
        action = "Edit"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForEditForUser_ThenIGetTheUserControllerEditView()
    {
        TestRoute("~/user/edit/1", new
        {
            controller = "User",
            action = "Edit"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForDeleteForUser_ThenIGetTheUserControllerDeleteFunction()
    {
        TestRoute("~/user/delete/1", new
        {
            controller = "User",
            action = "Delete"
        },
        "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForUser_ThenIGetTheUserControllerIndexView()
    {
        TestRoute("~/user/index", new
        {
            controller = "User",
            action = "Index"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForDomicileManCoes_ThenIGetTheUserControllerUserManCoPartialView()
    {
        TestRoute("~/user/RetrieveManCoes/2", new
        {
            controller = "User",
            action = "RetrieveManCoes",
            domicileId = "2"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSubDocTypeForSearch_ThenIGetTheSearchControllerSubDocTypeView()
    {
      TestRoute("~/search/subdoctype/1", new
      {
        controller = "Search",
        action = "SubDocType"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForFileForQuickSearch_ThenIGetTheQuickSearchFileView()
    {
      TestRoute("~/quicksearch/file", new
      {
        controller = "QuickSearch",
        action = "File"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForGRIDQuickSearch_ThenIGetTheQuickSearchGridView()
    {
      TestRoute("~/quicksearch/grid", new
      {
        controller = "QuickSearch",
        action = "Grid"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForBigZipQuickSearch_ThenIGetTheQuickSearchBigZipView()
    {
        TestRoute("~/quicksearch/bigzip", new
        {
            controller = "QuickSearch",
            action = "BigZip"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForToLogin_ThenIGetTheNewActionForSession()
    {
      TestRoute("~/session/new", new
      {
        controller = "Session",
        action = "New",
      },
                "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenILogin_ThenIGetTheCreateActionForTheSession()
    {
      TestRoute("~/session/create", new
      {
        controller = "Session",
        action = "Create",
      },
                "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIUpdateAnIndex_ThenIGetTheUpdateActionForIndexes()
    {
      TestRoute("~/index/update", new
      {
        controller = "Index",
        action = "Update",
      },
                "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenILogOut_ThenIGetTheRemoveActionForTheSession()
    {
      TestRoute("~/session/remove", new
      {
        controller = "Session",
        action = "Remove",
      },
                "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenMySessionExpires_ThenIGetTheExpiredActionForTheSession()
    {
      TestRoute("~/session/expired", new
      {
        controller = "Session",
        action = "Expired",
      },
                "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAmLockedOut_ThenIGetTheLockedOutActionForTheSession()
    {
      TestRoute("~/session/lockedout", new
      {
        controller = "Session",
        action = "LockedOut",
      },
                "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAmNotApproved_ThenIGetTheInactiveActionForTheSession()
    {
      TestRoute("~/session/inactive", new
      {
        controller = "Session",
        action = "InActive",
      },
                "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForChangePassword_ThenIGetTheChangePasswordActionForAccount()
    {
      TestRoute("~/account/changepassword", new
      {
        controller = "Account",
        action = "ChangePassword",
      },
                "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForUpdatePassword_ThenIGetTheChangePasswordActionForAccount()
    {
      TestRoute("~/account/updatepassword", new
      {
        controller = "Account",
        action = "UpdatePassword",
      },
                "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForDocType_ThenIGetTheDocTypeControllerIndexView()
    {
      TestRoute("~/doctype/index", new
      {
        controller = "DocType",
        action = "Index"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForSubDocType_ThenIGetTheSubDocTypeControllerIndexView()
    {
      TestRoute("~/subdoctype/index", new
      {
        controller = "SubDocType",
        action = "Index"
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAsktoEditDocType_ThenIGetTheEditActionForDocType()
    {
      TestRoute("~/doctype/edit", new
      {
        controller = "DocType",
        action = "Edit",
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAsktoEditSubDocType_ThenIGetTheEditActionForDocType()
    {
      TestRoute("~/subdoctype/edit", new
      {
        controller = "SubDocType",
        action = "Edit",
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIUpdateADocType_ThenIGetTheUpdateActionForDocTypes()
    {
      TestRoute("~/doctype/update", new
      {
        controller = "DocType",
        action = "Update"
      },
      "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIUpdateASubDocType_ThenIGetTheUpdateActionForSubDocTypes()
    {
      TestRoute("~/subdoctype/update", new
      {
        controller = "SubDocType",
        action = "Update"
      },
      "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForManCo_ThenIGetTheManCoControllerIndexView()
    {
      TestRoute("~/manco/index", new
      {
        controller = "ManCo",
        action = "Index"
      },
      "GET");
    }

    public void GivenACorrectRoutesCollection_WhenIAsktoEditManCo_ThenIGetTheEditActionForManCo()
    {
      TestRoute("~/manco/edit", new
      {
        controller = "ManCo",
        action = "Edit",
      },
      "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIUpdateAManCo_ThenIGetTheUpdateActionForManCo()
    {
      TestRoute("~/manco/update", new
      {
        controller = "ManCo",
        action = "Update"
      },
      "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForDocumentApproval_ThenIGetTheDocumentApprovalControllerIndexView()
    {
      TestRoute("~/autoapproval/index", new
        {
          controller = "AutoApproval",
            action = "Index"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForUserReports_ThenIGetTheUserReportsControllerIndexView()
    {
      TestRoute("~/userreports/index", new
      {
        controller = "UserReports",
        action = "Index"
      },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForLogInReport_ThenIGetTheLogInReportControllerIndexView()
    {
      TestRoute("~/loginreport/index", new
      {
        controller = "LogInReport",
        action = "Index"
      },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForRunForUserReports_ThenIGetTheUserReportsControllerIndexView()
    {
      TestRoute("~/userreports/run", new
      {
        controller = "UserReports",
        action = "Run"
      },
        "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForRunForLogInReport_ThenIGetTheLogInReportControllerIndexView()
    {
      TestRoute("~/loginreport/run", new
      {
        controller = "LogInReport",
        action = "Run"
      },
        "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForIndexForKpiReports_ThenIGetTheKpiReportsControllerIndexView()
    {
      TestRoute("~/kpireports/index", new
      {
        controller = "KpiReports",
        action = "Index"
      },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForRunForKpiReports_ThenIGetTheKpiReportsControllerRunView()
    {
      TestRoute("~/kpireports/run", new
      {
        controller = "KpiReports",
        action = "Run"
      },
        "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForExportForKpiReports_ThenIGetTheKpiReportsControllerExportView()
    {
      TestRoute("~/kpireports/export", new
      {
        controller = "KpiReports",
        action = "Export"
      },
        "POST");
    }

   [Test]
    public void GivenACorrectRoutesCollection_WhenIAsktoEditDocumentApproval_ThenIGetTheEditActionForDocumentApproval()
    {
        TestRoute("~/autoApproval/edit", new
        {
            controller = "AutoApproval",
            action = "Edit",
        },
        "GET");
    }

   [Test]
   public void GivenACorrectRoutesCollection_WhenIAsktoDeleteDocumentApproval_ThenIGetTheDeleteActionForDocumentApproval()
   {
     TestRoute("~/autoApproval/delete", new
     {
       controller = "AutoApproval",
       action = "Delete",
     },
     "GET");
   }

   [Test]
   public void GivenACorrectRoutesCollection_WhenIAsktoApprovalAdminDocumentApprovals_ThenIGetTheDocumentApprovalsActionForApprovalAdmin()
   {
       TestRoute("~/autoapproval/autoapprovals", new
       {
           controller = "AutoApproval",
           action = "AutoApprovals",
       },
       "GET");
   }

   [Test]
   public void GivenACorrectRoutesCollection_WhenIAskForForgottenPasswordPage_ThenIGetTheForgottenPasswordPage()
   {
       TestRoute("~/password/forgotten", new
       {
           controller = "Password",
           action = "Forgotten",
       },
       "GET");
   }

   [Test]
   public void GivenACorrectRoutesCollection_WhenIAskForChangeCurrentPasswordPage_ThenIGetTheChangeCurrentPasswordPage()
   {
       TestRoute("~/password/changecurrent", new
       {
           controller = "Password",
           action = "ChangeCurrent",
       },
       "GET");
   }

   [Test]
   public void GivenACorrectRoutesCollection_WhenISubmitTheForgottenPasswordPage_ThenIGetTheSubmitActionForgottenPasswordPage()
   {
       TestRoute("~/password/forgotten", new
       {
           controller = "Password",
           action = "Forgotten",
       },
       "POST");
   }

   [Test]
   public void GivenACorrectRoutesCollection_WhenIAskForChangePasswordPage_ThenIGetTheChangePasswordPage()
   {
       TestRoute("~/password/change", new
       {
           controller = "Password",
           action = "Change",
       },
       "GET");
   }

   [Test]
   public void GivenACorrectRoutesCollection_WhenISubmitTheChangePasswordPage_ThenIGetTheSubmitActionChangePasswordPage()
   {
       TestRoute("~/password/change", new
       {
           controller = "Password",
           action = "Change",
       },
       "POST");
   }

    [Test]
   public void GivenACorrectRoutesCollection_WhenIUpdateApprovalAdminController_ThenIGetTheUpdateActionForDocumentApproval()
    {
      TestRoute("~/autoapproval/update", new
        {
          controller = "AutoApproval",
            action = "Update"
        },
        "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenICreateApprovalAdminController_ThenIGetTheCreateActionForDocumentApproval()
    {
      TestRoute("~/autoapproval/create", new
        {
          controller = "AutoApproval",
            action = "Create"
        },
        "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForPasswordSecurityQuestions_ThenIGetThePasswordControllerSecurityQuestions()
    {
        TestRoute("~/password/securityquestions/fryer", new
        {
            controller = "Password",
            action = "SecurityQuestions"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenISubmitPasswordSecurityQuestions_ThenThePasswordSecurityQuestionsIsPosted()
    {
        TestRoute("~/password/securityquestions", new
        {
            controller = "Password",
            action = "SecurityQuestions"
        },
        "POST");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForPasswordComplexity_ThenThePasswordComplexityPartialViewIsShown()
    {
        TestRoute("~/password/PasswordComplexity", new
        {
            controller = "Password",
            action = "PasswordComplexity"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSecurityIndex_ThenIGetTheSecurityControllerIndex()
    {
        TestRoute("~/security/index", new
        {
            controller = "Security",
            action = "Index"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSecurityAddAnswers_ThenIGetTheSecurityControllerAddAnswers()
    {
        TestRoute("~/security/addanswers", new
        {
            controller = "Security",
            action = "AddAnswers"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSecurityAddOrUpdateAnswers_ThenIGetTheSecurityControllerAddOrUpdateAnswers()
    {
        TestRoute("~/security/addorupdate", new
        {
            controller = "Security",
            action = "AddOrUpdate"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIAskForSecurityEditAnswers_ThenIGetTheSecurityControllerEditAnswers()
    {
        TestRoute("~/security/editanswers", new
        {
            controller = "Security",
            action = "EditAnswers"
        },
        "GET");
    }

    [Test]
    public void GivenACorrectRoutesCollection_WhenIWantToSubmitSecurityAddAnswers_ThenIGetTheSecurityAddAnswersIsSubmitted()
    {
        TestRoute("~/security/addanswers", new
        {
            controller = "Security",
            action = "AddAnswers"
        },
        "POST");
    }
  }
}
