namespace Builder
{
  using Entities.File;

  public class ConFileBuilder : Builder<ConFile>
  {
    public ConFileBuilder()
    {
      Instance = new ConFile();
    }
  }
}
