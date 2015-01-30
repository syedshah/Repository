
namespace UnityWeb.Filters
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.Text.RegularExpressions;

  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
  public class PasswordValidAttribute : ValidationAttribute
  {
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
        var result = ValidationResult.Success;

        if (value != null)
        {
            if (this.IsComplexityNotValid(value.ToString()))
            {
                result = new ValidationResult("This password is not valid. Please click on the help message to view the password requirements.");
            }
        }

        return result;
      }

      private bool IsComplexityNotValid(string password)
      {
        bool validationResult = false;

        Match match = Regex.Match(password, @"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");

        if (!match.Success)
        {
          validationResult = true;
        }
          
        return validationResult;
      }
  }
}