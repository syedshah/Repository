namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class ExportService : IExportService
  {
    private readonly IExportRepository _exportRepository;

    public ExportService(IExportRepository exportRepository)
    {
      _exportRepository = exportRepository;
    }

    public void CreateExport(List<Export> exports)
    {
      try
      {
        _exportRepository.Create(exports);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to set documents as exported", e);
      }
    }
  }
}
