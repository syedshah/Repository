namespace Builder
{
  using Entities;

  public class IndexBuilder : Builder<IndexDefinition>
  {
    public IndexBuilder()
    {
      Instance = new IndexDefinition();
    }

    public IndexBuilder WithApplication(Application application)
    {
      Instance.Application = application;
      return this;
    }
  }
}