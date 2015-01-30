namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface ISecurityQuestionService
  {
    IList<SecurityQuestion> GetSecurityQuestions();

    IList<SecurityQuestion> GetThreeRandomSecurityQuestions();
  }
}
