namespace Builder
{
  using Entities;

  public class ExportFileBuilder : Builder<ExportFile>
  {
    public ExportFileBuilder()
    {
      Instance = new ExportFile();
    }
  }
}
