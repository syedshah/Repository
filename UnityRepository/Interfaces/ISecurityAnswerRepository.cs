namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface ISecurityAnswerRepository : IRepository<SecurityAnswer>
  {
    SecurityAnswer GetSecurityAnswer(string userId, string securityQuestionId);

    IList<SecurityAnswer> GetSecurityAnswers(string userId);

    void DeleteByUserId(string userId);

    void UpdateSecurityAnswer(string id, string answer);
  }
}
