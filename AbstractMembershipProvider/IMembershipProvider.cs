namespace AbstractMembershipProvider
{
  using System.Web.Security;

  public interface IMembershipProvider
  {
    int MinPasswordLength { get; }

    bool ValidateUser(string userName, string password);

    MembershipCreateStatus CreateUser(string userName, string password, string email);

    MembershipUser GetUser();

    MembershipUser GetUser(string userName);

    MembershipUser GetUser(string userName, bool userIsOnline);

    bool ChangePassword(string userName, string password, string newpassword);
  }
}
