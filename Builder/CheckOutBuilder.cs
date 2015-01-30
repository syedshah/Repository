namespace Builder
{
  using Entities;

  public class CheckOutBuilder : Builder<CheckOut>
  {
    public CheckOutBuilder()
    {
      Instance = new CheckOut();
    }

    public CheckOutBuilder WithDocument(Document document)
    {
      Instance.Document = document;
      return this;
    }
  }
}
