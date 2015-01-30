namespace ServiceInterfaces
{
  using System;

  public interface IConFileService
  {
    void CreateConFile(string documentSetId, string fileName, string parentFileName, DateTime received);
  }
}
