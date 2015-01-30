namespace IdentityWrapper
{
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;

    using Owin;

  /// <summary>
  /// Startup configuration for Open Web Interface for .Net
  /// </summary>
  public partial class Startup
  {
    /// <summary>
    /// Configures the authentication.
    /// </summary>
    /// <param name="app">The application.</param>
    public void ConfigureAuth(IAppBuilder app)
    {
        // Enable the application to use a cookie to store information for the signed in user
        app.UseCookieAuthentication(new CookieAuthenticationOptions
        {
            AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            LoginPath = new PathString("/Session/New")
        });

      // Enable the application to use a cookie to store information for the signed in user
      // and to use a cookie to temporarily store information about a user logging in with a third party login provider
      //  app.UseSignInCookies();

      // Uncomment the following lines to enable logging in with third party login providers
      // app.UseMicrosoftAccountAuthentication(
      //    clientId: "",
      //    clientSecret: "");

      // app.UseTwitterAuthentication(
      //   consumerKey: "",
      //   consumerSecret: "");

      // app.UseFacebookAuthentication(
      //   appId: "",
      //   appSecret: "");

      // app.UseGoogleAuthentication();
    }
  }
}
