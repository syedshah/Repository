namespace Services
{
  using System;
  using BusinessEngineInterfaces;
  using ServiceInterfaces;

  public class SyncService : ISyncService
  {
    private readonly IFileSyncService _fileSyncService;
    private readonly IOneStepService _oneStepService;
    private readonly IGridRunEngine _gridRunEngine;

    public SyncService(
      IFileSyncService fileSyncService,
      IOneStepService oneStepService,
      IGridRunEngine gridRunEngine)
    {
      _fileSyncService = fileSyncService;
      _oneStepService = oneStepService;
      _gridRunEngine = gridRunEngine;
    }

    public void Synchronise()
    {
      var oneStepSync = _fileSyncService.GetLatest();

      var fileData = _oneStepService.GetFileData(oneStepSync.SyncDate.AddMinutes(-20));
      _fileSyncService.UpdateFileSync(1);

      try
      {
        foreach (var fileStatusData in fileData)
        {
          _gridRunEngine.ProcessGridRun(
            fileStatusData.FileName,
            fileStatusData.ApplicationCode,
            fileStatusData.ApplicationDesc,
            fileStatusData.Grid,
            fileStatusData.StartDate,
            fileStatusData.EndDate,
            fileStatusData.GridRunStatus,
            fileStatusData.IsDebug);
        }
      }
      catch (Exception e)
      {
        throw new Exception("Could not process grid run", e);
      }
    }    
  }
}
