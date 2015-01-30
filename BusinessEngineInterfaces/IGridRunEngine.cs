namespace BusinessEngineInterfaces
{
  using System;
  using ClientProxies.OneStepServiceReference;

  public interface IGridRunEngine
  {
    void ProcessGridRun(
      string fileName,
      string applicationCode,
      string applicationDesc,
      string grid,
      DateTime? startDate,
      DateTime? endDate,
      GridRunStatus gridRunStatus,
      bool? isDebug);
  }
}
