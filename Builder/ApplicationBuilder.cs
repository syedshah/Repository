namespace Builder
{
  using Entities;

  public class ApplicationBuilder : Builder<Application>
  {
    public ApplicationBuilder()
    {
      Instance = new Application();
    }
  }
}