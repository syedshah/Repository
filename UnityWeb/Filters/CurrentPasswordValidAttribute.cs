
namespace UnityWeb.Filters
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.Configuration;
  using IdentityWrapper.Identity;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Repositories;

  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
  public class CurrentPasswordValidAttribute : ValidationAttribute
  {
    public IUserService UserService { get; set; }

    public string OtherPropertyName { get; private set; }

    public CurrentPasswordValidAttribute(string userId)
    {
      OtherPropertyName = userId;

      UserService = this.GetUserService();
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var result = ValidationResult.Success;

        if (value != null)
        {
            var userId =
                validationContext.ObjectType.GetProperty(OtherPropertyName).GetValue(validationContext.ObjectInstance, null);

            if (!this.ConfirmUser(value.ToString(), userId.ToString()))
           {
                result = new ValidationResult("This current password provided is wrong");
           }
        }

        return result;
    }

    private bool ConfirmUser(string password, string userId)
    {
      var loggedInUser = this.UserService.GetApplicationUserById(userId);

      var confirmUser = this.UserService.GetApplicationUser(loggedInUser.UserName.Trim(), password.Trim());

      if (loggedInUser != confirmUser)
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    private IUserService GetUserService()
    {
      var globalSettingsRepository = new GlobalSettingRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      var applicationUserRepository = new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      var manCoRepository = new ManCoRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      var passwordHistoryRepository = new PasswordHistoryRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      var userManagerProvider = new UserManagerProvider(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      var roleManagerProvider = new RoleManagerProvider(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      var authenticationManagerProvider = new AuthenticationManagerProvider(new HttpContextBaseProvider());
      var sessionRepository = new SessionRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      return new UserService(
          userManagerProvider,
          roleManagerProvider,
          authenticationManagerProvider,
          applicationUserRepository,
          passwordHistoryRepository,
          globalSettingsRepository,
          manCoRepository,
          sessionRepository);
    }
  }
}