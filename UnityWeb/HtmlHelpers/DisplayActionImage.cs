namespace UnityWeb.HtmlHelpers
{
  using System.Web.Mvc;

  public static class DisplayActionImage
  {

    public static MvcHtmlString ActionImage(this HtmlHelper html, string href, string imagePath, string alt = null, string cssClass = null)
    {

      // build the <img> tag
      var imgBuilder = new TagBuilder("img");
      imgBuilder.MergeAttribute("src", imagePath);
      if (alt != null)
        imgBuilder.MergeAttribute("alt", alt);

      string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

      // build the <a> tag
      var anchorBuilder = new TagBuilder("a");

      anchorBuilder.MergeAttribute("href", href);
      if (cssClass != null)
        anchorBuilder.MergeAttribute("class", cssClass);

      anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
      string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

      return MvcHtmlString.Create(anchorHtml);
    }
  }
}