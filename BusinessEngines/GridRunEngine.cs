namespace BusinessEngines
{
  using System;
  using BusinessEngineInterfaces;
  using ClientProxies.OneStepServiceReference;
  using Entities;
  using Entities.File;
  using Logging;
  using ServiceInterfaces;

  public class GridRunEngine : IGridRunEngine
  {
    private readonly IGridRunService _gridRunService;
    private readonly IXmlFileService _xmlFileService;
    private readonly IApplicationService _applicationService;
    private readonly ILogger _logger;

    public GridRunEngine(IGridRunService gridRunService, IXmlFileService xmlFileService, IApplicationService applicationService, ILogger logger)
    {
      _gridRunService = gridRunService;
      _xmlFileService = xmlFileService;
      _applicationService = applicationService;
      _logger = logger;
    }

    public void ProcessGridRun(string fileName, string applicationCode, string applicationDesc, string grid, DateTime? startDate, DateTime? endDate, GridRunStatus gridRunStatus, bool? isDebug)
    {
      XmlFile xmlFile = _xmlFileService.GetFile(fileName);

      if (xmlFile == null)
      {
        _logger.Warn(string.Format("Unable to update file information as the file is not in the database. File name is {0}.", fileName));
      }

      GridRun gridRun = SearchForGridRunThatCameFromOneStep(fileName, applicationCode, grid, startDate)
                        ?? SearchForGridThatCameFromNTGEN95Import(applicationCode, grid);

      if (gridRun != null && xmlFile == null)
      {
        _gridRunService.Update(gridRun.Id, startDate, endDate, (int)gridRunStatus);
      }
      else if (gridRun != null)
      {
        _gridRunService.Update(gridRun.Id, startDate, endDate, (int)gridRunStatus, xmlFile.Id);
      }
      else
      {
        Application application = _applicationService.GetApplication(applicationCode);

        if (application == null)
        {
          application = _applicationService.CreateApplication(applicationCode, applicationDesc);
        }

        _gridRunService.Create(
          application.Id,
          xmlFile != null ? xmlFile.Id : (int?)null,
          (int)gridRunStatus,
          grid,
          isDebug.GetValueOrDefault(),
          startDate == null ? (DateTime?)null : startDate.Value,
          endDate == null ? (DateTime?)null : endDate.Value);
      }
    }

    private GridRun SearchForGridThatCameFromNTGEN95Import(string applicationCode, string grid)
    {
      return this._gridRunService.GetGridRun(applicationCode, grid);
    }

    private GridRun SearchForGridRunThatCameFromOneStep(string fileName, string applicationCode, string grid, DateTime? startDate)
    {
      return this._gridRunService.GetGridRun(fileName, applicationCode, grid, startDate.GetValueOrDefault());
    }
  }
}
