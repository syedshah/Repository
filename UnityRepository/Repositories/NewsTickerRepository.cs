namespace UnityRepository.Repositories
{
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;
using System;

  public class NewsTickerRepository : BaseEfRepository<NewsTicker>, INewsTickerRepository
  {
    public NewsTickerRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public NewsTicker GetNewsTicker(int id)
    {
      return (from n in Entities
              where n.Id == id
              select n).FirstOrDefault();
    }

    public NewsTicker GetNewsTicker()
    {
      return Entities.FirstOrDefault();
    }

    public void Update(int id, string news, DateTime date)
    {
      NewsTicker nt= new NewsTicker{Id=id,News=news,Date=date};
      base.Update(nt);
    }

  }
}

