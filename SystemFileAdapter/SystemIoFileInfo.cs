namespace SystemFileAdapter
{
  using System.IO;
  using System.Text;
  using FileSystemInterfaces;

  public class SystemIoFileInfo : IFileInfo
  {
    public byte[] StreamData { get; set; }

    public StringBuilder FileData { get; set; }

    private readonly string _fileName;

    public SystemIoFileInfo(string fileName)
    {
      _fileName = fileName;
    }

    public Stream Create()
    {
      var file = new FileInfo(_fileName);
      if (file.Exists)
      {
        throw new IOException("File already exists");
      }
      return file.Create();
    }

    public void Delete()
    {
      var file = new FileInfo(_fileName);
      file.Delete();
    }

    public Stream Open(FileMode fileMode)
    {
      var file = new FileInfo(_fileName);
      return file.Open(fileMode);
    }

    public bool Exists
    {
      get
      {
        var file = new FileInfo(_fileName);
        return file.Exists;
      }
    }
  }
}
