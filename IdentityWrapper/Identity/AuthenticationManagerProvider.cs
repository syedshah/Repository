namespace IdentityWrapper.Identity
{
  using System.Web;
  using IdentityWrapper.Interfaces;
  using System.Security.Claims;
  using Microsoft.Owin.Security;

  public class AuthenticationManagerProvider : IAuthenticationManagerProvider
  {
    private readonly IHttpContextBaseProvider _httpContextBaseProvider;
    private readonly IAuthenticationManager _authenticationManager;
    private readonly ClaimsPrincipal _user;

    public AuthenticationManagerProvider(IHttpContextBaseProvider httpContextBaseProvider)
    {
      this._httpContextBaseProvider = httpContextBaseProvider;
      this._authenticationManager = this._httpContextBaseProvider.HttpContext.GetOwinContext().Authentication;
      this._user = this._authenticationManager.User;
    }

    public IAuthenticationManager AuthenticationManager
    {
      get
      {
        return this._authenticationManager;
      }
    }

    public ClaimsPrincipal User
    {
      get
      {
         return this._user;
      }
    }

    public void SignOut()
    {
      this._authenticationManager.SignOut();
    }

    public void SignOut(string authenticationType)
    {
      this.AuthenticationManager.SignOut(authenticationType);
    }

    public void SignIn(AuthenticationProperties authProps, ClaimsIdentity identity)
    {
      this.AuthenticationManager.SignIn(authProps, identity);
    }
  }
}
