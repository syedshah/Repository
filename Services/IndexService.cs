namespace Services
{
  using System;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class IndexService : IIndexService
  {
    private readonly IIndexRepository _indexRepository;

    public IndexService(IIndexRepository indexRepository)
    {
      _indexRepository = indexRepository;
    }

    public IndexDefinition GetIndex(int indexId)
    {
      try
      {
        return _indexRepository.GetIndex(indexId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve index", e);
      }
    }

    public void Update(int indexId, string name, string archiveName, string archiveColumn)
    {
      try
      {
        _indexRepository.Update(indexId, name, archiveName, archiveColumn);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update index", e);
      }
    }

    public IndexDefinition GetByApplicationId(int applicationId)
    {
      try
      {
        return _indexRepository.GetIndexByApplicationId(applicationId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve index", e);
      }
    }
  }
}
