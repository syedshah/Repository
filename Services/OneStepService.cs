namespace Services
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using AbstractConfigurationManager;
  using ClientProxies.OneStepServiceReference;
  using Exceptions;
  using ServiceInterfaces;

  public class OneStepService : IOneStepService
  {
    private readonly IFileService _fileService;
    private readonly IConfigurationManager _configurationManager;

    public OneStepService(IFileService fileService, IConfigurationManager configurationManager)
    {
      _fileService = fileService;
      _configurationManager = configurationManager;
    }

    public List<FileStatusData> GetFileData(DateTime syncDate)
    {
      try
      {
        return _fileService.GetFileStatusInfo(syncDate, _configurationManager.AppSetting("companyCode"))
               .Where(t => Path.GetExtension(t.FileName) == ".xml")
               .ToList();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve one step data", e);
      }
    }
  }
}

