namespace UnityWeb.Models.Security
{
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Resources.Security;

  public class SecurityQuestionModel
  {
    public string UserId { get; set; }

    public string Question1 { get; set; }

    public string Question1Id { get; set; }

    [Required(ErrorMessageResourceName = "Question1ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer1 { get; set; }

    public string Question2 { get; set; }

    public string Question2Id { get; set; }

    [Required(ErrorMessageResourceName = "Question2ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer2 { get; set; }

    public string Question3 { get; set; }

    public string Question3Id { get; set; }

   [Required(ErrorMessageResourceName = "Question3ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer3 { get; set; }

    public string Question4 { get; set; }

    public string Question4Id { get; set; }

    [Required(ErrorMessageResourceName = "Question4ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer4 { get; set; }

    public string Question5 { get; set; }

    public string Question5Id { get; set; }

    [Required(ErrorMessageResourceName = "Question5ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer5 { get; set; }

    public string Question6 { get; set; }

    public string Question6Id { get; set; }

    [Required(ErrorMessageResourceName = "Question6ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer6 { get; set; }

    public string Question7 { get; set; }

    public string Question7Id { get; set; }

   [Required(ErrorMessageResourceName = "Question7ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer7 { get; set; }

    public string Question8 { get; set; }

    public string Question8Id { get; set; }

    [Required(ErrorMessageResourceName = "Question8ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer8 { get; set; }

    public string Question9 { get; set; }

    public string Question9Id { get; set; }

    [Required(ErrorMessageResourceName = "Question9ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer9 { get; set; }

    public string Question10 { get; set; }

    public string Question10Id { get; set; }

    [Required(ErrorMessageResourceName = "Question10ErrorMessage", ErrorMessageResourceType = typeof(Edit))]
    public string Answer10 { get; set; }
  }
}