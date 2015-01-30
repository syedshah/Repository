namespace UnityWeb.HtmlHelpers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Security.Principal;
  using System.Text;
  using System.Web;
  using System.Web.Mvc;

  public static class DivHelper 
  {

    public static MvcHtmlString BeginDiv(this HtmlHelper html)
    {
       // build the <div> tag
       var sb = new StringBuilder();
       sb.Append("<div>");

       return MvcHtmlString.Create(sb.ToString());
    }

    public static MvcHtmlString BeginDiv(this HtmlHelper html, string roles)
    {
        // build the <div> tag
        var sb = new StringBuilder();
        sb.Append("<div");

        sb = CheckInRoles(SplitString(roles), HttpContext.Current.User) ? sb : HideDiv(sb);

        sb.Append(">");

        var resultHtml = MvcHtmlString.Create(sb.ToString());

        return resultHtml;
    }

    public static MvcHtmlString BeginDiv(this HtmlHelper html, string roles, string cssClass, string id)
    {
        // build the <div> tag
        var sb = new StringBuilder();
        sb.AppendFormat("<div class='{0}' id='{1}'", cssClass, id);

        sb = CheckInRoles(SplitString(roles), HttpContext.Current.User) ? sb  : HideDiv(sb);

        sb.Append(">");

        var resultHtml = MvcHtmlString.Create(sb.ToString());

        return resultHtml;
    }

    public static MvcHtmlString BeginDiv(this HtmlHelper html, string roles, IDictionary<string, object> htmlAttributes)
    {
        // build the <div> tag
        var sb = new StringBuilder();
        sb.Append("<div");

        foreach (var htmlAttribute in htmlAttributes)
        {
            sb.AppendFormat(" " + htmlAttribute.Key.Trim() + "='{0}'", htmlAttribute.Value.ToString().Trim());
        }

        sb = CheckInRoles(SplitString(roles), HttpContext.Current.User) ? sb : HideDiv(sb);

        sb.Append(">");

        return MvcHtmlString.Create(sb.ToString());
    }

    public static MvcHtmlString BeginDiv(this HtmlHelper html, IDictionary<string, object> htmlAttributes)
    {
        // build the <div> tag
        var sb = new StringBuilder();
        sb.Append("<div");

        foreach (var htmlAttribute in htmlAttributes)
        {
            sb.AppendFormat(" " + htmlAttribute.Key.Trim() + "='{0}'", htmlAttribute.Value.ToString().Trim());
        }

        sb.Append(">");

        return MvcHtmlString.Create(sb.ToString());
    }
 
    public static MvcHtmlString EndDiv(this HtmlHelper html)
    {
        var sb = new StringBuilder();
        sb.Append("</div>");

        var resultHtml = MvcHtmlString.Create(sb.ToString());

        return resultHtml;
    }

    private static StringBuilder HideDiv(StringBuilder sb)
    {
        var noDisplay = "display: none";

        sb.AppendFormat(" style='{0}'", noDisplay);

        return sb;
    }

    private static IList<string> SplitString(string original)
    {
        if (String.IsNullOrEmpty(original))
        {
            return new string[0];
        }

        return original.Split(',').ToList();
    }

    private static bool CheckInRoles(IList<string> roles, IPrincipal user)
    {
        if (roles.Count < 1)
        {
            return true;
        }
        else if (roles.Any(c => user.IsInRole(c)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
  }
}