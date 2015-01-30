namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using Entities.File;

  public interface IZipFileService
  {
    void CreateBigZipFile(string documentSetId, string fileName, DateTime received);

    void CreateLittleZipFile(string documentSetId, string fileName, string parentFileName, DateTime received);

    IList<ZipFile> SearchBigZip(string fileName);

    IList<ZipFile> SearchBigZip(string fileName, List<int> manCoIds);
  }
}
