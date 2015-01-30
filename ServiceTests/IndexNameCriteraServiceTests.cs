namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using FluentAssertions;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;

  [TestFixture]
  public class IndexNameCriteraServiceTests
  {
    [SetUp]
    public void Setup()
    {
      _indexNameCriteraService = new IndexNameCriteraService();
    }

    private IIndexNameCriteraService _indexNameCriteraService;

    [Test]
    public void GivenASearchFields_WhenIAskToBuildTheType_IndexNameCriteriaDataIsReturned()
    {
      var listManCoTexts = new List<string>();
      listManCoTexts.Add("manco1");
      listManCoTexts.Add("manco2");

      var result = _indexNameCriteraService.BuildSearchCriteria(
        "docType",
        "subDocType",
        "addresseeSubType",
        "accountNumber",
        "mailingName",
        listManCoTexts,
        "investorReference",
        new DateTime(2014, 1, 13, 13, 10, 22),
        new DateTime(2014, 1, 14, 13, 10, 22),
        "primaryHolder",
        "agentReference",
        "addresseePostCode",
        "emailAddress",
        "faxNumber",
        new DateTime(2014, 1, 13, 13, 10, 22),
        new DateTime(2014, 1, 12, 13, 10, 22),
        "documentNumber");

      result.Should()
            .Contain(i => i.IndexName == "DOCUMENT_TYPE")
            .And.Contain(i => i.SearchValue == "docType")
            .And.Contain(i => i.IndexName == "DOCUMENT_SUB_TYPE")
            .And.Contain(i => i.SearchValue == "subDocType")
            .And.Contain(i => i.IndexName == "ACCOUNT_NUMBER")
            .And.Contain(i => i.SearchValue == "accountNumber")
            .And.Contain(i => i.IndexName == "MAILING_NAME")
            .And.Contain(i => i.SearchValue == "mailingName")
            .And.Contain(i => i.IndexName == "MANAGEMENT_COMPANY")
            .And.Contain(i => i.SearchValue == "manco1")
            .And.Contain(i => i.IndexName == "MANAGEMENT_COMPANY")
            .And.Contain(i => i.SearchValue == "manco2")
            .And.Contain(i => i.IndexName == "INVESTOR_REFERENCE")
            .And.Contain(i => i.SearchValue == "investorReference")
            .And.Contain(i => i.IndexName == "ADDRESSEE_SUB_TYPE")
            .And.Contain(i => i.SearchValue == "addresseeSubType")
            .And.Contain(i => i.IndexName == "PRIMARY_HOLDER_NAME")
            .And.Contain(i => i.SearchValue == "primaryHolder")
            .And.Contain(i => i.IndexName == "AGENT_REFERENCE")
            .And.Contain(i => i.SearchValue == "agentReference")
            .And.Contain(i => i.IndexName == "ADDRESSEE_POSTCODE")
            .And.Contain(i => i.SearchValue == "addresseePostCode")
            .And.Contain(i => i.IndexName == "EMAIL_ADDRESS")
            .And.Contain(i => i.SearchValue == "emailAddress")
            .And.Contain(i => i.IndexName == "FAX_NUMBER")
            .And.Contain(i => i.SearchValue == "faxNumber")
            .And.Contain(i => i.IndexName == "CONTRACT_DATE")
            .And.Contain(i => i.SearchValue == "13/01/2014*")
            .And.Contain(i => i.IndexName == "PAYMENT_DATE")
            .And.Contain(i => i.SearchValue == "12/01/2014*")
            .And.Contain(i => i.IndexName == "DOCUMENT_REFERENCE")
            .And.Contain(i => i.SearchValue == "documentNumber")
            .And.HaveCount(16);
    }
  }
}
