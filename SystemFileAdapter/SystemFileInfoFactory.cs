namespace SystemFileAdapter
{
  using FileSystemInterfaces;

  public class SystemFileInfoFactory : IFileInfoFactory
  {
    public IFileInfo CreateFileInfo(string filename)
    {
      return new SystemIoFileInfo(filename);
    }
  }
}