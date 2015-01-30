namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using Entities.File;

  public interface IXmlFileService
  {
    XmlFile GetFile(string fileName);

    void CreateXmlFile(
      string documentSetId,
      string fileName,
      string parentFileName,
      bool offShore,
      int docTypeId,
      int manCoId,
      int status,
      string domicleId,
      string bigZip,
      DateTime allocated,
      string allocatorGrid,
      DateTime received);

    IList<XmlFile> Search(string fileName);

    IList<XmlFile> Search(string fileName, List<int> manCoIds);
  }
}
