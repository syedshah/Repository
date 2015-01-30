namespace IdentityWrapper.Interfaces
{
  using System.Security.Claims;
  using Microsoft.Owin.Security;

  public interface IAuthenticationManagerProvider
  {
    IAuthenticationManager AuthenticationManager { get; }

    ClaimsPrincipal User { get; }

    void SignOut();

    void SignOut(string authenticationType);

    void SignIn(AuthenticationProperties authProps, ClaimsIdentity identity);
  }
}
