namespace UnityWeb.Models.User
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Web.Mvc;
  using Microsoft.AspNet.Identity.EntityFramework;
  using UnityWeb.Filters;
  using UnityWeb.Models.Shared;

  using WebGrease.Css.Extensions;

  public class AddUserViewModel : IValidatableObject
  {
    public AddUserViewModel()
    {
      Domiciles = new List<SelectListItem>();
      UserManCoViewModel = new UserManCoViewModel();
      AvailableRoleItems = new List<CheckBoxModel>();
      SelectedRoleItems = new List<CheckBoxModel>();
    }

    [Required]
    public string UserName { get; set; }

    public string UserNameHidden { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    [PasswordValid]
    public string Password { get; set; }

    [Required]
    public string ConfirmPassword { get; set; }

    public IList<SelectListItem> Domiciles { get; set; }

    public int DomicileId { get; set; }

    public UserManCoViewModel UserManCoViewModel { get; set; }

    public PostedCheckBox PostedCheckBox { get; set; }

    public PostedCheckBox PostedRolesCheckBox { get; set; }

    public IList<CheckBoxModel> SelectedRoleItems { get; set; }

    public IList<CheckBoxModel> AvailableRoleItems { get; set; }

    public void AddDomiciles(IList<Entities.Domicile> domiciles)
    {
      foreach (var domicile in domiciles)
      {
        Domiciles.Add(new SelectListItem { Value = domicile.Id.ToString(), Text = domicile.Code });
      }
    }

    public void AddIdentityRoles(IList<IdentityRole> identityRoles, IList<string> roles)
    {
      var isNTAdmin = (from r in roles
                       where r.ToLower() == "Admin".ToLower()
                      select r).SingleOrDefault();

      var identityroles = identityRoles.OrderBy(x => x.Name).ToList();

      if (!string.IsNullOrEmpty(isNTAdmin))
      {
        var itemToRemove = (identityroles.Where(r => r.Name.ToLower() == "dstadmin".ToLower())).SingleOrDefault();
        identityroles.Remove(itemToRemove);

        identityroles.ForEach(x => AvailableRoleItems.Add(new CheckBoxModel { Value = x.Id, Text = x.Name }));
      }
      else
      {
        identityRoles.OrderBy(x => x.Name)
                     .ToList()
                     .ForEach(x => AvailableRoleItems.Add(new CheckBoxModel { Value = x.Id, Text = x.Name }));  
      }
    }

    public void AddIdentityRoles(IList<IdentityRole> identityRoles)
    {
      identityRoles.OrderBy(x => x.Name)
                   .ToList()
                   .ForEach(x => AvailableRoleItems.Add(new CheckBoxModel { Value = x.Id, Text = x.Name }));
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (PostedRolesCheckBox.CheckBoxValues.Count() > 1)
      {
        yield return new ValidationResult("Only one role can be selected per user.");
      }
    }
  }
}