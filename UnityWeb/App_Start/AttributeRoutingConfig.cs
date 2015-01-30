using System.Web.Routing;
using AttributeRouting.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof(UnityWeb.AttributeRoutingConfig), "Start")]

namespace UnityWeb
{
  public static class AttributeRoutingConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.MapAttributeRoutes();
    }

    public static void Start()
    {
      RegisterRoutes(RouteTable.Routes);
    }
  }
}
