namespace Builder
{
  using Entities;

  public class HouseHoldingRunBuilder : Builder<HouseHoldingRun>
  {
    public HouseHoldingRunBuilder()
    {
      Instance = new HouseHoldingRun();
    }

    public HouseHoldingRunBuilder WithDocument(Document document)
    {
      if (document != null)
      {
        Instance.Documents.Add(document);
      }
      return this;
    }

    public HouseHoldingRunBuilder WithGridRun(GridRun gridRun)
    {
      if (gridRun != null)
      {
        Instance.GridRuns.Add(gridRun);
      }
      return this;
    }
  }
}
