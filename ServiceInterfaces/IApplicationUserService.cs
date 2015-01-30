namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public interface IApplicationUserService
  {
    void UpdateUserMancos(string userId, List<ManCo> mancos);

    void UpdateUserDomiciles(string userId, List<Domicile> domiciles);

    IList<ManCo> GetManCos(string userId);
  }
}
