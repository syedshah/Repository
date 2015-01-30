namespace UnityWeb.Models.Account
{
  using System.ComponentModel.DataAnnotations;
  using System.Web.Mvc;

  public class ChangePasswordModel
  {
    public ChangePasswordModel()
    {
    }

    public ChangePasswordModel(string userName)
    {
      UserName = userName;
    }

    public string UserName { get; set; }

    [Required(ErrorMessage = "Current password can't be empty")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "New password can't be empty")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm new password can't be empty")]
    [DataType(DataType.Password)]
    //[Compare("NewPassword", ErrorMessage = "Confirm new password must match new password")]
    public string ConfirmNewPassword { get; set; }
  }
}