namespace Builder
{
  using Entities;

  public class HouseHoldBuilder : Builder<HouseHold>
  {
    public HouseHoldBuilder()
    {
      Instance = new HouseHold();
    }

    public HouseHoldBuilder WithDocument(Document document)
    {
      Instance.Document = document;
      return this;
    }
  }
}
