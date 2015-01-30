namespace Services
{
  using System;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class NewsTickerService : INewsTickerService
  {
    private readonly INewsTickerRepository _newsTickerRepository;

    public NewsTickerService(INewsTickerRepository newsTickerRepository)
    {
      _newsTickerRepository = newsTickerRepository;
    }    
    public NewsTicker AddNewsTicker(string news, DateTime date)
    {
      try
      {
        var newsTicker = new NewsTicker(news,date);
        return _newsTickerRepository.Create(newsTicker);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create News Ticker", e);
      }
    }

    public NewsTicker GetNewsTicker()
    {
      try
      {
        return _newsTickerRepository.GetNewsTicker();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve News Ticker", e);
      }
    }

    public int UpdateNewsTicker(int id, string news, DateTime date)
    {
      try
      {     
        _newsTickerRepository.Update(id,news,date);
        return 1;
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update News Ticker", e);
      }
    }
  }
}
