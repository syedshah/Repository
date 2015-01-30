namespace Builder
{
  using Entities;

  public class SecurityQuestionBuilder : Builder<SecurityQuestion>
  {
    public SecurityQuestionBuilder()
    {
      Instance = new SecurityQuestion();    
    }
  }
}
