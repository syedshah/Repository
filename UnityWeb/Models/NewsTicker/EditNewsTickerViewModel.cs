namespace UnityWeb.Models.NewsTicker
{
using System;
using System.ComponentModel.DataAnnotations;

  public class EditNewsTickerViewModel
  {
    [Required]
    public int Id { get; set; }

    [Required]
    public string News { get; set; }

    [Required]
    public DateTime Date { get; set; }
  }
}