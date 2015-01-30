namespace SystemFileAdapter
{
  using FileSystemInterfaces;

  public interface IFileInfoFactory
  {
    IFileInfo CreateFileInfo(string filename);
  }
}
