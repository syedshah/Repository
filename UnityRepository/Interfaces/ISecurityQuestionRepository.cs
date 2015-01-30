namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface ISecurityQuestionRepository : IRepository<SecurityQuestion>
  {
    IList<SecurityQuestion> GetSecurityQuestions();

    IList<SecurityQuestion> GetThreeRandomSecurityQuestions();
  }
}
