namespace NTGEN95
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  using Exceptions;

  public class XmlDataExtractor
  {
    public List<ExtractedDocument> GetDocument(string dataFile)
    {
      List<ExtractedDocument> extractedDocs = new List<ExtractedDocument>();
      XElement application;
      string applicationName = string.Empty;
      try
      {
        ExtractedDocument extractedDoc;

        // Load Xml file
        XDocument xml = XDocument.Load(dataFile);

        // Read xml file application data
        application = (from a in xml.Descendants("Application") select a).FirstOrDefault();

        // Read xml file document data into document class
        var documents = xml.Descendants("Documents");// select new { DocumentChildren = c.Descendants("Document") });

        applicationName = application.Attribute("application").Value;

        foreach (var document in documents.Descendants("Document"))
        {
          extractedDoc = new ExtractedDocument
                           {
                             DocumentId = document.Attribute("DocumentIdGUID").Value,
                             ManCoCode = document.Attribute("MANAGEMENT_COMPANY").Value,
                             DocType = document.Attribute("DOCUMENT_TYPE").Value,
                             SubDocType = document.Attribute("DOCUMENT_SUB_TYPE").Value,
                             Application = applicationName,
                             MailPrintFlag = document.Attribute("MAIL_PRINT_FLAG").Value
                           };

          extractedDocs.Add(extractedDoc);
        }
      }
      catch (Exception e)
      {
        throw new UnityException(string.Format("Unable to retrieve application information from the trigger file for application {0}.", applicationName), e);
      }

      return extractedDocs;
    }

    public string GetGrid(string dataFile)
    {
      try
      {
        // Load Xml file
        XDocument xml = XDocument.Load(dataFile);

        // Read xml file batch data
        var batch = (from a in xml.Descendants("Batch") select a).FirstOrDefault();

        // Read xml file document data into document class
        return batch.Attribute("grid").Value;
      }
      
      catch (Exception)
      {
        throw;
      }
    }
  }
}
