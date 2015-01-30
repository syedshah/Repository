namespace BusinessEngines
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using BusinessEngineInterfaces;
  using Entities;
  using Exceptions;
  using FileRepository.Interfaces;
  using ServiceInterfaces;
  using ZipManagerWrapper;

  public class ExportEngine : IExportEngine
  {
    private readonly IExportFileRepository _exportFileRepository;
    private readonly IDocumentService _documentService;
    private readonly IZipManager _zipManager;
    private string _path;

    public ExportEngine(
        IExportFileRepository exportFileRepository,
        IDocumentService documentService,
        IZipManager zipManager,
        string path)
    {
      this._documentService = documentService;
      this._exportFileRepository = exportFileRepository;
      this._zipManager = zipManager;
      _path = path;
    }

    public string SaveAsZip(List<string> documentIds)
    {
      string zipFile = string.Empty;

      var exportList = this.GetExportList(documentIds);

      exportList = this.CreateDocuments(exportList).ToList();

      zipFile = this.CreateZip(exportList);

      this.DeleteExportFiles(exportList);

      return zipFile;
    }

   private List<ExportFile> GetExportList(IEnumerable<string> documentIds)
   {
      var listFiles = new List<ExportFile>();

      foreach (var documentId in documentIds)
      {
        listFiles.Add(new ExportFile() {Id = new Guid(documentId),
        FileData = this._documentService.GetDocumentStream(documentId)
        });
      }

      return listFiles;
   }

    private IList<ExportFile> CreateDocuments(IEnumerable<ExportFile> exportFiles)
    {
      var listExportFiles = new List<ExportFile>();

      foreach (var exportFile in exportFiles)
      {
        listExportFiles.Add(this._exportFileRepository.Create(exportFile));
      }

      return listExportFiles;
    }

    private string CreateZip(IEnumerable<ExportFile> exportFiles)
    {
      try
      {
        string zipFile = string.Format("{0}/{1}/{2}.{3}", _path, "exportFiles", Guid.NewGuid().ToString(), "zip");
        var exportArray = exportFiles.ToList().Select(x => x.FileName).ToArray();
        this._zipManager.Zip(zipFile, exportArray);
        return zipFile;
      }
      catch (Exception ex)
      {
        throw new UnityException("Unable to zip files", ex);
      }
    }

    private void DeleteExportFiles(IEnumerable<ExportFile> exportFiles)
    {
      foreach (var exportFile in exportFiles)
      {
        this._exportFileRepository.Delete(exportFile.FileName);
      }
    }
  }
}
