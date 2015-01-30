// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditUserViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   view model to edit users
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UnityWeb.Models.User
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Web.Mvc;
  using Microsoft.AspNet.Identity.EntityFramework;
  using UnityWeb.Filters;
  using UnityWeb.Models.Shared;

  public class EditUserViewModel : IValidatableObject
  {
    public EditUserViewModel()
    {
      this.Domiciles = new List<SelectListItem>();

      this.UserManCoViewModel = new UserManCoViewModel();

      this.AvailableRoleItems = new List<CheckBoxModel>();

      this.SelectedRoleItems = new List<CheckBoxModel>();
    }

    public EditUserViewModel(Entities.ApplicationUser user, string loggedOnUserName)
      : this()
    {
      this.UserName = user.UserName;
      this.FirstName = user.FirstName;
      this.LastName = user.LastName;
      this.Email = user.Email;
      this.UserId = user.Id;
      this.IsLockedOut = user.IsLockedOut;
      this.IsApproved = user.IsApproved;
      this.LoggedOnUserName = loggedOnUserName;
    }

    public String UserId { get; set; }

    public String LoggedOnUserName { get; set; }

    [Required]
    public String UserName { get; set; }

    [Required]
    public String FirstName { get; set; }

    [Required]
    public String LastName { get; set; }

    [Required]
    public String Email { get; set; }

    [Required]
    public Boolean IsLockedOut { get; set; }

    [Required]
    public Boolean IsApproved { get; set; }

    [PasswordHistory("UserId")]
    [PasswordValid]
    public String Password { get; set; }

    public String ConfirmPassword { get; set; }

    public IList<SelectListItem> Domiciles { get; set; }

    public IList<CheckBoxModel> SelectedItems { get; set; }

    public IList<CheckBoxModel> SelectedRoleItems { get; set; }

    public IList<CheckBoxModel> AvailableRoleItems { get; set; }

    public PostedCheckBox PostedRolesCheckBox { get; set; }

    public PostedCheckBox PostedCheckBox { get; set; }

    public Int32 DomicileId { get; set; }

    public UserManCoViewModel UserManCoViewModel { get; set; }

    public void AddDomiciles(IList<Entities.Domicile> domiciles)
    {
        foreach (var domicile in domiciles)
        {
          this.Domiciles.Add(new SelectListItem
                              {
                                  Value = domicile.Id.ToString(), 
                                  Text = domicile.Code, 
                                  Selected = domicile.Id == this.DomicileId ? true : false
                              });
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
                       .ForEach(
                           x =>
                           this.AvailableRoleItems.Add(
                               new CheckBoxModel
                               {
                                   Value = x.Id,
                                   Text = x.Name
                               }));
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (this.PostedRolesCheckBox.CheckBoxValues.Count() > 1)
      {
        yield return new ValidationResult("Only one role can be selected per user.");
      }
    }
  }
}