namespace UnityWebTests.HtmlHelpers
{
  using System.Web.Mvc;
  using FluentAssertions;
  using UnityWeb.HtmlHelpers;
  using NUnit.Framework;

  [TestFixture]
  public class DisplayActionImageTests
  {
    HtmlHelper _helper = null;

    [Test]
    public void GivenValidData_WhenIUseTheDisplayACtionImageHelper_IGetValidHtml()
    {
      MvcHtmlString result = _helper.ActionImage("/Controller/Action?DocumentId=Id", "~/Images/pdf.gif", "alt", "css");

      result.ToString()
            .Should()
            .Be(
              @"<a class=""css"" href=""/Controller/Action?DocumentId=Id""><img alt=""alt"" src=""~/Images/pdf.gif"" /></a>");
      
    }}
}
