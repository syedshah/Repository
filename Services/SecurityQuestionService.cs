namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class SecurityQuestionService : ISecurityQuestionService
  {
    private readonly ISecurityQuestionRepository _securityQuestionRepository;

    public SecurityQuestionService(ISecurityQuestionRepository securityQuestionRepository)
    {
      this._securityQuestionRepository = securityQuestionRepository;
    }

    public IList<SecurityQuestion> GetSecurityQuestions()
    {
      try
      {
        return this._securityQuestionRepository.GetSecurityQuestions();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve security questions", e);
      }
    }


    public IList<SecurityQuestion> GetThreeRandomSecurityQuestions()
    {
       try
       {
           return this._securityQuestionRepository.GetThreeRandomSecurityQuestions();
       }
       catch (Exception e)
       {
          throw new UnityException("Unable to retrieve security questions", e);
       }
    }
  }
}
