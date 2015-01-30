
namespace UnityWeb.Models.Password
{
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Filters;
  using UnityWeb.Resources.Password;

  public class ChangeCurrentPasswordModel
  {
    public ChangeCurrentPasswordModel()
    {
        
    }

    public ChangeCurrentPasswordModel(string userId)
    {
      UserId = userId;
    }

    public string UserId { get; set; }

    [CurrentPasswordValid("UserId")]
    [Required(ErrorMessageResourceName = "CurrentPasswordError", ErrorMessageResourceType = typeof(Change))]
    public string CurrentPassword { get; set; }

    [PasswordValid]
    [Required(ErrorMessageResourceName = "PasswordError", ErrorMessageResourceType = typeof(Change))]
    [PasswordHistory("UserId")]
    public string NewPassword { get; set; }

    [Required(ErrorMessageResourceName = "ConfirmPasswordError", ErrorMessageResourceType = typeof(Change))]
    [Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordCompareError", ErrorMessageResourceType = typeof(Change))]
    public string ConfirmNewPassword { get; set; }
  }
}