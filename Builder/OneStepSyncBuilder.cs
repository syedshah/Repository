namespace Builder
{
  using Entities;

  public class OneStepSyncBuilder : Builder<FileSync>
  {
    public OneStepSyncBuilder()
    {
      Instance = new FileSync();
    }
  }
}
