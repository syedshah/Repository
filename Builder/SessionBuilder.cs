namespace Builder
{
  using Entities;

  public class SessionBuilder : Builder<Session>
  {
    public SessionBuilder()
    {
      Instance = new Session();
    }

    public SessionBuilder WithUser(ApplicationUser user)
    {
      Instance.ApplicationUser = user;
      return this;
    }
  }
}
