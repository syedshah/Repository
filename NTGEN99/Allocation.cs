using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexdox.Composer;
using System.IO;
using Xceed.Zip;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;

namespace ntgen99
{
  using NTGEN00;

  using ServiceInterfaces;

  public class Allocation
  {
    #region Member Variables

    private ApplicationInfo _appInfo;

    private NexdoxEngine _engine;

    private readonly IConFileService _conFileService;

    private readonly IXmlFileService _xmlFileService;

    private readonly IZipFileService _zipFileService;

    private readonly IDocTypeService _docTypeService;

    private readonly IManCoService _manCoService;

    private List<string> _extractedFiles = new List<string>();

    private List<string> _allocatedFiles = new List<string>();

    private List<string> _ignoreFiles = new List<string>();

    private List<string> _xmlErrorFiles = new List<string>();

    private List<string> _zipErrorFiles = new List<string>();

    private List<string> _pdfErrorFiles = new List<string>();

    private List<string> _transferFiles = new List<string>();

    private List<string> _contractFiles = new List<string>();

    private List<string> _contractCWRCWSFiles = new List<string>();

    private List<string> _contractSWRSWSFiles = new List<string>();

    //DV NT Re-eng Phase 1 - create archive csv file
    private List<string> _archiveCSV = new List<string>();

    private string _transferFileDestinationDirectory = string.Empty;

    private string _contractSWRSWSFileDestinationDirectory = string.Empty;

    private string _contractFileDestinationDirectory = string.Empty;

    private string _contractCWRCWSFileDestinationDirectory = string.Empty;

    #endregion

    #region Properties

    public string _zipFileName { get; set; }

