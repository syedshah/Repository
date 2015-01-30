namespace Entities.File
{
  using System;
  using System.Collections.Generic;
  using Microsoft.Build.Framework;

  public class XmlFile : InputFile
  {
    public enum ValidStatuses
    {
      AllocationFail,
      NotApplicable,
      Allocated,
      Processing,
      ProcessException,
      ProcessOkWaitingForApproval,
      ProcessOkApproved,
      ExtractedHouseholding,
      PrintProductionReady,
      FaxReady,
      EmailReady
    };

    private int _status;

    public XmlFile()
    {
      GridRuns = new List<GridRun>();
    }

    public XmlFile(string documentSetId, string fileName, string parentFileName, bool offShore, string bigZip, DateTime allocated, string allocatorGrid, DateTime received, int docTypeId, int manCoId, string domicileId) 
    : this()
    {
      DocumentSetId = documentSetId;
      FileName = fileName;
      ParentFileName = parentFileName;
      OffShore = offShore;
      BigZip = bigZip;
      Allocated = allocated;
      AlloctorGrid = allocatorGrid;
      Received = received;
      DocTypeId = docTypeId;
      ManCoId = manCoId;
      DomicileId = domicileId;
    }

    public XmlFile(
      string documentSetId,
      string fileName,
      string parentFileName,
      bool offShore,
      int docTypeId,
      int manCoId,
      string domicleId,
      string bigZip,
      DateTime allocated,
      string allocatorGrid,
      DateTime received)
      : this(documentSetId, fileName, parentFileName, offShore, bigZip, allocated, allocatorGrid, received, docTypeId, manCoId, domicleId)
    {
    }
    
    [Required]
    public bool OffShore { get; set; }

    public int DocTypeId { get; set; }
    
    public virtual DocType DocType { get; set; }

    public int ManCoId { get; set; }

    public virtual ManCo ManCo { get; set; }

    public string DomicileId { get; set; }
    
    public Domicile Domicile { get; set; }

    public virtual ICollection<GridRun> GridRuns { get; set; }

    [Required]
    public string BigZip { get; set; }

    public DateTime Allocated { get; set; }
  }
}
