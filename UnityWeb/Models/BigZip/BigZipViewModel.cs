namespace UnityWeb.Models.BigZip
{
  using System;

  public class BigZipViewModel
  {
    public BigZipViewModel(Entities.File.ZipFile zipFile)
    {
      FileName = zipFile.FileName;
      ParentFileName = zipFile.ParentFileName;
      Received = zipFile.Received;
      BigZip = zipFile.BigZip;
      DocumentSetId = zipFile.DocumentSetId;
    }

    public string DocumentSetId { get; set; }
    
    public string FileName { get; set; }

    public string ParentFileName { get; set; }

    public bool BigZip { get; set; }

    public DateTime Received { get; set; }
  }
}