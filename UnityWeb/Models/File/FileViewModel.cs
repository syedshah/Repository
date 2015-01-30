namespace UnityWeb.Models.File
{
  using System;

  public class FileViewModel
  {
    public FileViewModel(Entities.GridRun gridRun)
    {
      FileName = gridRun.XmlFile.FileName;
      BigZip = gridRun.XmlFile.BigZip;
      Received = gridRun.XmlFile.Received;
      Allocated = gridRun.XmlFile.Allocated;
      AllocatorGrid = gridRun.XmlFile.AlloctorGrid;
      ProcessesGrid = gridRun.Grid;
    }

    public string FileName { get; set; }

    public string BigZip { get; set; }

    public DateTime Received { get; set; }

    public DateTime Allocated { get; set; }

    public string AllocatorGrid { get; set; }

    public string ProcessesGrid { get; set; }
  }
}