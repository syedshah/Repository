namespace UnityWeb.Models.User
{
  using System.ComponentModel;
  using System.ComponentModel.DataAnnotations;
  using System.Security.Principal;

  public class UserViewModel : IPrincipal, IIdentity
  {
    public int Id { get; set; }

    [Required]
    [DisplayName(@"User name")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Editable(false)]
    public bool IsLoggedIn { get; set; }

    public bool IsInRole(string role)
    {
      throw new System.NotImplementedException();
    }

    public IIdentity Identity
    {
      get { return this; }
    }

    public string Name { get; private set; }

    public string AuthenticationType 
    { 
      get { return "Cookie"; }
    }

    public bool IsAuthenticated
    {
      get { return IsLoggedIn; }
    }
  }
}