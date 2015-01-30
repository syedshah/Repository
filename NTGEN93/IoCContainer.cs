namespace NTGEN93
{
  using System.Reflection;
  using Autofac;
  using Nexdox.Composer;

  public static class IoCContainer
  {
    private static IContainer _container;

    public static void ResoloveDependencies(ApplicationInfo appInfo)
    {
      var builder = new ContainerBuilder();
      RegisterTypes(builder, appInfo);
      _container = builder.Build();
    }

    public static TService Resolve<TService>()
    {
      return _container.Resolve<TService>();
    }

    private static void RegisterTypes(ContainerBuilder builder, ApplicationInfo appInfo)
    {
      string ctor = Statics.UnitySQLServer;
      string baseDirectory = appInfo.InputPath;

      NexdoxMessaging.SendMessage(string.Format("Connection string : {0} ", ctor), false, null);

      var repositoryAssemblies = Assembly.Load("UnityRepository");
      builder.RegisterAssemblyTypes(repositoryAssemblies)
             .AsImplementedInterfaces()
             .WithParameter(new NamedParameter("connectionString", ctor));

      var serviceAssemblies = Assembly.Load("Services");
      builder.RegisterAssemblyTypes(serviceAssemblies).AsImplementedInterfaces();

      var businessAssemblies = Assembly.Load("BusinessEngines");
      builder.RegisterAssemblyTypes(businessAssemblies).AsImplementedInterfaces();

      var fileAssembly = Assembly.Load("SystemFileAdapter");
      builder.RegisterAssemblyTypes(fileAssembly).AsImplementedInterfaces();

      builder.RegisterType<FileRepository.Repositories.HouseHoldingFileRepository>()
             .As<FileRepository.Interfaces.IHouseHoldingFileRepository>()
             .WithParameter(new NamedParameter("path", baseDirectory));
    }
  }
}
