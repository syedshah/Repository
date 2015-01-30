namespace NTGEN94
{
  using System.Collections.Generic;

  public class TriggerList
  {
    public TriggerList()
    {
       Records = new List<OtfRecord>();   
    }

    public string AppId { get; set; }

    public string AppName { get; set; }

    public string ManCoId { get; set; }

    public string Name { get; set; }

    public List<OtfRecord> Records { get; set; } 
  }
}
