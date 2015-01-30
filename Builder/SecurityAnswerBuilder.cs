namespace Builder
{
  using Entities;

  public class SecurityAnswerBuilder : Builder<SecurityAnswer>
  {
    public SecurityAnswerBuilder()
    {
      Instance = new SecurityAnswer();   
    }

    public SecurityAnswerBuilder WithSecurityQuestionId(string securityQuestionId)
    {
      Instance.SecurityQuestionId = securityQuestionId;
        return this;
    }

    public SecurityAnswerBuilder WithUserId(string userId)
    {
      Instance.UserId = userId;
        return this;
    }
  }
}
