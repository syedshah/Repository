namespace UnityWeb.Models.Password
{
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Filters;
  using UnityWeb.Resources.Password;

  public class ChangePasswordModel
  {
    public ChangePasswordModel()
    {
        
    }

    public ChangePasswordModel(string userId)
    {
      UserId = userId;
    }
      
    public string UserId { get; set; }

    [PasswordValid]
    [Required(ErrorMessageResourceName = "PasswordError", ErrorMessageResourceType = typeof(Change))]
    [PasswordHistory("UserId")]
    public string Password { get; set; }

    [Required(ErrorMessageResourceName = "ConfirmPasswordError", ErrorMessageResourceType = typeof(Change))]
    [Compare("Password", ErrorMessageResourceName = "ConfirmPasswordCompareError", ErrorMessageResourceType = typeof(Change))]
    public string ConfirmPassword { get; set; }
  }
}