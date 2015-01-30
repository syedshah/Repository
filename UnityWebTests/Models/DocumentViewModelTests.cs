namespace UnityWebTests.Models
{
  using System;
  using System.Collections.Generic;

  using ClientProxies.ArchiveServiceReference;

  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.Document;

  [TestFixture]
  public class DocumenthViewModelTests
  {
    [Test]
    public void GivenANewDocumentsCollection_WhenIAccessTheCollection_ThenItIsTheSameAsTheSetValue()
    {
      var documents = new List<DocumentViewModel>();
      var model = new DocumentsViewModel();
      model.Documents = documents;

      model.Documents.Should().BeEquivalentTo(documents);
    }

    [Test]
    public void GivenADocuemnt_WhenIAskToCreateTheVieWModel_ThenCorrectValuesAreReturned()
    {
      var docId = Guid.NewGuid();
      var document = new ClientProxies.ArchiveServiceReference.IndexedDocumentData()
                       {
                         Id = docId,
                         MappedIndexes = new List<IndexMapped>
                                           {
                                             new IndexMapped()
                                               {
                                                 IndexName = "documenttype",
                                                 IndexValue = "docTypeValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "documentsubtype",
                                                 IndexValue = "docSubTypeValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "manco",
                                                 IndexValue = "manCoValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "processingdate",
                                                 IndexValue = "processingdateValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "mailingname",
                                                 IndexValue = "mailingnameValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "ntid",
                                                 IndexValue = "ntidValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "faxstatus",
                                                 IndexValue = "faxstatusValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "faxdate",
                                                 IndexValue = "faxdateValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "emailstatus",
                                                 IndexValue = "emailstatusValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "emaildate",
                                                 IndexValue = "emaildateValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "printstatus",
                                                 IndexValue = "printstatusValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "printdate",
                                                 IndexValue = "printdateValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "documentdate",
                                                 IndexValue = "documentdateValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "mailingdate",
                                                 IndexValue = "mailingdateValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "investorreference",
                                                 IndexValue = "investorreferenceValue"
                                               },
                                               new IndexMapped()
                                               {
                                                 IndexName = "accountnumber",
                                                 IndexValue = "accountnumberValue"
                                               },
                                           }
                       };

      var model = new DocumentViewModel(document);
      model.DocumentId = docId.ToString();
      model.DocType = "docTypeValue";
      model.SubDocType = "docSubTypeValue";
      model.MailingName = "mailingnameValue";
      model.InvestorReference = "investorreferenceValue";
      model.ManCo = "manCoValue";
      model.ProcessingDate = "processingdateValue";
      model.MailingDate = "mailingdateValue";
      model.NTID = "ntidValue";
      model.AccountNumber = "accountnumberValue";
    }
  }
}
