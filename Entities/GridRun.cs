namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.Data.Linq.Mapping;
  using Entities.File;
  using Microsoft.Build.Framework;

  public class GridRun
  {
    public GridRun()
    {
      Documents = new List<Document>();
    }

    public GridRun(string grid, bool isDebug, DateTime startDate, DateTime? enddate, int status) : this()
    {
      Grid = grid;
      IsDebug = isDebug;
      StartDate = startDate;
      EndDate = enddate;
      Status = status;
    }

    public enum ValidStatuses
    {
      Undefined,
      Processing,
      OK,
      ProcessAbort,
      ProcessException,
      ScriptException,
      ApplicationTimeout,
      EmptyTrigger,
      NoData,
      ProcessUnexpectedClosure
    };

    private int _status;

    public int Id { get; set; }

    [Column(Name = "xml_file_id")]
    public int? XmlFileId { get; set; }

    [Column(Name = "application_id")]
    public int ApplicationId { get; set; }

    [Column(Name = "householding_run_id")]
    public int? HouseHoldingRunId { get; set; }

    [Required]
    public string Grid { get; set; }

    [Required]
    public bool IsDebug { get; set; }

    [Required]
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public virtual int Status
    {
      get { return _status; }
      set
      {
        if (Enum.IsDefined(typeof(ValidStatuses), value))
        {
          _status = value;
        }
        else
        {
          _status = (int)ValidStatuses.Undefined;
        }
      }
    }

    public virtual XmlFile XmlFile { get; set; }

    public virtual Application Application { get; set; }

    public virtual HouseHoldingRun HouseHoldingRun { get; set; }

    public virtual ICollection<Document> Documents { get; set; }

    public void AddGridRun(int applicationId, int? xmlFileId, DateTime? endDate, string grid, bool isDebug, DateTime? startDate, int status)
    {
      ApplicationId = applicationId;
      XmlFileId = xmlFileId;
      EndDate = endDate;
      Grid = grid;
      IsDebug = isDebug;
      StartDate = startDate;
      Status = status;
    }

    public void UpdateGridRun(DateTime? startDate, DateTime? endDate, int status, int? xmlFileId, int? houseHoldingRundId)
    {
      if (endDate != null)
      {
        EndDate = endDate;
      }

      if (startDate != null)
      {
        StartDate = startDate;
      }

      if (status != 0)
      {
        Status = status;  
      }

      if (xmlFileId != null && XmlFileId == null)
      {
        XmlFileId = xmlFileId;
      }

      if (houseHoldingRundId != null)
      {
        HouseHoldingRunId = houseHoldingRundId.Value;
      }
    }
  }
}
