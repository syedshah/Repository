// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   serive to manage users
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;

  public interface IUserService
  {
    Boolean CheckForPassRenewal(DateTime passwordLastChanged, DateTime lastLogin);

    ApplicationUser GetApplicationUser();

    ApplicationUser GetApplicationUser(String userName);

    ApplicationUser GetApplicationUser(String userName, String password);

    ApplicationUser GetApplicationUserById(String userId);

    void SignIn(ApplicationUser user, Boolean isPersistent);

    void SignOut();

    void ChangePassword(String userId, String newPassword);

    void ChangePassword(String userId, String currentPassword, String newPassword);

    IList<ApplicationUser> GetAllUsers();

    IList<String> GetRoles(String userId);

    IEnumerable<ApplicationUser> GetUsersByDomicile(List<Int32> domiciles);

    PagedResult<ApplicationUser> GetUsersByDomicile(List<Int32> domiciles, Int32 pageNumber, Int32 numberOfItems);

    void CreateUser(String userName, String password, String firstName, String lastName, String email, IList<Int32> manCoIds, Int32 domicileId, IList<String> identityRoles);

    void Updateuser(String userName, String password, String firstName, String lastName, String email, IList<Int32> manCoIds, IList<String> identityRoles, Boolean isApproved);

    List<Int32> GetUserManCoIds();

    List<ManCo> GetUserManCos();

    IEnumerable<Domicile> GetUserDomiciles();

    void UpdateUserLastLogindate(String userId);

    void UpdateUserLastLogindate(String userId, String guid);

    void DeactivateUser(String userName);

    void UpdateUserFailedLogin(String userId);

    void UnlockUser(String userId);

    Boolean IsLockedOut(String userId);
  }
}
