namespace UnityRepository.Repositories
{
    using System;
    using System.Collections.Generic;
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class SecurityQuestionRepository : BaseEfRepository<SecurityQuestion>, ISecurityQuestionRepository
  {
    public SecurityQuestionRepository(string connectionString)
          : base(new UnityDbContext(connectionString))
    {
          
    }

    public IList<SecurityQuestion> GetSecurityQuestions()
    {
      return Entities.ToList();
    }


    public IList<SecurityQuestion> GetThreeRandomSecurityQuestions()
    {
        return (from a in Entities orderby Guid.NewGuid() select a).Take(3).ToList();
    }
  }
}
