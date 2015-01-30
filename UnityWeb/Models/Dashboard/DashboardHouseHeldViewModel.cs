namespace UnityWeb.Models.Dashboard
{
  using System;

  public class DashboardHouseHeldViewModel
  {
    public DashboardHouseHeldViewModel(String houseHoldingGrid, DateTime startDate, DateTime endDate)
    {
      HouseHoldingGrid = houseHoldingGrid;
      StartDate = startDate;
      EndDate = endDate;
    }

    public String HouseHoldingGrid { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
  }
}