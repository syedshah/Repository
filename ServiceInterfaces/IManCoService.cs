namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface IManCoService
  {
    ManCo CreateManCo(string description, string name);
    ManCo GetManCo(int id);
    ManCo GetManCo(string code);
    IList<ManCo> GetManCos();
    IList<ManCo> GetManCosByUserId(string userId);
    void Update(int manCoId, string code, string description);
    IEnumerable<ManCo> GetManCos(int domicileId);
  }
}
