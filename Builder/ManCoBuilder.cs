namespace Builder
{
  using Entities;

  public class ManCoBuilder : Builder<ManCo>
  {
    public ManCoBuilder()
    {
      Instance = new ManCo();
    }

    public ManCoBuilder WithDomicile(Domicile domicile)
    {
      Instance.Domicile = domicile;
      return this;
    }
  }
}
