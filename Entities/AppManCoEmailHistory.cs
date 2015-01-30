namespace Entities
{
  using System;

  public class AppManCoEmailHistory
  {
    public int Id { get; set; }

    public virtual AppManCoEmail AppManCoEmail { get; set; }

    public int AppManCoEmailId { get; set; }

    public String ChangeInfo { get; set; }

    public String ChangedBy { get; set; }

    public DateTime ChangeDate { get; set; }
  }
}
