namespace UnityRepository.Interfaces
{
  using Entities;
  using Repository;
  using System;

  public interface INewsTickerRepository : IRepository<NewsTicker>
  {
    NewsTicker GetNewsTicker(int id);
    NewsTicker GetNewsTicker();
    void Update(int id, string news, DateTime date);
  }
}
