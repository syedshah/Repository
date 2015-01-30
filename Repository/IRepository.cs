namespace Repository
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public interface IRepository<T> : IDisposable
  {
    IQueryable<T> Entities { get; }
    T New();
    void Update(T entity);
    T Create(T entity);
    void Create(List<T> collection);
    void Delete(T entity);
    ////void Save();
  }
}
