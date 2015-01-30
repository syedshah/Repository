// -----------------------------------------------------------------------
// <copyright file="IoCContainer.cs" company="Hewlett-Packard Company">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace nugen99
{
  using System.Reflection;

  using Autofac;

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

      var serviceAssemblies = Assembly.Load("Services");
      builder.RegisterAssemblyTypes(serviceAssemblies).AsImplementedInterfaces();
    }
  }
}
