namespace UnityRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class PasswordHistoryRepository : BaseEfRepository<PasswordHistory>, IPasswordHistoryRepository
  {
    public PasswordHistoryRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public IList<PasswordHistory> GetPasswordHistory(string userId, int lastNumberOfRecords)
    {
        return
            (from a in Entities orderby a.Id descending where a.UserId == userId select a).Take(lastNumberOfRecords)
                                                                                          .ToList();
    }

    public IList<PasswordHistory> GetPasswordHistoryByMonths(string userId, int lastNumberOfMonths)
    {
        var previousDate = DateTime.Now.AddMonths(-lastNumberOfMonths);
        return
            (from a in Entities
             orderby a.Id descending
             where a.LogDate >= previousDate
             && a.UserId == userId
             select a).ToList();
    }
  }
}
