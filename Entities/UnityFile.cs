namespace Entities
{
  using System.IO;
  using System.Text;

  public class UnityFile
  {
    public FileInfo FileInfo { get; set; }

    public byte[] StreamData { get; set; }

    public StringBuilder FileData { get; set; }
  }
}
