namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class SecurityAnswerRepository : BaseEfRepository<SecurityAnswer>, ISecurityAnswerRepository
  {
    public SecurityAnswerRepository(string connectionString)
          : base(new UnityDbContext(connectionString))
    {
        
    }

    public SecurityAnswer GetSecurityAnswer(string userId, string securityQuestionId)
    {
      return (from a in Entities
                .Include(d => d.SecurityQuestion)
                where a.SecurityQuestionId == securityQuestionId 
                && a.UserId == userId
                select a).FirstOrDefault();
    }

    public IList<SecurityAnswer> GetSecurityAnswers(string userId)
    {
      return (from a in Entities
              .Include(d => d.SecurityQuestion)
              where a.UserId == userId 
              select a).ToList();
    }

    public void DeleteByUserId(string userId)
    {
      var securityAnswers = (from a in Entities where a.UserId == userId select a).ToList();

        foreach (var securityAnswer in securityAnswers)
        {
          this.Delete(securityAnswer);
        }
    }

    public void UpdateSecurityAnswer(string id, string answer)
    {
        SecurityAnswer securityAnswer = (from a in Entities
                                         where a.Id == id
                                          select a).FirstOrDefault();

        securityAnswer.Answer = answer;

        this.Update(securityAnswer);
    }
  }
}
