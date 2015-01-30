namespace UnityWebTests.HtmlHelpers
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Security.Claims;
  using System.Security.Principal;
  using System.Web;
  using System.Web.Mvc;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.HtmlHelpers;
  using UnityWeb.Models.Helper;

  [TestFixture]
  public class DivHelperTests
  {
    [SetUp]
    public void SetUp()
    {
      ContextSetUp();
    }

    private readonly HtmlHelper _helper = null;

    [Test]
    public void WhenUsingHelperDiv_WithNoParameter_IGetAValidDiv()
    {
      MvcHtmlString result1 = _helper.BeginDiv();
      MvcHtmlString result2 = _helper.EndDiv();

      (result1.ToString() + result2.ToString()).Should().Be(@"<div></div>");

    }

    [Test]
    public void WhenUsingHelperDiv_WithAnAdminRoleAsAdminParameter_AndUserIsGovernorRole_IGetAValidDivWithHiddenStyle()
    {
        MvcHtmlString result1 = _helper.BeginDiv("Admin");
        MvcHtmlString result2 = _helper.EndDiv();

        (result1.ToString() + result2.ToString()).Should().Be(@"<div style='display: none'></div>");

    }


    [Test]
    public void WhenUsingHelperDiv_WithAnAdminRoleAsGovernorParameter_AndUserIsGovernorRole_IGetAValidDivWithNoHiddenStyle()
    {
        MvcHtmlString result1 = _helper.BeginDiv("Governor");
        MvcHtmlString result2 = _helper.EndDiv();

        (result1.ToString() + result2.ToString()).Should().Be(@"<div></div>");

    }

    [Test]
    public void WhenUsingHelperDiv_WithParametersRoleAsAdminCssClassAndId_AndUserIsGovernorRole_IGetAValidDivWithHiddenStyle()
    {
        MvcHtmlString result1 = _helper.BeginDiv("Admin","Great", "divGreat");
        MvcHtmlString result2 = _helper.EndDiv();

        (result1.ToString() + result2.ToString()).Should().Be(@"<div class='Great' id='divGreat' style='display: none'></div>");

    }

    [Test]
    public void WhenUsingHelperDiv_WithParametersRoleAsGovernorCssClassAndId_AndUserIsGovernorRole_IGetAValidDivWithNoHiddenStyle()
    {
        MvcHtmlString result1 = _helper.BeginDiv("Governor", "Great", "divGreat");
        MvcHtmlString result2 = _helper.EndDiv();

        (result1.ToString() + result2.ToString()).Should().Be(@"<div class='Great' id='divGreat'></div>");

    }

    [Test]
    public void WhenUsingHelperDiv_WithParametersRoleAsAdminAndDictionaryOfAttributes_AndUserIsGovernorRole_IGetAValidDivWithHiddenStyle()
    {
        var attributes = new Dictionary<string, object>
                             {
                                {"class", "theOne" } ,
                                {"id", "dOne" }
                             };

        MvcHtmlString result1 = _helper.BeginDiv("Admin", attributes);
        MvcHtmlString result2 = _helper.EndDiv();

        (result1.ToString() + result2.ToString()).Should().Be(@"<div class='theOne' id='dOne' style='display: none'></div>");

    }

    [Test]
    public void WhenUsingHelperDiv_WithParametersRoleAsGovernorAndDictionaryOfAttributes_AndUserIsGovernorRole_IGetAValidDivWithNoHiddenStyle()
    {
        var attributes = new Dictionary<string, object>
                             {
                                {"class", "theOne" } ,
                                {"id", "dOne" }
                             };

        MvcHtmlString result1 = _helper.BeginDiv("Governor", attributes);
        MvcHtmlString result2 = _helper.EndDiv();

        (result1.ToString() + result2.ToString()).Should().Be(@"<div class='theOne' id='dOne'></div>");

    }

    [Test]
    public void WhenUsingHelperDiv_WithParameterDictionaryOfAttributes_AndUserIsGovernorRole_IGetAValidDivNotHidden()
    {
        var attributes = new Dictionary<string, object>
                             {
                                {"class", "theOne" } ,
                                {"id", "dOne" }
                             };

        MvcHtmlString result1 = _helper.BeginDiv(attributes);
        MvcHtmlString result2 = _helper.EndDiv();

        (result1.ToString() + result2.ToString()).Should().Be(@"<div class='theOne' id='dOne'></div>");

    }

    private void ContextSetUp()
    {
        HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""),new HttpResponse(new StringWriter()));

        var listClaims = new List<Claim>();

        listClaims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "eea3b620-f64a-4c52-8ede-7af2b23c9960"));
        listClaims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "username"));
        listClaims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
        listClaims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Governor"));

        var identity = new ClaimsIdentity(listClaims, "ApplicationCookie", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

        HttpContext.Current.User = new System.Security.Claims.ClaimsPrincipal(identity);
    }
  }
}
