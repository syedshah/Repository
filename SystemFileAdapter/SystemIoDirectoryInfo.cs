namespace SystemFileAdapter
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using FileSystemInterfaces;

  public class SystemIoDirectoryInfo : IDirectoryInfo
  {
    public IEnumerable<FileInfo> EnumerateFiles(string path, string filter)
    {
      var di = new DirectoryInfo(path);
      FileInfo[] inputFiles = di.GetFiles();

      return inputFiles.Where(f => filter == "*.*" || f.Extension == filter);
    }
  }
}