namespace ServiceInterfaces
{
  using System;
  using Entities;

  public interface INewsTickerService
  {
    NewsTicker AddNewsTicker(string news, DateTime date);
    NewsTicker GetNewsTicker();
    int UpdateNewsTicker(int id, string news, DateTime date);
  }
}
