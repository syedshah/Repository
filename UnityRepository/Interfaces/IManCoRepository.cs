namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IManCoRepository : IRepository<ManCo>
  {
    ManCo GetManCo(int id);
    ManCo GetManCo(string code);
    IList<ManCo> GetManCos();
    IList<ManCo> GetManCos(IList<int> manCoIds);
    IList<ManCo> GetManCosByUserId(string userId);
    void Update(int manCoId, string code, string description);
    IEnumerable<ManCo> GetManCos(int domicileId);
  }
}
