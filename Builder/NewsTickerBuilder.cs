namespace Builder
{
  using Entities;

  public class NewsTickerBuilder : Builder<NewsTicker>
  {
    public NewsTickerBuilder()
    {
      Instance = new NewsTicker();
    }
  }
}
