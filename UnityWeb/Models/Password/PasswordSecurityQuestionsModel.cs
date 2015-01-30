namespace UnityWeb.Models.Password
{
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Resources.Password;

  public class PasswordSecurityQuestionsModel
  {
    public PasswordSecurityQuestionsModel()
    {
         
    }

    public PasswordSecurityQuestionsModel(string userId,
        string question1Id, 
        string question1,
        string question2Id, 
        string question2,
        string question3Id, 
        string question3)

    {
        this.Question1 = question1;
        this.Question1Id = question1Id;
        this.Question2 = question2;
        this.Question2Id = question2Id;
        this.Question3 = question3;
        this.Question3Id = question3Id;
        this.UserId = userId;
    }

    public string UserId { get; set; }

    public string Question1Id { get; set; }

    public string Question1 { get; set; }

    [Required(ErrorMessageResourceName = "Question1ErrorMessage", ErrorMessageResourceType = typeof(SecurityQuestions))]
    public string Answer1 { get; set; }

    public string Question2Id { get; set; }

    public string Question2 { get; set; }

    [Required(ErrorMessageResourceName = "Question2ErrorMessage", ErrorMessageResourceType = typeof(SecurityQuestions))]
    public string Answer2 { get; set; }

    public string Question3Id { get; set; }

    public string Question3 { get; set; }

    [Required(ErrorMessageResourceName = "Question3ErrorMessage", ErrorMessageResourceType = typeof(SecurityQuestions))]
    public string Answer3 { get; set; }
  }
}