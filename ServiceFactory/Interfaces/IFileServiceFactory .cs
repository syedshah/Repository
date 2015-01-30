namespace ServiceFactory.Interfaces
{
  using OneStepServiceFactory.OneStepService;

  public interface IFileServiceFactory 
  {
    IFileServiceChannel CreateChannel();
  }
}
