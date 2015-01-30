namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities.File;
  using Repository;

  public interface IZipFileRepository : IRepository<ZipFile>
  {
    IList<ZipFile> SearchBigZip(string fileName);

    IList<ZipFile> SearchBigZip(string fileName, List<int> manCoIds);
  }
}