    public string _guid { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Default Constructor.
    /// </summary>
    /// <param name="engine">The nexdox engine containing all the streams.</param>
    /// <param name="appInfo">Application Info created by NexdoxLaunch and passed to the program on declaration.</param>
    public Allocation(
      NexdoxEngine engine,
      ApplicationInfo appInfo,
      IConFileService conFileService,
      IXmlFileService xmlFileService,
      IZipFileService zipFileService,
      IDocTypeService docTypeService,
      IManCoService manCoService)
    {
      this._engine = engine;
      this._appInfo = appInfo;
      _conFileService = conFileService;
      _xmlFileService = xmlFileService;
      _zipFileService = zipFileService;
      _docTypeService = docTypeService;
      _manCoService = manCoService;
    }

    #endregion

    #region Methods

    public void Process()
    {
      NexdoxMessaging.SendMessage("Allocation Stage...", true, this);

      //Todo: Do some kind of sequence checking... there may be a better place to do this than here.

      //Make sure Xceed libraries are enabled
      Xceed.Zip.Licenser.LicenseKey = "ZIN37-T1W1B-SW87P-N8AA";

      string grid = _appInfo.NexdoxGlobalRunID.ToString();

      //Get the input files
      ScanForInputFiles(_appInfo.InputPath);

      //Now add each level of Zip structure to the class
      NexdoxMessaging.SendMessage("Deconstructing Zip Package....", true, this);

      //Top Level first (Big Zip)
      foreach (string zipFile in Directory.GetFiles(_appInfo.InputPath, "*", SearchOption.TopDirectoryOnly))
      {
        ZipPackage zpBig = new ZipPackage()
                             {
                               FileName = Path.GetFileName(zipFile),
                               ParentZipFileName = "",
                               IsBigZip = true,
                               InputCreationDate = new FileInfo(zipFile).LastWriteTime,
                               StatusID = 0,
                             };

        Statics.zipPackage.Add(zpBig);

        string pathLittleZip = _appInfo.InputPath + Path.ChangeExtension(zpBig.FileName, "");

        //Next Level. (Little Zip)
        foreach (string littlezipFile in Directory.GetFiles(pathLittleZip, "*", SearchOption.TopDirectoryOnly))
        {
          ZipPackage zpLittle = new ZipPackage()
                                  {
                                    FileName = Path.GetFileName(littlezipFile.ToUpper()),
                                    ParentZipFileName = zpBig.FileName,
                                    IsLittleZip = true,
                                    InputCreationDate = new FileInfo(littlezipFile).LastWriteTime,
                                    StatusID = 0,
                                  };
          Statics.zipPackage.Add(zpLittle);

          string pathDataFiles = pathLittleZip + "\\" + Path.ChangeExtension(zpLittle.FileName, "");

          //Now the Data Files
          foreach (string dataFile in Directory.GetFiles(pathDataFiles, "*", SearchOption.TopDirectoryOnly))
          {
            if (Path.GetExtension(dataFile) == ".XML")
            {
              foreach (ZipPackage zp in Statics.zipPackage)
              {
                if (zp.FileName == Path.GetFileName(dataFile))
                {
                  zp.ParentZipFileName = zpLittle.FileName;
                  zp.InputCreationDate = new FileInfo(littlezipFile).LastWriteTime;
                }
              }
            }
            else //.CON File
            {
              ZipPackage zpData = new ZipPackage()
                                    {
                                      FileName = Path.GetFileName(dataFile),
                                      ParentZipFileName = zpLittle.FileName,
                                      InputCreationDate = new FileInfo(littlezipFile).LastWriteTime,
                                      StatusID = 0,
                                    };
              Statics.zipPackage.Add(zpData);
            }
          }
        }
      }

      Statics.zipPackage.SaveToDatabase(_xmlFileService, _zipFileService, _conFileService, _docTypeService, _manCoService, _appInfo);
      Statics.zipPackage.SaveToOutputDir(_appInfo.OutputPath, grid);

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

      if (_allocatedFiles.Count > 0)
      {
        using (
          StreamWriter allocatedFile =
            new StreamWriter(File.Create(_appInfo.OutputPath + "[" + grid + "]" + "-AllocatedXML.csv")))
        {
          allocatedFile.WriteLine("\"FileName\",\"Application\"");

          foreach (string file in _allocatedFiles) allocatedFile.WriteLine(file);
        }
      }

      if (_zipErrorFiles.Count > 0)
      {
        using (
          StreamWriter zipErrorFile =
            new StreamWriter(File.Create(_appInfo.OutputPath + "[" + grid + "]" + "-FailedZIP.csv")))
        {
          zipErrorFile.WriteLine("\"FileName\",\"Error\"");

          foreach (string error in _zipErrorFiles) zipErrorFile.WriteLine(error);
        }
      }

      if (_pdfErrorFiles.Count > 0)
      {
        using (
          StreamWriter pdfErrorFile =
            new StreamWriter(File.Create(_appInfo.OutputPath + "[" + grid + "]" + "-FailedPDF.csv")))
        {
          pdfErrorFile.WriteLine("\"FileName\",\"Error\"");

          foreach (string error in _pdfErrorFiles) pdfErrorFile.WriteLine(error);
        }
      }

      if (_xmlErrorFiles.Count > 0)
      {
        using (
          StreamWriter xmlErrorFile =
            new StreamWriter(File.Create(_appInfo.OutputPath + "[" + grid + "]" + "-FailedXML.csv")))
        {
          xmlErrorFile.WriteLine("\"FileName\",\"Error\"");

          foreach (string error in _xmlErrorFiles) xmlErrorFile.WriteLine(error);
        }
      }

      if (_ignoreFiles.Count > 0)
      {
        using (
          StreamWriter ignoredFile =
            new StreamWriter(File.Create(_appInfo.OutputPath + "[" + grid + "]" + "-ignoredXML.csv")))
        {
          ignoredFile.WriteLine("\"FileName\",\"Application\"");

          foreach (string ignore in _ignoreFiles) ignoredFile.WriteLine(ignore);
        }
      }

      NexdoxMessaging.SendMessage("Finished Allocation Stage.", true, this);
    }

    /// <summary>
    /// Scan directory for zips, pdfs & xmls
    /// </summary>
    /// <param name="directory">Path of the directory to scan</param>
    private void ScanForInputFiles(string directory)
    {
      string rootDir = directory;
      //Search for and unzip any zip files
      foreach (string zipFile in Directory.GetFiles(directory, _appInfo["ZipFileMask"], SearchOption.TopDirectoryOnly))
      {

        try
        {
          UnzipFile(zipFile);
        }
        catch (Exception e)
        {
          _zipErrorFiles.Add("\"" + Path.GetFileName(zipFile) + "\",\"" + e.Message + "\",\"" + e.StackTrace + "\"");
        }

      }

      //Search for and process any pdf files
      foreach (string pdfFile in Directory.GetFiles(directory, _appInfo["PdfFileMask"], SearchOption.TopDirectoryOnly))
      {
        try
        {
          ProcessInserts(pdfFile);
        }
        catch (Exception e)
        {
          _pdfErrorFiles.Add("\"" + Path.GetFileName(pdfFile) + "\",\"" + e.Message + "\",\"" + e.StackTrace + "\"");
        }
      }

      ////Search for and process any xml files
      foreach (string xmlFile in Directory.GetFiles(directory, _appInfo["XmlFileMask"], SearchOption.TopDirectoryOnly))
      {
        _extractedFiles.Add(Path.GetFileName(xmlFile));

        try
        {
          ProcessXMLFile(xmlFile);
        }
        catch (Exception e)
        {
          OutputErrorRecord(Path.GetFileName(xmlFile), e.Message + "\",\"" + e.StackTrace);
        }
      }

      DirectoryInfo dir = new DirectoryInfo(directory);

      //create trigger files if necessary
      if (_transferFiles.Count > 0) CreateTriggerFileForTransfers(dir.Name);

      if (_contractFiles.Count > 0) CreateTriggerFileForContractNotes(dir.Name);

      if (_contractCWRCWSFiles.Count > 0) CreateTriggerFilesForCwrCws(dir.Name);

      if (_contractSWRSWSFiles.Count > 0) CreateTriggerFilesForSwrSws(dir.Name);
    }

    /// <summary>
    /// Process the XML input files
    /// </summary>
    private void ProcessXMLFile(string dataFile)
    {
      ZipPackage zp = new ZipPackage();
      zp.FileName = Path.GetFileName(dataFile);

      string[] parts = dataFile.Split('.');
      string documentIdentifier = parts[3] + parts[4];

      NexdoxMessaging.SendMessage("    Processing xml file " + Path.GetFileName(dataFile) + "...", true, this);

      if (!File.Exists(dataFile.ToUpper().Replace(".XML", ".job")))
      {
        OutputErrorRecord(Path.GetFileName(dataFile), "Corresponding .job file does not exist");
        return;
      }

      StreamReader jobInputFileStream = new StreamReader(dataFile.ToUpper().Replace(".XML", ".job"));
      XmlReaderSettings jobSettings = new XmlReaderSettings();
      jobSettings.XmlResolver = null;
      XmlReader jobData = XmlReader.Create(jobInputFileStream, jobSettings);

      string domicile = string.Empty;
      string batchNumber = string.Empty;

      jobData.ReadToFollowing("Domicile");
      domicile = jobData.ReadElementContentAsString().ToUpper();
      zp.Domicile = domicile;

      string managerIdString = string.Empty;

      try
      {
        jobData.ReadToFollowing("Manager_ID");
      }
      catch (Exception e)
      {
        OutputErrorRecord(Path.GetFileName(dataFile), e.Message + "\",\"" + e.StackTrace);
        jobData.Close();
        jobInputFileStream.Close();
        return;
      }

      if (jobData.EOF)
      {
        OutputErrorRecord(Path.GetFileName(dataFile), "The job file is does not contain Manager_ID");
        jobData.Close();
        jobInputFileStream.Close();
        return;
      }
      else
      {
        managerIdString = jobData.ReadElementContentAsString();
        int managerId;
        if (!int.TryParse(managerIdString, out managerId)) 
        {
          OutputErrorRecord(Path.GetFileName(dataFile), "Unknown Manager ID: " + managerIdString);
        }
        zp.ManCoID = managerIdString;

        if (Statics.ManagementCompanySettings.Exists(delegate(NTGEN00.ManCoSettings manCoSettings){ return manCoSettings.ManagementCompanyID == managerId; }))
        {
          NTGEN00.ManCoSettings manCoSettings = Statics.ManagementCompanySettings[managerId];

          Statics.zipPackage.Add(zp);

          //Open data file as xml
          StreamReader inputFileStream = new StreamReader(dataFile);
          XmlReaderSettings settings = new XmlReaderSettings();
          settings.XmlResolver = null;
          XmlReader xmlData = XmlReader.Create(inputFileStream, settings);

          //Get file name minus extension
          string fileNameNoExtension = new FileInfo(dataFile).Name;
          fileNameNoExtension = fileNameNoExtension.Substring(0, fileNameNoExtension.Length - 4);

          //Can we tell what application it's supposed to go to by doc type
          string destinationDirectory = string.Empty;
          string appName = string.Empty;
          string docType = string.Empty;
          string subDocType = string.Empty;


          //Get document type
          xmlData.ReadToFollowing("Doc_Type");

          if (xmlData.EOF)
          {
            destinationDirectory = _appInfo.OutputPath + "InvalidFiles";
            OutputErrorRecord(Path.GetFileName(dataFile), "No Doc_Type in data file");
            return;
          }
          else
          {
            docType = xmlData.ReadElementContentAsString().ToUpper();
            zp.DocumentType = docType;
            xmlData.ReadToFollowing("Doc_Sub_Type");
            subDocType = xmlData.EOF ? "N/A" : xmlData.ReadElementContentAsString();

            jobData.Close();
            jobInputFileStream.Close();

            string subDocAndDocType = docType + subDocType;
            if (Statics.Resources.Lookups[LookupEnums.MonitorDirectory].Lookup.Contains(subDocAndDocType))
            {
              appName = Statics.Resources.Lookups[LookupEnums.AppName].Lookup.Find(subDocAndDocType);
              destinationDirectory =
                Statics.Resources.Lookups[LookupEnums.MonitorDirectory].Lookup.Find(subDocAndDocType);
            }
            else
            {
              destinationDirectory = _appInfo.OutputPath + "InvalidFiles";
              OutputErrorRecord(
                Path.GetFileName(dataFile),
                string.Format("Doc_Type ({0}) and Sub_Doc_Type ({1}) unknown", docType, subDocType));
              return;
            }
          }



          if (string.Compare(appName, "ntgen03", true) == 0)
          {
            if (manCoSettings.IsEMEA(appName) || manCoSettings.Offshore)
            {
              appName = "ntgen23";
              destinationDirectory = @"\\127.0.0.1\nexdox\ntgen23\monitor";
            }
          }
          else if (string.Compare(appName, "ntgen01", true) == 0)
          {
            switch (Statics.CntFilesRoutingMode)
            {
              case "PremierSeparately":
                if (!manCoSettings.Offshore)
                {
                  appName = (manCoSettings.ManagementCompanyID == 14) || (manCoSettings.ManagementCompanyID == 17)
                              ? "ntprm01"
                              : "ntgen01";
                }
                else
                {
                  appName = "ntgen51";
                }
                break;

              case "PremierAsOnshore":
                appName = manCoSettings.Offshore ? "ntgen51" : "ntgen01";
                break;

              default:
                break;
            }

            destinationDirectory = string.Format(@"\\127.0.0.1\nexdox\{0}\monitor", appName);
          }

          if (!manCoSettings.ApplicationsSwitchedOff.Contains(appName))
          {
            //Make sure destination directory string ends in a \
            if (!destinationDirectory.EndsWith("\\")) destinationDirectory += "\\";

            //Move job file to application input directory
            NexdoxMessaging.SendMessage(
              string.Format("  Allocating '{0}' files to '{1}' directory.", fileNameNoExtension, destinationDirectory),
              false,
              this);

            if (!File.Exists(string.Format("{0}.job", dataFile.Substring(0, dataFile.Length - 4))))
            {
              OutputErrorRecord(Path.GetFileName(dataFile), "No job file supplied with xml file");
              return;
            }
            else if (!Statics.AppsToIgnore.Contains(appName.ToUpper()))
            {
              if (!Directory.Exists(destinationDirectory)) Directory.CreateDirectory(destinationDirectory);

              //Move data file to application input directory
              MoveFile(
                string.Format("{0}.job", dataFile.Substring(0, dataFile.Length - 4)),
                string.Format("{0}{1}.job", destinationDirectory, fileNameNoExtension));
              MoveFile(
                string.Format("{0}.xml", dataFile.Substring(0, dataFile.Length - 4)),
                string.Format("{0}{1}.xml", destinationDirectory, fileNameNoExtension));

              _allocatedFiles.Add("\"" + Path.GetFileName(dataFile) + "\",\"" + appName + "\"");


              zp.Offshore = manCoSettings.Offshore ? true : false;
              zp.StatusID = 1;


              if (string.Compare(docType, "CNF", true) == 0)
              {
                //All input for NXNOR07 should be processed together
                _transferFiles.Add(fileNameNoExtension);
                _transferFileDestinationDirectory = destinationDirectory;
              }
              else if (string.Compare(docType, "CNT", true) == 0)
              {
                if (string.Compare(subDocType, "SWS", true) == 0 || string.Compare(subDocType, "SWR", true) == 0)
                {
                  _contractSWRSWSFiles.Add(fileNameNoExtension);
                  _contractSWRSWSFileDestinationDirectory = destinationDirectory;
                }
                else if (string.Compare(subDocType, "CWR", true) == 0 || string.Compare(subDocType, "CWS", true) == 0)
                {
                  _contractCWRCWSFiles.Add(fileNameNoExtension);
                  _contractCWRCWSFileDestinationDirectory = destinationDirectory;
                }
                else
                {
                  _contractFiles.Add(fileNameNoExtension);
                  _contractFileDestinationDirectory = destinationDirectory;
                }
              }
            }
            else
            {
              //ignore these files
              _ignoreFiles.Add("\"" + Path.GetFileName(dataFile) + "\",\"" + appName + "\"");
            }
          }
          else
          {
            OutputErrorRecord(
              Path.GetFileName(dataFile), "Application: " + appName + " switched off for ManCo ID: " + managerIdString);
            return;
          }
        }
        else
        {
          OutputErrorRecord(Path.GetFileName(dataFile), "Unknown ManCo ID: " + managerIdString);
          return;
        }
      }
    }

    private void CreateTriggerFilesForSwrSws(string triggerName)
    {
      NexdoxMessaging.SendMessage("    Creating Contract SWR+SWS Trigger File trigger.cnf", true, this);

      StreamWriter triggerFile = new StreamWriter(_contractSWRSWSFileDestinationDirectory + triggerName + ".cnf");

      foreach (string fileName in _contractSWRSWSFiles) triggerFile.WriteLine(fileName);

      triggerFile.Close();
      _contractSWRSWSFiles.Clear();
    }

    private void CreateTriggerFilesForCwrCws(string triggerName)
    {
      NexdoxMessaging.SendMessage("    Creating Contract CWR+CWS Trigger File trigger.cnf", true, this);

      StreamWriter triggerFile = new StreamWriter(_contractCWRCWSFileDestinationDirectory + triggerName + ".cnf");

      foreach (string fileName in _contractCWRCWSFiles) triggerFile.WriteLine(fileName);

      triggerFile.Close();
      _contractCWRCWSFiles.Clear();
    }

    private void CreateTriggerFileForContractNotes(string triggerName)
    {
      NexdoxMessaging.SendMessage("    Creating Contract Trigger File(s) trigger_*.cnf", true, this);

      int contractCount = 0;

      foreach (string fileName in _contractFiles)
      {
        //need to generate a trigger file for each xml that is not SWS/SWR/CWS/CWR
        using (
          StreamWriter triggerFile =
            new StreamWriter(
              string.Format("{0}{1}_{2}.cnf", _contractFileDestinationDirectory, triggerName, contractCount)))
        {
          triggerFile.WriteLine(fileName);
        }

        contractCount++;
      }
      _contractFiles.Clear();
    }

    private void CreateTriggerFileForTransfers(string triggerName)
    {
      NexdoxMessaging.SendMessage("    Creating Transfers Trigger File trigger.cnf", true, this);

      StreamWriter triggerFile = new StreamWriter(_transferFileDestinationDirectory + triggerName + ".cnf");

      foreach (string transferFile in _transferFiles) triggerFile.WriteLine(transferFile);

      triggerFile.Close();
      _transferFiles.Clear();
    }

    private void MoveFile(string from, string to)
    {
      NexdoxMessaging.SendMessage(string.Format("  Moving - {0} to {1}", from, to), false, this);

      if (File.Exists(to) && Statics.OverwriteDataFiles) File.Delete(to);

      File.Copy(from, to);
    }

    private void OutputErrorRecord(string fileName, string error)
    {
      _xmlErrorFiles.Add("\"" + fileName + "\",\"" + error + "\"");
    }

    /////// <summary>
    /////// Checks to see if any of the XML files are over the size threshold and attempts to splirt them into smaller files
    /////// </summary>
    ////private void SplitHugeDataFiles()
    ////{
    ////  int hugeFileSplitThreshold = Int32.Parse(_appInfo["HugeFileSplitThreshold"]) * 1000;

    ////  foreach (string inputFile in Directory.GetFiles(_appInfo.OutputPath, _appInfo["XmlFileMask"]))
    ////  {
    ////    FileInfo fi = new FileInfo(inputFile);

    ////    if (fi.Length < hugeFileSplitThreshold)
    ////      continue;

    ////    var documents = XElement.Load(inputFile).Elements("Document_Data");

    ////    string docSubType = documents.First().Element("Doc").Element("Doc_Sub_Type").Value;

    ////    if (!(new[] { "PER", "AGT" }).Contains(docSubType))
    ////      continue;

    ////    int splitCount = (int)(fi.Length / hugeFileSplitThreshold) + 1;

    ////    var consolidatedDocuments = documents.GroupBy(d => d.Element("Addressee").Element("Id").Value).OrderBy(g => g.Count()).Reverse();

    ////    int documentsPerChunk = (int)(documents.Count() / splitCount);

    ////    int counter = 0;

    ////    string mainPartOfInitialFileNames = Path.ChangeExtension(inputFile, null);

    ////    XElement initialJobFile = XElement.Load(mainPartOfInitialFileNames + ".job");

    ////    List<XElement> tempDocuments = new List<XElement>();

    ////    foreach (var documentsForOneAddressee in consolidatedDocuments)
    ////    {
    ////      if (tempDocuments.Count + documentsForOneAddressee.Count() < documentsPerChunk)
    ////      {
    ////        tempDocuments.AddRange(documentsForOneAddressee);
    ////      }
    ////      else
    ////      {
    ////        if (tempDocuments.Count() == 0) // Is still too large, but we had no possibility to change this
    ////        {
    ////          tempDocuments = documentsForOneAddressee.ToList();
    ////          // Process tempDocuments and clear tempDocuments
    ////          SaveSmallerXMLChunks(tempDocuments, initialJobFile, mainPartOfInitialFileNames, counter);
    ////          tempDocuments.Clear();
    ////        }
    ////        else
    ////        {
    ////          // Process tempDocuments, and tail should be saved
    ////          SaveSmallerXMLChunks(tempDocuments, initialJobFile, mainPartOfInitialFileNames, counter);
    ////          tempDocuments = documentsForOneAddressee.ToList();
    ////        }

    ////        counter++;
    ////      }
    ////    }

    ////    SaveSmallerXMLChunks(tempDocuments, initialJobFile, mainPartOfInitialFileNames, counter);

    ////    File.Move(mainPartOfInitialFileNames + ".xml", mainPartOfInitialFileNames + ".xml.huge");
    ////    File.Move(mainPartOfInitialFileNames + ".job", mainPartOfInitialFileNames + ".job.huge");
    ////  }
    ////}

    /////// <summary>
    /////// saves chunks of xml as smaller input files
    /////// </summary>
    /////// <param name="documents">List of documents that makeup our new file</param>
    /////// <param name="initialJobFile">The initial XML file</param>
    /////// <param name="mainPartOfInitialFileNames">Filename</param>
    /////// <param name="counter">Current File Chunk Number</param>
    ////private void SaveSmallerXMLChunks(List<XElement> documents, XElement initialJobFile, string mainPartOfInitialFileNames, int counter)
    ////{
    ////  string newMainpartOfName = string.Format("{0}.{1:d3}", mainPartOfInitialFileNames, counter);

    ////  XElement newDocumentSet = new XElement("Document_Pack", documents);

    ////  initialJobFile.Element("Rec_Total").Element("count").Value = documents.Count().ToString();

    ////  newDocumentSet.Save(newMainpartOfName + ".xml");
    ////  initialJobFile.Save(newMainpartOfName + ".job");
    ////}

    /// <summary>
    /// Updates any new PDFs supplied into the shared resources folder
    /// </summary>
    private void ProcessInserts(string pdfFile)
    {
      //Get filename of the insert
      string pdfFileName = Path.GetFileName(pdfFile);

      string path = pdfFile.Replace(pdfFileName, "");

      NexdoxMessaging.SendMessage("    Processing pdf file " + pdfFileName + "...", true, this);

      try
      {
        string pdfInsert = pdfFile;
        string epsInsert = pdfInsert.Substring(0, pdfInsert.LastIndexOf("."));

        System.Diagnostics.Process process;
        ProcessStartInfo processInfo = new ProcessStartInfo(
          _appInfo["PDFConversionAppPath"] + @"\pdf2vec.exe", "\"" + pdfInsert + "\" \"" + epsInsert + ".eps\"");
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardOutput = true;
        process = System.Diagnostics.Process.Start(processInfo);
        process.WaitForExit(10000); //10 seconds?

        using (StreamWriter writer = new StreamWriter(_appInfo.OutputPath + "test.txt"))
        {
          string line;

          while ((line = process.StandardOutput.ReadLine()) != null) writer.WriteLine(line);
        }

        int exitCode = process.ExitCode;
        process.Close();

      }
      catch (Exception e)
      {
        NexdoxMessaging.SendMessage("ERROR - When converting eps file - " + e.Message, false, null);
      }

      //Ok, now we need to make sure that there is no showpage. Otherwise our output will create
      //an additional page. blast!

      NexdoxResourceManager.ImageList allImages =
        Statics.CentralResources.Images.FindAllMatchingNames(Path.GetFileNameWithoutExtension(pdfFile));
      NexdoxResourceManager.ImageList updatedImages = new NexdoxResourceManager.ImageList();

      foreach (string file in Directory.GetFiles(path, Path.GetFileNameWithoutExtension(pdfFile) + "*.eps"))
      {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
        string newFileNameWithoutExtension = fileNameWithoutExtension.Replace("-", "_").Replace(" ", "_");
        File.Move(path + fileNameWithoutExtension + ".eps", path + newFileNameWithoutExtension + ".temp");
        using (StreamWriter writer = new StreamWriter(path + newFileNameWithoutExtension + ".eps", false))
        {
          using (StreamReader reader = new StreamReader(path + newFileNameWithoutExtension + ".temp"))
          {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
              if (line.Contains("showpage") || line.Contains("verydoc")) continue;

              writer.WriteLine(line);
            }
          }
        }

        NexdoxResourceManager.ImageList images =
          Statics.CentralResources.Images.FindAllMatchingNames(newFileNameWithoutExtension);

        if (images.Count > 1)
        {
          bool imageUploaded = false;
          foreach (NexdoxResourceManager.ImageResource image in images)
          {
            if (string.Compare(image.BaseName, newFileNameWithoutExtension, true) == 0)
            {
              UpdateImageInDAM(path, updatedImages, newFileNameWithoutExtension, image);
              imageUploaded = true;
            }
          }

          if (!imageUploaded) AddNewImageToDAM(path, updatedImages, newFileNameWithoutExtension);
        }
        else if (images.Count == 0)
        {
          AddNewImageToDAM(path, updatedImages, newFileNameWithoutExtension);
        }
        else
        {
          UpdateImageInDAM(path, updatedImages, newFileNameWithoutExtension, images[0]);
        }

        File.Delete(newFileNameWithoutExtension + ".temp");
      }

      string errMsg = String.Empty;
      // DV - I'm not sure why this bit is here, it looks to duplicate what happens above, seemingly just 
      // here to catch any issues

      //chekout RMC
      foreach (NexdoxResourceManager.ImageResource image in allImages)
      {
        if (!updatedImages.Contains(image))
        {
          NexdoxResourceManager.CheckOutResult result = image.ParentResourceManager.ResourceManagerAsset.CheckOut();
          if (result != NexdoxResourceManager.CheckOutResult.Success)
          {
            throw NexdoxMessaging.Exception(
              "Error checking out parent resource: " + image.ParentResourceManager.ResourceManagerAsset.Name + ". "
              + result.ToString(),
              this);
          }

          result = image.CheckOut();

          if (result != NexdoxResourceManager.CheckOutResult.Success)
          {
            throw NexdoxMessaging.Exception(
              "Error checking out image resource: " + image.Name + ". " + result.ToString(), this);
          }

          image.Deleted = true;
          image.Save("", false);
          image.CheckIn();
          image.ParentResourceManager.ResourceManagerAsset.Save("", false);
          image.ParentResourceManager.ResourceManagerAsset.CheckIn();
          if (string.Compare(_appInfo["Region"], "Live", true) == 0)
          {
            image.SetCurrentRegionVersion("Live", image.VersionNo);
            image.SetCurrentRegionVersion("Test", image.VersionNo);
            image.ParentResourceManager.ResourceManagerAsset.SetCurrentRegionVersion(
              "Live", image.ParentResourceManager.ResourceManagerAsset.VersionNo);
            image.ParentResourceManager.ResourceManagerAsset.SetCurrentRegionVersion(
              "Test", image.ParentResourceManager.ResourceManagerAsset.VersionNo);

          }
          else if (string.Compare(_appInfo["Region"], "Test", true) == 0)
          {
            image.SetCurrentRegionVersion("Test", image.VersionNo);
            image.ParentResourceManager.ResourceManagerAsset.SetCurrentRegionVersion(
              "Test", image.ParentResourceManager.ResourceManagerAsset.VersionNo);
          }
          //Always into Dev
          image.SetCurrentRegionVersion("Dev", image.VersionNo);
          image.ParentResourceManager.ResourceManagerAsset.SetCurrentRegionVersion(
            "Dev", image.ParentResourceManager.ResourceManagerAsset.VersionNo);
        }
      }
    }

