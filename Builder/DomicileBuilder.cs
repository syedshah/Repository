namespace Builder
{
  using Entities;

  public class DomicileBuilder : Builder<Domicile>
  {
    public DomicileBuilder()
    {
      Instance = new Domicile();
    }
  }
}
