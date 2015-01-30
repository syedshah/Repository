namespace UnityWebTests.HtmlHelpers
{
  using System;
  using System.Web.Mvc;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.HtmlHelpers;
  using UnityWeb.Models.Helper;

  [TestFixture]
  public class HelperTests
  {
    [SetUp]
    public void SetUp()
    {
      _pagingInfo = new PagingInfoViewModel
    {
      CurrentPage = 2,
      TotalItems = 28,
      ItemsPerPage = 10,
      TotalPages = (int)Math.Ceiling((decimal)28 / 10)
    };
    }

    private readonly HtmlHelper _helper = null;
    private PagingInfoViewModel _pagingInfo; 
    Func<int, string> _pageUrlDelegate = i => "Page" + i;

    [Test]
    public void WhenUsingPageLinks_WhenPagingInfoIsValid_IGetAValidPageLink()
    {
      MvcHtmlString result = _helper.PageLinks(_pagingInfo, _pageUrlDelegate);

      result.ToString().Should().Be(@"<a href=""Page1"">1</a><a class=""selected"" href=""Page2"">2</a><a href=""Page3"">3</a>");
    }
  }
}
