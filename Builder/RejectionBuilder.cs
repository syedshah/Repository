namespace Builder
{
  using Entities;

  public class RejectionBuilder : Builder<Rejection>
  {
    public RejectionBuilder()
    {
      Instance = new Rejection();
    }

    public RejectionBuilder WithDocument(Document document)
    {
      Instance.Document = document;
      return this;
    }
  }
}
