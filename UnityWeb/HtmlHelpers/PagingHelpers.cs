namespace UnityWeb.HtmlHelpers
{
  using System;
  using System.Text;
  using System.Web.Mvc;

  using UnityWeb.Models.Helper;

  public static class PagingHelpers
  {
    public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfoViewModel pagingInfo,
      Func<int, string> pageUrl, string classValue = "")
    {
      StringBuilder result = new StringBuilder();
      for (int i = 1; i <= pagingInfo.TotalPages; i++)
      {
        TagBuilder tag = new TagBuilder("a");
        tag.MergeAttribute("href", pageUrl(i));
        tag.InnerHtml = i.ToString();
        if (i == pagingInfo.CurrentPage)
          tag.AddCssClass("selected");

        if (!string.IsNullOrEmpty(classValue))
          tag.AddCssClass(classValue);

        result.Append(tag.ToString());
      }
      return MvcHtmlString.Create(result.ToString());
    }
  }
}