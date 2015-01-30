namespace Common.Contracts
{
  public interface IServiceFactory
  {
    T CreateClient<T>() where T : IServiceContract;
  }
}
