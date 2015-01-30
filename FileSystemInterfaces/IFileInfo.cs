namespace FileSystemInterfaces
{
   using System.IO;

  public interface IFileInfo
  {
    Stream Create();
    void Delete();
    Stream Open(FileMode fileMode);
    bool Exists { get; }

  }
}
