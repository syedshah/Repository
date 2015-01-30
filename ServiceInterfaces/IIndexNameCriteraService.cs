namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using ClientProxies.ArchiveServiceReference;

  public interface IIndexNameCriteraService
  {
    List<IndexNameCriteriaData> BuildSearchCriteria(
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
      string documentNumber);
  }
}
