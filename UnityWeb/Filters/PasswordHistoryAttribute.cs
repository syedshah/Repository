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
  public class PasswordHistoryAttribute : ValidationAttribute
  {
    public IPasswordHistoryService PasswordHistoryService { get; set; }

    public PasswordHistoryAttribute(string userId)
    {
       OtherPropertyName = userId;

       var passwordHistoryRepository = new PasswordHistoryRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
       var userManagerProvider = new UserManagerProvider(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

       PasswordHistoryService = new PasswordHistoryService(passwordHistoryRepository, userManagerProvider);
    }

    public string OtherPropertyName { get; private set; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var result = ValidationResult.Success;

        if (value != null)
        {
            var userId =
                validationContext.ObjectType.GetProperty(OtherPropertyName).GetValue(validationContext.ObjectInstance, null);

            if (PasswordHistoryService.IsPasswordInHistory(userId.ToString(), value.ToString()))
            {
                result = new ValidationResult("This password has already been used recently, please choose another password");
            }
        }
        
        return result;
    }

  }
}