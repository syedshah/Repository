namespace ServiceInterfaces
{
  using Entities;

  public interface IFileSyncService
  {
    FileSync GetLatest();

    void CreateFileSync(int gridRunId);

    void UpdateFileSync(int gridRunId);
  }
}
