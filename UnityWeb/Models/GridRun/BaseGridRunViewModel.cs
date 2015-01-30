namespace UnityWeb.Models.GridRun
{
  using System;

  public class BaseGridRunViewModel
  {
    public BaseGridRunViewModel(bool filterHouseHolding)
    {
      FilterHouseHolding = filterHouseHolding;
    }

    public bool FilterHouseHolding { get; set; }

    public string Grid { get; set; }

    public DateTime StartDate { get; set; }

    public string BigZip { get; set; }
  }
}