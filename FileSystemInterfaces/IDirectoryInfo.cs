using System.Collections.Generic;

namespace FileSystemInterfaces
{
  using System.IO;

  public interface IDirectoryInfo
  {
   // IEnumerable<IFileInfo> EnumerateFiles(string path, string filter);

    IEnumerable<FileInfo> EnumerateFiles(string path, string filter);
  }
}
