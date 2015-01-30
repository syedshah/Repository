namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface IPasswordHistoryService
  {
    IList<PasswordHistory> GetPasswordHistory(string userId, int lastNumberOfRecords);

    IList<PasswordHistory> GetPasswordHistoryByMonths(string userId, int lastNumberOfMonths);

    void SavePasswordHistory(string userId, string passwordHash);

    bool IsPasswordInHistory(string userId, string passwordHash);
  }
}
