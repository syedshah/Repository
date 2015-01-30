namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IPasswordHistoryRepository : IRepository<PasswordHistory>
  {
    IList<PasswordHistory> GetPasswordHistory(string userId, int lastNumberOfRecords);

    IList<PasswordHistory> GetPasswordHistoryByMonths(string userId, int lastNumberOfMonths);
  }
}
