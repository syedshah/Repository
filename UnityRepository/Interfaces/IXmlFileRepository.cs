namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities.File;
  using Repository;

  public interface IXmlFileRepository : IRepository<XmlFile>
  {
    XmlFile GetFile(string fileName);

    IList<XmlFile> Search(string fileName);

    IList<XmlFile> Search(string fileName, List<int> manCoIds);
  }
}
