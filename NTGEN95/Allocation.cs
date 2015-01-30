namespace NTGEN95
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using BusinessEngineInterfaces;
  using ClientProxies.OneStepServiceReference;
  using Nexdox.Composer;
  using ServiceInterfaces;

  public class Allocation
  {
    #region Member Variables

    private readonly IDocumentService _documentService;

    private readonly IAutoApprovalService _documentApprovalService;

    private readonly ISubDocTypeService _subDocTypeService;

    private readonly IManCoService _manCoService;

    private readonly IGridRunEngine _gridRunEngine;

    private readonly IApprovalEngine _approvalEngine;

    private readonly IGridRunService _gridRunService;

    private ApplicationInfo _appInfo;

    private NexdoxEngine _engine;

    private List<string> _extractedFiles = new List<string>();

    private List<string> _xmlErrorFiles = new List<string>();

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="Allocation" /> class.
    /// </summary>
    /// <param name="engine">The engine.</param>
    /// <param name="appInfo">The application information.</param>
    /// <param name="documentService">The document service.</param>
    /// <param name="documentApprovalService">The document approval service.</param>
    /// <param name="subDocTypeService">The sub document type service.</param>
    public Allocation(
      NexdoxEngine engine,
      ApplicationInfo appInfo,
      IDocumentService documentService,
      IAutoApprovalService documentApprovalService,
      ISubDocTypeService subDocTypeService,
      IGridRunEngine gridRunEngine,
      IManCoService manCoService,
      IApprovalEngine approvalEngine,
      IGridRunService gridRunService)
    {
      _engine = engine;
      _appInfo = appInfo;
      _documentService = documentService;
      _documentApprovalService = documentApprovalService;
      _subDocTypeService = subDocTypeService;
      _gridRunEngine = gridRunEngine;
      _manCoService = manCoService;
      _approvalEngine = approvalEngine;
      _gridRunService = gridRunService;
    }

    #endregion
  
    public void Process()
    {
      string grid = _appInfo.NexdoxGlobalRunID.ToString();

      ScanForInputFiles(_appInfo.InputPath);

      if (_extractedFiles.Count > 0)
      {
        using (
          StreamWriter extractedFile =
            new StreamWriter(File.Create(_appInfo.OutputPath + "[" + grid + "]" + "-ExtractedXML.csv")))
        {
          extractedFile.WriteLine("\"FileName\"");

          foreach (string file in _extractedFiles) extractedFile.WriteLine(file);
        }
      }
    }

    private void ScanForInputFiles(string directory)
    {
      string rootDir = directory;

      foreach (string xmlFile in Directory.GetFiles(directory, _appInfo["XmlFileMask"], SearchOption.TopDirectoryOnly))
      {
        _extractedFiles.Add(Path.GetFileName(xmlFile));
   
        ProcessXMLFile(xmlFile);
      }
    }

    private void ProcessXMLFile(string dataFile)
    {
      NexdoxMessaging.SendMessage(string.Format("Processing file {0}", dataFile), true, this);

      XmlDataExtractor xmlDataExtractor = new XmlDataExtractor();

      var grid = xmlDataExtractor.GetGrid(dataFile);

      var extractedDocuments = xmlDataExtractor.GetDocument(dataFile);

      string applicationCode = extractedDocuments.First().Application;

      _gridRunEngine.ProcessGridRun(
        string.Empty,
        applicationCode,
        string.Empty,
        grid,
        null,
        null,
        GridRunStatus.Undefined,
        null);

      var gridRun = _gridRunService.GetGridRun(applicationCode, grid);

      NexdoxMessaging.SendMessage(string.Format("Grid processed"), true, this);

      foreach (var document in extractedDocuments)
      {
        var subDocTypeData = this.GetSubDocTypeData(document.SubDocType);
        var manCoData = this.GetManCoData(document.ManCoCode);
        
        var autoApproval = this._documentApprovalService.GetAutoApproval(manCoData.ManCoId, subDocTypeData.DocTypeId, subDocTypeData.SubDocTypeId);


        SaveDocumentToDatabase(document, subDocTypeData.DocTypeId, subDocTypeData.SubDocTypeId, manCoData.ManCoId, gridRun.Id);

        if (autoApproval != null)
        {
          NexdoxMessaging.SendMessage(string.Format("Document {0} automatically approved", document.DocumentId), true, this);

          _approvalEngine.AutoApproveDocument(document.DocumentId);  
        }
        else
        {
          NexdoxMessaging.SendMessage(string.Format("Document {0} requires manual approval approved", document.DocumentId), true, this);
        }
      }
    }

    private void SaveDocumentToDatabase(ExtractedDocument extractedDocument, int docTypeId, int subDocTypeId, int manCoId, int gridRunId)
    {
      _documentService.AddDocument(
        extractedDocument.DocumentId,
        docTypeId,
        subDocTypeId,
        manCoId,
        gridRunId,
        extractedDocument.MailPrintFlag);

      NexdoxMessaging.SendMessage(string.Format("Document {0} saved to database", extractedDocument.DocumentId), true, this);
    }

    private SubDocTypeData GetSubDocTypeData(string subDocTypeCode)
    {
      var subDocType = this._subDocTypeService.GetSubDocType(subDocTypeCode);

      if (subDocType == null)
      {
        throw new Exception(string.Format("Sub Doc Type {0} does not exist in Unity database", subDocTypeCode));
      }

      return new SubDocTypeData { DocTypeId = subDocType.DocType.Id, SubDocTypeId = subDocType.Id };
    }

    private ManCoData GetManCoData(string manCoCode)
    {
      var manCo = this._manCoService.GetManCo(manCoCode);

      if (manCo == null)
      {
        throw new Exception(string.Format("Man Co {0} does not exist in Unity database", manCoCode));
      }

      return new ManCoData { ManCoId = manCo.Id };
    }

    private void OutputErrorRecord(string fileName, string error)
    {
      _xmlErrorFiles.Add("\"" + fileName + "\",\"" + error + "\"");
    }
  }
}
