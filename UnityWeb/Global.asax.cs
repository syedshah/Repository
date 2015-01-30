namespace UnityWeb
{
  using System.Configuration;
  using System.Reflection;
  using System.Web;
  using System.Web.Http;
  using System.Web.Mvc;
  using System.Web.Optimization;
  using System.Web.Routing;
  using Autofac;
  using Autofac.Integration.Mvc;
  using IdentityWrapper.Identity;
  using IdentityWrapper.Interfaces;
  using Logging;
  using Logging.NLog;
  using ServiceInterfaces;
  using Services;
  using UnityWeb.App_Start;

  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      var builder = new ContainerBuilder();
      RegisterTypes(builder);
      var container = builder.Build();

      DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

      AreaRegistration.RegisterAllAreas();

      WebApiConfig.Register(GlobalConfiguration.Configuration);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
      AuthConfig.RegisterAuth();

      //DatabaseConfig.Initialize();

      IdentityConfig.Initialize();
    }

    private static void RegisterTypes(ContainerBuilder builder)
    {
      string baseDirectory = HttpContext.Current.Server.MapPath("~/App_Data") + ConfigurationManager.AppSettings["dataFolderName"];

      builder.RegisterControllers(typeof(MvcApplication).Assembly);

      var businessAssemblies = Assembly.Load("BusinessEngines");
      builder.RegisterAssemblyTypes(businessAssemblies).AsImplementedInterfaces();

      var serviceAssemblies = Assembly.Load("Services");
      builder.RegisterAssemblyTypes(serviceAssemblies).AsImplementedInterfaces();

      var configurationManagerWrapperAssembly = Assembly.Load("ConfigurationManagerWrapper");
      builder.RegisterAssemblyTypes(configurationManagerWrapperAssembly).AsImplementedInterfaces();

      var clientProxiesAssembly = Assembly.Load("ClientProxies");
      builder.RegisterAssemblyTypes(clientProxiesAssembly).AsImplementedInterfaces();

      var zipManagerWrapper = Assembly.Load("ZipManagerWrapper");
      builder.RegisterAssemblyTypes(zipManagerWrapper).AsImplementedInterfaces();

      string ctor = ConfigurationManager.ConnectionStrings["unity"].ConnectionString;
      var repositoryAssemblies = Assembly.Load("UnityRepository");
      builder.RegisterAssemblyTypes(repositoryAssemblies).AsImplementedInterfaces().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerHttpRequest();

      var identityAssembly = Assembly.Load("IdentityWrapper");
      builder.RegisterAssemblyTypes(identityAssembly).AsImplementedInterfaces()
          .WithParameter(new NamedParameter("connectionString", ctor))
          .WithParameter(new NamedParameter("context", new HttpContextWrapper(HttpContext.Current)))
          .InstancePerHttpRequest();

      builder.RegisterType<FileRepository.Repositories.ExportFileRepository>()
             .As<FileRepository.Interfaces.IExportFileRepository>()
             .WithParameter(new NamedParameter("path", baseDirectory));

      builder.RegisterType<FileRepository.Repositories.HouseHoldingFileRepository>()
             .As<FileRepository.Interfaces.IHouseHoldingFileRepository>()
             .WithParameter(new NamedParameter("path", baseDirectory));

      builder.RegisterType<BusinessEngines.ExportEngine>()
             .As<BusinessEngineInterfaces.IExportEngine>()
             .WithParameter(new NamedParameter("path", baseDirectory));

      var fileAssembly = Assembly.Load("SystemFileAdapter");
      builder.RegisterAssemblyTypes(fileAssembly).AsImplementedInterfaces();

      builder.Register(c => new NLogLogger()).As<ILogger>().InstancePerHttpRequest();

      builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();

      builder.RegisterType<UserManagerProvider>().As<IUserManagerProvider>().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerDependency();

      builder.RegisterType<PasswordHistoryService>().As<IPasswordHistoryService>().InstancePerDependency();

      builder.RegisterFilterProvider();
    }
  }
}