namespace ArchiveServiceFactory.Factories
{
  using System;
  using System.ServiceModel;
  using AbstractConfigurationManager;
  using ArchiveServiceFactory.ArchiveService;
  using ArchiveServiceFactory.Interfaces;
  using Exceptions;

  public class DocumentServiceFactory : IDocumentServiceFactory
  {
    private readonly IConfigurationManager _configurationManager;

    public DocumentServiceFactory(IConfigurationManager configurationManager)
    {
      _configurationManager = configurationManager;
    }

    public IDocumentChannel CreateChannel()
    {
      try
      {
        var binding = new BasicHttpBinding() { MaxReceivedMessageSize = 2147483647 };
        var endPoint = new EndpointAddress(_configurationManager.AppSetting("documentService"));
        return new ChannelFactory<IDocumentChannel>(binding, endPoint).CreateChannel();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to open channel", e);
      }
    }
  }
}
