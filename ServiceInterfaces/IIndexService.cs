namespace ServiceInterfaces
{
  using Entities;

  public interface IIndexService
  {
    IndexDefinition GetIndex(int indexId);
    void Update(int indexId, string name, string archiveName, string archiveColumn);
    IndexDefinition GetByApplicationId(int applicationId);
  }
}
