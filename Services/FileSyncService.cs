namespace Services
{
  using System;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class FileSyncService : IFileSyncService
  {
    private readonly IFileSyncRepository _fileSyncRepository;

    public FileSyncService(IFileSyncRepository oneStepSyncRepository)
    {
      _fileSyncRepository = oneStepSyncRepository;
    }

    public FileSync GetLatest()
    {
      try
      {
        var syncDate = _fileSyncRepository.GetLatest();
        return syncDate == null ? new FileSync() { SyncDate = new DateTime(2013, 1, 1)} : syncDate;
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve file sync", e);
      }
    }

    public void CreateFileSync(int gridRunId)
    {
      try
      {
        FileSync fileSync = new FileSync(gridRunId);
        _fileSyncRepository.Create(fileSync);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create file sync", e);
      }
    }

    public void UpdateFileSync(int gridRunId)
    {
      try
      {
        FileSync fileSync = _fileSyncRepository.GetLatest();

        if (fileSync == null)
        {
          fileSync = new FileSync(gridRunId);
          _fileSyncRepository.Create(fileSync);
        }
        else
        {
          fileSync.GridRunId = gridRunId;
          fileSync.SyncDate = DateTime.Now;
          _fileSyncRepository.Update(fileSync);
        }
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update file sync", e);
      }
    }
  }
}
