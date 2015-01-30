namespace Builder
{
  using Entities;

  public class PasswordHistoryBuilder : Builder<PasswordHistory>
  {
    public PasswordHistoryBuilder()
    {
      Instance = new PasswordHistory();    
    }
  }
}
