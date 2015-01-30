namespace UnityRepository.Interfaces
{
  using Entities;
  using Repository;

  public interface IIndexRepository : IRepository<IndexDefinition>
  {
    IndexDefinition GetIndex(int id);
    void Update(int indexId, string name, string archiveName, string archiveColumn);
    IndexDefinition GetIndexByApplicationId(int applicationId);
  }
}