    private void UpdateImageInDAM(
      string path,
      NexdoxResourceManager.ImageList updatedImages,
      string fileNameWithoutExtension,
      NexdoxResourceManager.ImageResource image)
    {
      NexdoxResourceManager.ImageResource imageResource = Statics.CentralResources.Images[image.Name];
      imageResource.CheckOut();
      imageResource.UploadImageFile(path + fileNameWithoutExtension + ".eps", false, false);
      imageResource.Save("", false);
      imageResource.CheckIn();
      if (string.Compare(_appInfo["Region"], "Live", true) == 0)
      {
        imageResource.SetCurrentRegionVersion("Live", imageResource.VersionNo);
        imageResource.SetCurrentRegionVersion("Test", imageResource.VersionNo);
      }
      else if (string.Compare(_appInfo["Region"], "Test", true) == 0) imageResource.SetCurrentRegionVersion("Test", imageResource.VersionNo);
      //Always into Dev
      imageResource.SetCurrentRegionVersion("Dev", imageResource.VersionNo);
      updatedImages.Add(image);
    }

    private void AddNewImageToDAM(
      string path, NexdoxResourceManager.ImageList updatedImages, string fileNameWithoutExtension)
    {
      Statics.CentralResources.DisconnectFromDam();
      NexdoxResourceManager.ImageResource newResource = new NexdoxResourceManager.ImageResource();
      newResource.ParentResourceManager = Statics.CentralResources;
      newResource.Name = fileNameWithoutExtension;
      newResource.UploadImageFile(path + fileNameWithoutExtension + ".eps", true, false);
      Statics.CentralResources.ConnectToDam();
      newResource.Save("", true);
      newResource.CheckIn();
      Statics.CentralResources.ResourceManagerAsset.CheckOut();
      Statics.CentralResources.Images.Add(newResource);
      Statics.CentralResources.ResourceManagerAsset.Save("", false);
      Statics.CentralResources.ResourceManagerAsset.CheckIn();
      if (string.Compare(_appInfo["Region"], "Live", true) == 0)
      {
        newResource.SetCurrentRegionVersion("Live", 0);
        newResource.SetCurrentRegionVersion("Test", 0);
        Statics.CentralResources.ResourceManagerAsset.SetCurrentRegionVersion(
          "Live", Statics.CentralResources.ResourceManagerAsset.VersionNo);
        Statics.CentralResources.ResourceManagerAsset.SetCurrentRegionVersion(
          "Test", Statics.CentralResources.ResourceManagerAsset.VersionNo);
      }
      else if (string.Compare(_appInfo["Region"], "Test", true) == 0)
      {
        newResource.SetCurrentRegionVersion("Test", 0);
        Statics.CentralResources.ResourceManagerAsset.SetCurrentRegionVersion(
          "Test", Statics.CentralResources.ResourceManagerAsset.VersionNo);
      }
      //Always into Dev
      newResource.SetCurrentRegionVersion("Dev", 0);
      Statics.CentralResources.ResourceManagerAsset.SetCurrentRegionVersion(
        "Dev", Statics.CentralResources.ResourceManagerAsset.VersionNo);
      updatedImages.Add(newResource);
    }

