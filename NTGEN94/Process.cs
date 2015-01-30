namespace NTGEN94
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Xml.Linq;
  using System.Text;
  using Entities;
  using Nexdox.Composer;
  using ServiceInterfaces;

  public class Process
  {
    enum ValidMode
    {
      OffShore,
      OnShore
    };

    private ApplicationInfo _appInfo;

    private NexdoxEngine _engine;

    private IDocumentService _documentService;

    private IExportService _exportService;

    private IAppManCoEmailService _appManCoEmailService;

    private int _mode;

    public Process(ApplicationInfo appInfo)
    {
      _appInfo = appInfo;
    }

    public void Go()
    {
      NexdoxMessaging.StartEvent(this);
      Statics.Initialise(_engine, _appInfo);
      this.PerformIOC();

      GetOffShoreOrOnshore();

      IList<Document> documents = GetDocuments();

      NexdoxMessaging.SendMessage(string.Format("{0} documents found for export", documents.Count), true, this);

      if (documents.Count > 0)
      {
        var exports = new List<Export>();
        var exportsData = new List<ExportFileData>();

        GetExportData(documents, exports, exportsData);

        _exportService.CreateExport(exports);

        CreateOutputFile(exportsData);

        CreateOtfXmlFiles();

        NexdoxMessaging.SendMessage("Documents successfully set as exported", true, this);  
      }

      NexdoxMessaging.EndEvent(this);
       
    }

    private static void GetExportData(IEnumerable<Document> documents, List<Export> exports, List<ExportFileData> exportsData)
    {
      var docuemntsGroupedByAppAndGrid = from d in documents
                                         orderby d.GridRun.Grid, d.GridRun.Application.Code
                                         group d by new { d.GridRun.Grid, d.GridRun.Application.Code };

      foreach (var documentGroupedData in docuemntsGroupedByAppAndGrid)
      {
        var xportFileData = new ExportFileData
                            {
                              ApplicationCode = documentGroupedData.Key.Code, 
                              Grid = documentGroupedData.Key.Grid
                            };

        foreach (var document in documentGroupedData)
        {
          xportFileData.DocumentId.Add(document.DocumentId);

          exports.Add(new Export() { DocumentId = document.Id, ExportDate = DateTime.Now });
        }

        exportsData.Add(xportFileData);
      }
    }

    private IList<Document> GetDocuments()
    {
      IList<Document> documents;
      if (this._mode == (int)ValidMode.OffShore)
      {
        documents = this._documentService.GetApprovedAndNotExported(true);
      }
      else
      {
        documents = this._documentService.GetApprovedAndNotExported(false);
      }
      return documents;
    }

    private void GetOffShoreOrOnshore()
    {
      string inputDirectory = this._appInfo.InputPath;
      DirectoryInfo assetDirInfo = new DirectoryInfo(inputDirectory);
      FileInfo[] inputFiles = assetDirInfo.GetFiles();

      FileInfo file;

      try
      {
        file =
          (from f in inputFiles where f.Name == "OFF_SHORE.TRIGGERFILE" || f.Name == "ON_SHORE.TRIGGERFILE" select f)
            .SingleOrDefault();
      }
      catch (Exception)
      {
        throw new Exception("Could not find either an onshore or offshore trigger file ");
      }

      if (file.Name == "OFF_SHORE.TRIGGERFILE")
      {
        this._mode = (int)ValidMode.OffShore;
      }
      else if (file.Name == "ON_SHORE.TRIGGERFILE")
      {
        this._mode = (int)ValidMode.OnShore;
      }
      else
      {
        throw new Exception("Trigger file name is not valid");
      }
    }

    private void CreateOutputFile(IEnumerable<ExportFileData> exportsData)
    {
      foreach (var export in exportsData)
      {
        var CSVBuilder = new StringBuilder();
        foreach (var documentId in export.DocumentId)
        {
          CSVBuilder.Append(documentId);
          CSVBuilder.Append("\n");  
        }

        using (var unityImport = new StreamWriter(File.Create(string.Format("{0}{1}_{2}_{3}.csv", this._appInfo.OutputPath, ((ValidMode)_mode).ToString().ToUpper(), export.ApplicationCode, export.Grid))))
        {
          unityImport.WriteLine(CSVBuilder.ToString());
        }
      }
    }

    private void CreateOtfXmlFiles()
    {
      var totalTriggerList = this.GenerateTotalTriggerList();

      totalTriggerList.ForEach(x => this.GenerateTriggerXml(x));
      
    }

    private List<TriggerList> GenerateTotalTriggerList()
    {
      var appManCoEmails = this._appManCoEmailService.GetAppManCoEmails();

      var applications = GetApplications(appManCoEmails.ToList());

      var totalTriggerList = new List<TriggerList>();

      foreach (var app in applications)
      {
        var appOtfData = this.GetOtfDataByAppId(app.Id, appManCoEmails.ToList());

        var manCos = this.GetManCos(appOtfData);

          foreach (var manCo in manCos)
          {
              var manCoTriggerList = new TriggerList() { AppId = app.Code, ManCoId = manCo.Code};

              var listOtfRecords = this.GetSingleOtfRecords(manCo.Id, appOtfData);

              manCoTriggerList.Name = listOtfRecords[0].DocType;

              manCoTriggerList.Records.AddRange(listOtfRecords);

              totalTriggerList.Add(manCoTriggerList);
          }
      }

      appManCoEmails.Clear();

      return totalTriggerList;
    }

    private void GenerateTriggerXml(TriggerList triggerList)
    {
        var docName = triggerList.ManCoId + triggerList.AppId + ".OTF";
        var filePath = this._appInfo.OutputPath;

       XAttribute xTypeText = new XAttribute("type", "text");
       XAttribute xTypeEmail = new XAttribute("type", "email");
       XAttribute xTypeFax = new XAttribute("type", "fax");
       XAttribute xTypeCheckBox = new XAttribute("type", "checkbox");
       XAttribute xMultipleFalse = new XAttribute("multiple", "false");
       XAttribute xMultipleTrue = new XAttribute("multiple", "true");

       XDocument xDoc = new XDocument(
           new XDeclaration("1.0", "UTF-8", null),
           new XElement("TriggerList", new XAttribute("applID", triggerList.AppId), new XAttribute("name", GetTriggerName(triggerList.Name))));

        foreach (var record in triggerList.Records)
        {
            xDoc.Element("TriggerList")
                .Add(new XElement(
                    "Record", 
                new XElement(
                    "Section",
                    new XElement("Field", record.ManCo, new XAttribute("label", "ManagerId"), xTypeText, xMultipleFalse),
                    new XElement("Field", record.AccountNumber, new XAttribute("label", "AccountNumber"), xTypeText, xMultipleFalse),
                    new XElement("Field", record.DocType, new XAttribute("label", "DocType"), xTypeText, xMultipleFalse),
                    new XElement("Field", "ALL", new XAttribute("label", "SubDocType"), xTypeText, xMultipleFalse),
                    new XElement("Field", "true", new XAttribute("label", "TxtFile"), xTypeCheckBox, xMultipleFalse),
                    new XAttribute("label", "Main")),
                new XElement(
                    "Section",
                    new XElement("Field", new XAttribute("label", "InvestorName"), xTypeText, xMultipleFalse),
                    new XElement("Field", new XAttribute("label", "Designation"), xTypeText, xMultipleFalse), 
                    new XAttribute("label", "Contact")),
                new XElement(
                    "Section",
                    new XElement("Field", new XAttribute("label", "email1"), xTypeEmail, xMultipleTrue, record.Emails.Select(x => new XElement("Value", x))), 
                    new XAttribute("label", "EMail")),
                new XElement(
                    "Section",
                    new XElement("Field", new XAttribute("label", "fax1"), xTypeFax, xMultipleTrue),
                    new XAttribute("label", "Fax"))));
        }

        xDoc.Save(filePath + docName);
    }

    private string GetTriggerName(string docType)
    {
        string triggerName = string.Empty;

        switch (docType)
        {
            case "CNT":
               triggerName = "Contract Notes";
                break;
            case "STM":
               triggerName = "Non-Periodic Statements";
                break;
        }

        return triggerName;
    }

    private List<Application> GetApplications(List<AppManCoEmail> appManCoEmails)
    {
      return (from a in appManCoEmails select a.Application).Distinct().ToList();
    }

    private List<AppManCoEmail> GetOtfDataByAppId(int appId, List<AppManCoEmail> appManCoEmails)
    {
       return appManCoEmails.Where(x => x.ApplicationId == appId).ToList();
    }

    private List<ManCo> GetManCos(List<AppManCoEmail> appManCoEmails)
    {
       return appManCoEmails.Select(x => x.ManCo).Distinct().ToList();
    }

    private List<OtfRecord> GetSingleOtfRecords(int manCoId, List<AppManCoEmail> appManCoEmails)
    {
      var otfRecords = new List<OtfRecord>();
      var items = (from a in appManCoEmails 
                   where a.ManCo.Id == manCoId
                   select new { manCo = a.ManCo, DocType = a.DocType, AccountNumber = a.AccountNumber })
          .Distinct().ToList();

      foreach (var item in items)
      {
         var otfRecord = new OtfRecord(item.manCo.Code, item.AccountNumber, item.DocType.Code);
         appManCoEmails.Where(x => x.AccountNumber == item.AccountNumber && x.DocTypeId == item.DocType.Id && x.ManCoId == item.manCo.Id).ToList().ForEach(x => otfRecord.Emails.Add(x.Email));
         otfRecords.Add(otfRecord);
      }

      return otfRecords;
    }

    private void PerformIOC()
    {
      IoCContainer.ResoloveDependencies();

      _exportService = IoCContainer.Resolve<IExportService>();
      _documentService = IoCContainer.Resolve<IDocumentService>();
      _appManCoEmailService = IoCContainer.Resolve<IAppManCoEmailService>();
    }
  }
}
