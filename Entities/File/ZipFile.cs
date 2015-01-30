namespace Entities.File
{
  using System;
  using Microsoft.Build.Framework;

  public class ZipFile : InputFile
  {
    public ZipFile()
    {
    }

    public ZipFile(string documentSetId, string fileName, string parentFileName, bool bigZip, DateTime received) : this()
    {
      DocumentSetId = documentSetId;
      FileName = fileName;
      ParentFileName = parentFileName;
      BigZip = bigZip;
      Received = received;
    }

    [Required]
    public bool BigZip { get; set; }
  }
}
