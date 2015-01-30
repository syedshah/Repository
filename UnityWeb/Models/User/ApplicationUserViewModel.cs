namespace UnityWeb.Models.User
{
  using System.ComponentModel.DataAnnotations;

  public class ApplicationUserViewModel
  {
    public ApplicationUserViewModel(Entities.ApplicationUser user)
    {
      this.UserName = user.UserName;
      this.FirstName = user.FirstName;
      this.LastName = user.LastName;
      this.Id = user.Id;
      this.IsLockedOut = user.IsLockedOut;
    }

    public string Id { get; set; }

    public bool IsLockedOut { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
  }
}