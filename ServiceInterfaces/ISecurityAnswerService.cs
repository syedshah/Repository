namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface ISecurityAnswerService
  {
    void SaveSecurityAnswer(SecurityAnswer securityAnswer);

    void UpdateSecurityAnswer(SecurityAnswer securityAnswer);

    SecurityAnswer GetSecurityAnswer(string userId, string securityQuestionId);

    IList<SecurityAnswer> GetSecurityAnswers(string userId);

    bool SecurityAnswerIsValid(string userId, string securityQuestionId, string answer);

    bool HasSecurityAnswers(string userId);

    void SaveSecurityAnswers(IList<SecurityAnswer> securityAnswers, string userId);

    void UpdateSecurityAnswers(IList<SecurityAnswer> securityAnswers);
  }
}
