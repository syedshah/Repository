namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IApplicationUserRepository : IRepository<ApplicationUser>
  {
    ApplicationUser GetUserByName(string userName);

    IList<ApplicationUser> GetUsers();

    PagedResult<ApplicationUser> GetUserReport(int domicileId, int pageNumber, int numberOfItems);

    IList<ApplicationUser> GetUsersByDomicile(IList<int> domicileIds);

    PagedResult<ApplicationUser> GetUsersByDomicile(IList<int> domicileIds, int pageNumber, int numberOfItems);

    void UpdateUserMancos(string userId, List<ManCo> manCos);

    void UpdateUserDomiciles(string userId, List<Domicile> domiciles);

    IList<ManCo> GetManCos(string userId);

    void UpdateUserlastLogindate(string userId);

    void UpdateUserLastPasswordChangedDate(string userId);

    ApplicationUser UpdateUserFailedLogin(string userId);

    void UnlockUser(string userId);

    void DeactivateUser(string userId);

    bool IsLockedOut(string userId);
  }
}
