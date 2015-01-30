namespace Entities.File
{
  using System;

  public class ConFile : InputFile
  {
    public ConFile()
    {
    }

    public ConFile(string documentSetId, string fileName, string parentFileName, DateTime received)
      : this()
    {
      DocumentSetId = documentSetId;
      FileName = fileName;
      ParentFileName = parentFileName;
      Received = received;
    }
  }
}