    /// <summary>
    /// Unzip a file and any zip files contained within it.
    /// </summary>
    /// <param name="zipFile">path of zipfile</param>
    /// <returns>true if file successfully unzipped, false if it failed</returns>
    private bool UnzipFile(string zipFile)
    {
      try
      {
        //Unzip the zip file
        string zipFileName = Path.GetFileName(zipFile);

        //DV NT Re-eng Phase 1 set as a public for use elsewhere
        _zipFileName = zipFileName;
        // create a public guid for the big zip file - not working as this is called for each little zip
        _guid = Guid.NewGuid().ToString();

        string zipFilePath = zipFile.Replace(zipFileName, "");
        string zipFileSubDir = zipFilePath + zipFileName.Substring(0, zipFileName.Length - 4);
        Directory.CreateDirectory(zipFileSubDir);
        QuickZip.Unzip(zipFile, zipFileSubDir, new string[] { "*" });

        //Scan the unipped contents for input files
        ScanForInputFiles(zipFileSubDir);
        //DirectoryInfo dirInfo = new DirectoryInfo(zipFileSubDir);
        //dirInfo.Delete(true);
      }
      catch (Exception e)
      {
        NexdoxMessaging.SendMessage("Failed to unzip file: " + zipFile + " error message: " + e.Message, true, this);

        //_errorFiles.Add();
        return false;
      }

      return true;
    }

    #endregion
  }
}
