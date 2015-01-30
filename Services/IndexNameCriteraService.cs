namespace Services
{
  using System;
  using System.Collections.Generic;
  using ClientProxies.ArchiveServiceReference;
  using ServiceInterfaces;

  public class IndexNameCriteraService : IIndexNameCriteraService
  {
    public List<IndexNameCriteriaData> BuildSearchCriteria(
      string docType,
      string subDocType,
      string addressSubType,
      string accountNumber,
      string mailingName,
      IList<string> managementCompanies,
      string investorReference,
      DateTime? processingDateFrom,
      DateTime? processingDateTo,
      string primaryHolder,
      string agentReference,
      string addresseePostCode,
      string emailAddress,
      string faxNumber,
      DateTime? contractDate,
      DateTime? paymentDate,
      string documentNumber)
    {
      var criteria = new List<IndexNameCriteriaData>();

      if (!string.IsNullOrEmpty(docType))
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "DOCUMENT_TYPE", SearchValue = docType });
      }

      if (!string.IsNullOrEmpty(subDocType))
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "DOCUMENT_SUB_TYPE", SearchValue = subDocType });
      }

      if (!string.IsNullOrEmpty(addressSubType))
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "ADDRESSEE_SUB_TYPE", SearchValue = addressSubType });
      }

      if (!string.IsNullOrEmpty(accountNumber))
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "ACCOUNT_NUMBER", SearchValue = accountNumber });
      }

      if (!string.IsNullOrEmpty(mailingName))
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "MAILING_NAME", SearchValue = mailingName });
      }

      foreach (var managementCompany in managementCompanies)
      {
        //if (managementCompany == "022" || managementCompany == "024")
        //{
          criteria.Add(new IndexNameCriteriaData() { IndexName = "MANAGEMENT_COMPANY", SearchValue = managementCompany });  
        //}
      }

      if (!string.IsNullOrEmpty(investorReference))
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "INVESTOR_REFERENCE", SearchValue = investorReference });
      }

      if (primaryHolder != null)
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "PRIMARY_HOLDER_NAME", SearchValue = primaryHolder});
      }

      if (agentReference != null)
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "AGENT_REFERENCE", SearchValue = agentReference});
      }

      if (addresseePostCode != null)
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "ADDRESSEE_POSTCODE", SearchValue = addresseePostCode});
      }

      if (emailAddress != null)
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "EMAIL_ADDRESS", SearchValue = emailAddress});
      }

      if (faxNumber != null)
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "FAX_NUMBER", SearchValue = faxNumber});
      }

      if (contractDate != null)
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "CONTRACT_DATE", SearchValue = string.Format("{0}*", contractDate.Value.ToString("d")) });
      }

      if (paymentDate != null)
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "PAYMENT_DATE", SearchValue = string.Format("{0}*", paymentDate.Value.ToString("d")) });
      }

      if (documentNumber != null)
      {
        criteria.Add(new IndexNameCriteriaData() { IndexName = "DOCUMENT_REFERENCE", SearchValue = documentNumber});
      }

      return criteria;
    }
  }
}
