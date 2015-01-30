namespace ClientProxies
{
  using System.Reflection;
  using Autofac;
  using Common.Contracts;

  public class ServiceFactory : IServiceFactory
  {
    private IContainer _container;

    T IServiceFactory.CreateClient<T>()
    {
      var builder = new ContainerBuilder();

      var clientProxies = Assembly.Load("ClientProxies");
      builder.RegisterAssemblyTypes(clientProxies).AsImplementedInterfaces();

      _container = builder.Build();

      return _container.Resolve<T>();
    }
  }
}
