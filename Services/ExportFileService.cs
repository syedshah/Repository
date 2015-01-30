namespace Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using BusinessEngineInterfaces;
  using Exceptions;
  using ServiceInterfaces;

  public class ExportFileService : IExportFileService
  {
    private readonly IExportEngine _exportEngine;

    public ExportFileService(IExportEngine exportEngine)
    {
      this._exportEngine = exportEngine;
    }

    public string ExportToZip(IList<string> documentIds)
    {
      try
      {
        return this._exportEngine.SaveAsZip(documentIds.ToList());
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to export documents to zip", e);
      }
    }
  }
}
