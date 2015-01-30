namespace UnityWeb.Models.NewsTicker
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class NewsTickerViewModel
  {
    public NewsTickerViewModel()
    {
    }

    public NewsTickerViewModel(Entities.NewsTicker newsTicker)
    {
      if (newsTicker != null)
      {
        Id = newsTicker.Id;
        News = newsTicker.News;
        Date = newsTicker.Date;
      }
    }

    public int Id { get; set; }

    [Required]
    public string News { get; set; }

    [Required]
    public DateTime Date { get; set; }
  }
}