namespace UnityRepository.Repositories
{
  using System.Linq;
  using EFRepository;
  using Entities;
  using Exceptions;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class IndexRepository : BaseEfRepository<IndexDefinition>, IIndexRepository
  {
    public IndexRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public IndexDefinition GetIndex(int id)
    {
      IndexDefinition index = (from i in Entities where i.Id == id select i).FirstOrDefault();
      return index;
    }

    public void Update(int indexId, string name, string archiveName, string archiveColumn)
    {
      IndexDefinition index = GetIndex(indexId);
      if (index == null)
      {
        throw new UnityException("indexId not valid");
      }
      index.UpdateIndex(name, archiveName, archiveColumn);
      Update(index);
    }

    public IndexDefinition GetIndexByApplicationId(int applicationId)
    {
      IndexDefinition index = (from i in Entities where i.ApplicationId == applicationId select i).FirstOrDefault();
      return index;
    }
  }
}
