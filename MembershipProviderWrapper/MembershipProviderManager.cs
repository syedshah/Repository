namespace MembershipProviderWrapper
{
  using System.Web.Security;
  using AbstractMembershipProvider;

  public class MembershipProviderManager : IMembershipProvider
  {
    private readonly MembershipProvider _provider;

    public MembershipProviderManager()
      : this(null)
    {
    }

    public MembershipProviderManager(MembershipProvider provider)
    {
      _provider = Membership.Provider;
    }

    public int MinPasswordLength
    {
      get
      {
        return _provider.MinRequiredPasswordLength;
      }
    }

    public bool ValidateUser(string userName, string password)
    {
      return _provider.ValidateUser(userName, password);
    }

    public MembershipCreateStatus CreateUser(string userName, string password, string email)
    {
      MembershipCreateStatus status;
      _provider.CreateUser(userName, password, email, null, null, true, null, out status);
      return status;
    }

    public MembershipUser GetUser()
    {
      return Membership.GetUser();
    }

    public MembershipUser GetUser(string userName)
    {
      return Membership.GetUser(userName);
    }

    public MembershipUser GetUser(string userName, bool userIsOnline)
    {
      return _provider.GetUser(userName, true);
    }

    public bool ChangePassword(string usermame, string password, string newPassword)
    {
      return _provider.ChangePassword(usermame, password, newPassword);
    }
  }
}
