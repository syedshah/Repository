namespace Builder
{
  using Entities;

  public class GridRunBuilder : Builder<GridRun>
  {
    public GridRunBuilder()
    {
      Instance = new GridRun();
    }

    public GridRunBuilder WithApplication(Application application)
    {
      Instance.Application = application;
      return this;
    }

    public GridRunBuilder WithDocument(Document document)
    {
      if (document != null)
      {
        Instance.Documents.Add(document);
      }
      return this;
    }
  }
}
