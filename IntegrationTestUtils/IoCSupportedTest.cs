namespace IntegrationTestUtils
{
  using System.Reflection;

  using Autofac;

  public class IoCSupportedTest
  {
    private IContainer _container;

    public IoCSupportedTest()
    {
      var builder = new ContainerBuilder();

     /* var oneStepServiceFactory = Assembly.Load("OneStepServiceFactory");
      builder.RegisterAssemblyTypes(oneStepServiceFactory).AsImplementedInterfaces();

      var onestepServiceFactory = Assembly.Load("ArchiveServiceFactory");
      builder.RegisterAssemblyTypes(onestepServiceFactory).AsImplementedInterfaces();

      var onfigurationManagerWrapper = Assembly.Load("ConfigurationManagerWrapper");
      builder.RegisterAssemblyTypes(onfigurationManagerWrapper).AsImplementedInterfaces();*/

      var clientProxies = Assembly.Load("ClientProxies");
      builder.RegisterAssemblyTypes(clientProxies).AsImplementedInterfaces();

      _container = builder.Build();
    }

    protected TEntity Resolve<TEntity>()
    {
      return _container.Resolve<TEntity>();
    }

    protected void ShutdownIoC()
    {
      _container.Dispose();
    }
  }
}
