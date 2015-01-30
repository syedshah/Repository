namespace UnityWeb.Models.Password
{
  public class AnswerModel
  {
    public AnswerModel(string questionId, string answer)
    {
      this.QuestionId = questionId;
      
      this.Answer = answer;
    }

    public string QuestionId { get; set; }

    public string Answer { get; set; }
  }
}