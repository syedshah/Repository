namespace UnityWeb.Models.Password
{
  using System.ComponentModel;
  using System.ComponentModel.DataAnnotations;

  public class ForgottenPasswordModel
  {
    [Required]
    [DisplayName(@"User name")]
    public string UserName { get; set; }
  }
}