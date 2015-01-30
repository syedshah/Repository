namespace Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Encryptions;
  using Entities;
  using Exceptions;
  using IdentityWrapper.Interfaces;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class PasswordHistoryService : IPasswordHistoryService
  {
    private readonly IPasswordHistoryRepository _passwordHistoryRepository;
    private readonly IUserManagerProvider _userManagerProvider;

    public PasswordHistoryService(
        IPasswordHistoryRepository passwordHistoryRepository,
        IUserManagerProvider userManagerProvider)
    {
      this._passwordHistoryRepository = passwordHistoryRepository;
      this._userManagerProvider = userManagerProvider;
    }

    public IList<PasswordHistory> GetPasswordHistory(string userId, int lastNumberOfRecords)
    {
      try
      {
        return this._passwordHistoryRepository.GetPasswordHistory(userId, lastNumberOfRecords);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get password history", e);
      }
    }

    public IList<PasswordHistory> GetPasswordHistoryByMonths(string userId, int lastNumberOfMonths)
    {
      try
      {
         return this._passwordHistoryRepository.GetPasswordHistoryByMonths(userId, lastNumberOfMonths);
      }
      catch (Exception e)
      {
         throw new UnityException("Unable to get password history", e);
      }
    }

    public void SavePasswordHistory(string userId, string passwordHash)
    {
      try
      {
        var passwordHistory = new PasswordHistory
                                  {
                                      UserId = userId,
                                      PasswordHash = passwordHash,
                                      LogDate = DateTime.Now
                                  };

        this._passwordHistoryRepository.Create(passwordHistory);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to save password history", e);
      }
    }

    public bool IsPasswordInHistory(string userId, string passwordHash)
    {
      try
      {
        var passwordHistory = this.GetPasswordHistoryByMonths(userId, 12);

        var passwordList = this.GetEncryptedPasswordList(passwordHistory);

        if (passwordList.Contains(UnityEncryption.Encrypt(passwordHash)))
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
        throw new UnityException("Unable to validate password in password history", e);
      }
    }

    private IList<string> GetDecryptedPasswordList(IList<PasswordHistory> passwordHistory, string passPhrase)
    {
        var passwordList = new List<string>();

        passwordHistory.ToList().ForEach(x => passwordList.Add(UnityEncryption.Decrypt(x.PasswordHash, passPhrase)));

        return passwordList;
    }

    private IList<string> GetEncryptedPasswordList(IList<PasswordHistory> passwordHistory)
    {
        var passwordList = new List<string>();

        passwordHistory.ToList().ForEach(x => passwordList.Add(x.PasswordHash));

        return passwordList;
    }
  }
}
