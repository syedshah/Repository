namespace UnityRepository.Interfaces
{
  using Entities;
  using Repository;

  public interface IFileSyncRepository : IRepository<FileSync>
  {
    FileSync GetLatest();
  }
}
