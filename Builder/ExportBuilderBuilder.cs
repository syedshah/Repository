namespace Builder
{
  using Entities;

  public class ExportBuilder : Builder<Export>
  {
    public ExportBuilder()
    {
      Instance = new Export();
    }

    public ExportBuilder WithDocument(Document document)
    {
      Instance.Document = document;
      return this;
    }
  }
}
