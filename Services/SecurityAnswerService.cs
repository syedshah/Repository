namespace Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Encryptions;

  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class SecurityAnswerService : ISecurityAnswerService
  {
    private readonly ISecurityAnswerRepository _securityAnswerRepository;

    public SecurityAnswerService(ISecurityAnswerRepository securityAnswerRepository)
    {
      this._securityAnswerRepository = securityAnswerRepository;
    }

    public void SaveSecurityAnswer(SecurityAnswer securityAnswer)
    {
      try
      {
        this._securityAnswerRepository.Create(securityAnswer);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create security answer", e);
      }
    }

    public void UpdateSecurityAnswer(SecurityAnswer securityAnswer)
    {
      try
      {
        this._securityAnswerRepository.Update(securityAnswer);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update security answer", e);
      }
    }

    public SecurityAnswer GetSecurityAnswer(string userId, string securityQuestionId)
    {
      try
      {
        return this._securityAnswerRepository.GetSecurityAnswer(userId, securityQuestionId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve security answer", e);
      }
    }

    public bool SecurityAnswerIsValid(string userId, string securityQuestionId, string answer)
    {
      try
      {
        var securityAnswer = this.GetSecurityAnswer(userId, securityQuestionId);

        if (securityAnswer.Answer == UnityEncryption.Encrypt(answer))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to validate security answer", e);
      }
    }


    public bool HasSecurityAnswers(string userId)
    {
      try
      {
        bool result = false;
        var securityAnswers = new List<SecurityAnswer>();
        securityAnswers = this._securityAnswerRepository.GetSecurityAnswers(userId).ToList();

        if (securityAnswers.Count() >= 10)
        {
          result = true;
        }

        return result;
      }
      catch (Exception e)
      {
          throw new UnityException("Unable to retrieve security answers", e);
      }
    }


    public void SaveSecurityAnswers(IList<SecurityAnswer> securityAnswers, string userId)
    {
      try
      {
        foreach (var securityAnswer in securityAnswers)
        {
          this._securityAnswerRepository.Create(securityAnswer);   
        }
      }
      catch (Exception e)
      { 
        throw new UnityException("Unable to save security answers", e);
      }
    }


    public IList<SecurityAnswer> GetSecurityAnswers(string userId)
    {
      try
      {
        return this._securityAnswerRepository.GetSecurityAnswers(userId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve security answers", e);
      }
    }

    public void UpdateSecurityAnswers(IList<SecurityAnswer> securityAnswers)
    {
      try
      {
        foreach (var securityAnswer in securityAnswers)
        {
          this._securityAnswerRepository.UpdateSecurityAnswer(securityAnswer.Id, securityAnswer.Answer);
        }
      }
      catch (Exception e)
      {
         throw new UnityException("Unable to update security answers", e);
      }
    }
  }
}
