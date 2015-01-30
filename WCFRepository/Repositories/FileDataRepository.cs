namespace WCFRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using AbstractConfigurationManager;
  using OneStepServiceFactory.OneStepService;
  using ServiceFactory.Interfaces;
  using WCFRepository.Interfaces;

  public class FileDataRepository : IFileDataRepository
  {
    public FileDataRepository(IFileServiceFactory fileServiceFactory, IConfigurationManager configurationManager)
    {
      _fileServiceFactory = fileServiceFactory;
      _configurationManager = configurationManager;
    }

    private readonly IConfigurationManager _configurationManager;
    private readonly IFileServiceFactory _fileServiceFactory;

    public List<FileStatusData> GetFileData(DateTime syncDate)
    {
      using (var clientChannel = _fileServiceFactory.CreateChannel())
      {
        var proxy = (IFileService)clientChannel;
        return
          proxy.GetFileStatusInfo(syncDate, _configurationManager.AppSetting("companyCode"))
               .Where(t => Path.GetExtension(t.FileName) == ".xml")
               .ToList();
      }
    }
  }
}
