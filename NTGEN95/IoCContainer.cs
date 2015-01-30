namespace NTGEN95
{
  using System.Reflection;
  using Autofac;
  using Logging;
  using Logging.NLog;
  using Nexdox.Composer;

  public static class IoCContainer
  {
    private static IContainer _container;

    public static void ResoloveDependencies()
    {
      var builder = new ContainerBuilder();
      RegisterTypes(builder);
      _container = builder.Build();
    }

    public static TService Resolve<TService>()
    {
      return _container.Resolve<TService>();
    }

    private static void RegisterTypes(ContainerBuilder builder)
    {
      string ctor = Statics.UnitySQLServer;

      NexdoxMessaging.SendMessage(string.Format("Connection string : {0} ", ctor), false, null);

      var repositoryAssemblies = Assembly.Load("UnityRepository");
      builder.RegisterAssemblyTypes(repositoryAssemblies)
             .AsImplementedInterfaces()
             .WithParameter(new NamedParameter("connectionString", ctor));

      var abstractConfigurationManager = Assembly.Load("AbstractConfigurationManager");
      builder.RegisterAssemblyTypes(abstractConfigurationManager).AsImplementedInterfaces();

      var configurationManagerWrapperAssembly = Assembly.Load("ConfigurationManagerWrapper");
      builder.RegisterAssemblyTypes(configurationManagerWrapperAssembly).AsImplementedInterfaces();

      var logging = Assembly.Load("Logging");
      builder.RegisterAssemblyTypes(logging).AsImplementedInterfaces();

      var serviceAssemblies = Assembly.Load("Services");
      builder.RegisterAssemblyTypes(serviceAssemblies).AsImplementedInterfaces();

      var businessAssemblies = Assembly.Load("BusinessEngines");
      builder.RegisterAssemblyTypes(businessAssemblies).AsImplementedInterfaces();

      var commonContracts = Assembly.Load("Common.Contracts");
      builder.RegisterAssemblyTypes(commonContracts).AsImplementedInterfaces();

      builder.Register(c => new NLogLogger()).As<ILogger>().AsImplementedInterfaces();
    }
  }
}
