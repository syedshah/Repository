using Entities.File;

namespace Builder
{
  public class ZipFileBuilder : Builder<ZipFile>
  {
    public ZipFileBuilder()
    {
      Instance = new ZipFile();
    }
  }
}
