namespace ServiceFactory.Factories
{
  using System;
  using System.ServiceModel;
  using AbstractConfigurationManager;
  using Exceptions;
  using OneStepServiceFactory.OneStepService;
  using ServiceFactory.Interfaces;

  public class FileServiceFactory : IFileServiceFactory
  {
    private readonly IConfigurationManager _configurationManager;

    public FileServiceFactory(IConfigurationManager configurationManager)
    {
      _configurationManager = configurationManager;
    }

    public IFileServiceChannel CreateChannel()
    {
      try
      {
        var binding = new BasicHttpBinding() { MaxReceivedMessageSize = 2147483647 };
        var endPoint = new EndpointAddress(_configurationManager.AppSetting("fileService"));
        return new ChannelFactory<IFileServiceChannel>(binding, endPoint).CreateChannel();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to open channel", e);
      }
    }
  }
}
