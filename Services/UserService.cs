namespace Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Encryptions;
  using Entities;
  using Exceptions;
  using IdentityWrapper.Interfaces;
  using Microsoft.AspNet.Identity;
  using Microsoft.Owin.Security;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class UserService : IUserService
  {
    private readonly IGlobalSettingRepository _globalSettingRepository;

    private readonly IApplicationUserRepository _applicationUserRepository;

    private readonly IManCoRepository _manCoRepository;

    private readonly IPasswordHistoryRepository _passwordHistoryRepository;

    private readonly IUserManagerProvider _userManagerProvider;

    private readonly IRoleManagerProvider _roleManagerProvider;

    private readonly IAuthenticationManagerProvider _authenticationManagerProvider;

    private readonly ISessionRepository _sessionRepository;

    private GlobalSetting _globalSetting;

    //Constuctor required for NTGEN99
    public UserService()
    {
    }

    public UserService(
      IUserManagerProvider userManagerProvider,
      IRoleManagerProvider roleManagerProvider,
      IAuthenticationManagerProvider authenticationManagerProvider,
      IApplicationUserRepository applicationUserRepository,
      UnityRepository.Interfaces.IPasswordHistoryRepository passwordHistoryRepository,
      UnityRepository.Interfaces.IGlobalSettingRepository globalSettingRepository,
      IManCoRepository manCoRepository,
      ISessionRepository sessionRepository)
    {
      this._userManagerProvider = userManagerProvider;
      this._roleManagerProvider = roleManagerProvider;
      this._authenticationManagerProvider = authenticationManagerProvider;
      this._manCoRepository = manCoRepository;
      this._applicationUserRepository = applicationUserRepository;
      this._passwordHistoryRepository = passwordHistoryRepository;
      this._globalSettingRepository = globalSettingRepository;
      this._sessionRepository = sessionRepository;
    }

    public bool CheckForPassRenewal(DateTime passwordLastChanged, DateTime lastLogin)
    {
      try
      {
        GetGlobalSettings();

        int passwordExpiresDays = _globalSetting.PasswordExpDays;
        bool newUserPasswordReset = _globalSetting.NewUserPasswordReset;

        if (newUserPasswordReset && (lastLogin == DateTime.MinValue))
        {
          return true;
        }
        else if ((passwordExpiresDays > 0) && ((DateTime.Now - passwordLastChanged).TotalDays >= passwordExpiresDays))
        {
          return true;
        }
        return false;
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to check for password renewal", e);
      }
    }

    private void GetGlobalSettings()
    {
      this._globalSetting = _globalSettingRepository.Get();
    }

    public ApplicationUser GetApplicationUser()
    {
      try
      {
        var user = this._authenticationManagerProvider.User;

        return this._userManagerProvider.FindByName(user.Identity.GetUserName());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get user", e);
      }
    }

    public ApplicationUser GetApplicationUser(String userName)
    {
      try
      {
        return this._userManagerProvider.FindByName(userName);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get user", e);
      }
    }

    public ApplicationUser GetApplicationUser(String userName, String password)
    {
      try
      {
        return this._userManagerProvider.Find(userName, password);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get user", e);
      }
    }

    public void SignOut()
    {
      this._authenticationManagerProvider.SignOut();
    }

    public List<int> GetUserManCoIds()
    {
      try
      {
        var user = this.GetApplicationUser();
        var listManCoIds = new List<int>();

        if (user != null)
        {
          user.ManCos.ToList().ForEach(x => listManCoIds.Add(x.ManCoId));
        }

        return listManCoIds;
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get user manco ids", e);
      }
    }

    public List<ManCo> GetUserManCos()
    {
      var user = this.GetApplicationUser();
      return user.ManCos.Select(usermanCo => usermanCo.ManCo).ToList();
    }

    public IList<ApplicationUser> GetAllUsers()
    {
      try
      {
        return this._applicationUserRepository.GetUsers();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get all users", e);
      }
    }

    public IList<String> GetRoles(String userId)
    {
      try
      {
        return this._userManagerProvider.GetRoles(userId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get roles", e);
      }
    }

    public void CreateUser(String userName, String password, String firstName, String lastName, String email, IList<Int32> manCoIds, Int32 domicileId, IList<String> identityRoleIds)
    {
      var user = new ApplicationUser
      {
        UserName = userName,
        FirstName = firstName,
        LastName = lastName,
        Email = email,
        IsApproved = true,
        IsLockedOut = false,
        LastPasswordChangedDate = DateTime.Now
      };

      user.ManCos = this.GenerateUserManCos(manCoIds.ToList(), user.Id);

      var domicileList = new List<ApplicationUserDomicile>();
      domicileList.Add(new ApplicationUserDomicile { DomicileId = domicileId, UserId = user.Id });
      user.Domiciles = domicileList;

      var result = this._userManagerProvider.Create(user, password);

      if (result.Succeeded)
      {
        this.StorePasswordInHistory(user.Id, password);
      }

      this.ProcessRoles(user.Id, identityRoleIds, "create");
    }

    public void Updateuser(String userName, String password, String firstName, String lastName, String email, IList<int> manCoIds, IList<String> identityRoleIds, Boolean isApproved)
    {
      var user = this._userManagerProvider.FindByName(userName);

      user.FirstName = firstName;
      user.LastName = lastName;
      user.Email = email;
      user.IsApproved = isApproved;

      var manCos = this._manCoRepository.GetManCos(manCoIds);

      this._applicationUserRepository.UpdateUserMancos(user.Id, manCos.ToList());

      this._userManagerProvider.Update(user);

      if (password != null)
      {
        this._userManagerProvider.RemovePassword(user.Id);
        var result = this._userManagerProvider.AddPassword(user.Id, password);

        if (result.Succeeded)
        {
          this.StorePasswordInHistory(user.Id, password);
        }
      }

      this.ProcessRoles(user.Id, identityRoleIds, "edit");
    }

    private void StorePasswordInHistory(String userId, String password)
    {
      password = UnityEncryption.Encrypt(password);

      this._passwordHistoryRepository.Create(
        new PasswordHistory { PasswordHash = password, UserId = userId, LogDate = DateTime.Now });
    }

    public IEnumerable<Domicile> GetUserDomiciles()
    {
      var user = this.GetApplicationUser();
      return user.Domiciles.Select(domicile => domicile.Domicile).ToList();
    }

    public void UpdateUserLastLogindate(String userId)
    {
      UpdateUserLastLogindate(userId, String.Empty);
    }

    public void UpdateUserLastLogindate(String userId, String guid)
    {
      try
      {
        this._applicationUserRepository.UpdateUserlastLogindate(userId);

        if (guid != String.Empty)
        {
          var session = new Session
          {
            UserId = userId,
            Guid = guid,
            Start = DateTime.Now
          };

          this._sessionRepository.Create(session); 
        }
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update user last login date", e);
      }
    }

    public void SignIn(ApplicationUser user, bool isPersistent)
    {
      this._authenticationManagerProvider.SignOut(DefaultAuthenticationTypes.ExternalCookie);
      var identity = this._userManagerProvider.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
      this._authenticationManagerProvider.SignIn(
        new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
    }

    private IList<ApplicationUserManCo> GenerateUserManCos(List<int> manCoIds, String userId)
    {
      var manCos = this._manCoRepository.GetManCos(manCoIds);
      return manCos.Select(manCo => new ApplicationUserManCo() { UserId = userId, ManCoId = manCo.Id }).ToList();
    }

    public IEnumerable<ApplicationUser> GetUsersByDomicile(List<int> domiciles)
    {
      try
      {
        return this._applicationUserRepository.GetUsersByDomicile(domiciles);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get users", e);
      }
    }

    public PagedResult<ApplicationUser> GetUsersByDomicile(List<int> domiciles, int pageNumber, int numberOfItems)
    {
      try
      {
        return this._applicationUserRepository.GetUsersByDomicile(domiciles, pageNumber, numberOfItems);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get users", e);
      }
    }

    public void ChangePassword(String userId, String newPassword)
    {
      try
      {
        this._userManagerProvider.RemovePassword(userId);

        this._userManagerProvider.AddPassword(userId, newPassword);

        this._applicationUserRepository.UpdateUserLastPasswordChangedDate(userId);

        this.StorePasswordInHistory(userId, newPassword);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to change password", e);
      }
    }

    public void ChangePassword(String userId, String currentPassword, String newPassword)
    {
      try
      {
        this._userManagerProvider.ChangePassword(userId, currentPassword, newPassword);

        this._applicationUserRepository.UpdateUserLastPasswordChangedDate(userId);

        this.StorePasswordInHistory(userId, newPassword);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to change password", e);
      }
    }

    public ApplicationUser GetApplicationUserById(String userId)
    {
      try
      {
        return this._userManagerProvider.FindById(userId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to change password", e);
      }
    }

    public void DeactivateUser(String userName)
    {
      try
      {
        var user = this._userManagerProvider.FindByName(userName);
        this._applicationUserRepository.DeactivateUser(user.Id);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to delete user", e);
      }
    }

    public void UpdateUserFailedLogin(String userId)
    {
      try
      {
        GetGlobalSettings();
        var user = _applicationUserRepository.UpdateUserFailedLogin(userId);
        if (user.FailedLogInCount >= _globalSetting.MaxLogInAttempts)
        {
          _applicationUserRepository.DeactivateUser(userId);
        }
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update failed login count", e);
      }
    }

    public void UnlockUser(String userId)
    {
      try
      {
        this._applicationUserRepository.UnlockUser(userId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to unlock user", e);
      }
    }

    public bool IsLockedOut(String userId)
    {
      try
      {
        return _applicationUserRepository.IsLockedOut(userId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to check if user is locked out", e);
      }
    }

    private void AddUserToRole(String userId, String roleId)
    {
      var role = this._roleManagerProvider.FindById(roleId);

      this._userManagerProvider.AddToRole(userId, role.Name);
    }

    private void ProcessRoles(String userId, IList<String> roleIds, String transaction)
    {
      if (transaction == "edit")
      {
        var roles = this._userManagerProvider.GetRoles(userId);

        foreach (var role in roles)
        {
          this._userManagerProvider.RemoveFromRole(userId, role);
        }
      }

      foreach (var roleId in roleIds)
      {
        this.AddUserToRole(userId, roleId);
      }
    }
  }
}
