namespace UnityWeb.Models.Document
{
  using System;

  public class DocumentViewModel
  {
    public DocumentViewModel(ClientProxies.ArchiveServiceReference.IndexedDocumentData document)
    {
      DocumentId = document.Id.ToString();

      ApprovalStatus = "Unapproved";

      foreach (var mappedIndex in document.MappedIndexes)
      {
        switch (mappedIndex.IndexName.ToUpper())
        {
          case "DOCUMENT_TYPE":
            DocType = mappedIndex.IndexValue;
            break;
          case "DOCUMENT_SUB_TYPE":
            SubDocType = mappedIndex.IndexValue;
            break;
          case "ADDRESSEE_SUB_TYPE":
            AddresseeSubType = mappedIndex.IndexValue;
            break;
          case "MAILING_NAME":
            MailingName = mappedIndex.IndexValue;
            break;
          case "PROCESSING_DATE":
            ProcessingDate = mappedIndex.IndexValue;
            break;
          case "PRIMARY_HOLDER_NAME":
            PrimaryHolderName = mappedIndex.IndexValue;
            break;
          case "MANAGEMENT_COMPANY":
            ManCo = mappedIndex.IndexValue;
            break;
          case "DESIGNATION":
            Designation = "DESIGNATION";
            break;
          case "INVESTOR_REFERENCE":
            InvestorReference = mappedIndex.IndexValue;
            break;
          case "ACCOUNT_NUMBER":
            AccountNumber = mappedIndex.IndexValue;
            break;
          case "AGENT_REFERENCE":
            AgentReference = mappedIndex.IndexValue;
            break;
          case "ADDRESSEE_POSTCODE":
            AddresseePostCode = "ADDRESSEE_POSTCODE";
            break;
          case "EMAIL_ADDRESS":
            EmailAddress = "EMAIL_ADDRESS";
            break;
          case "FAX_NUMBER":
            FaxNumber = "FAX_NUMBER";
            break;
          case "CONTRACT_DATE":
            ContractDate =  "CONTRACT_DATE";
            break;
          case "PAYMENT_DATE":
            PaymentDate = "PAYMENT_DATE";
            break;
          case "ACCOUNTING_PERIOD_END":
            AccountingPerionEnd = "ACCOUNTING_PERIOD_END";
            break;
          case "DOCUMENT_REFERENCE":
            DocumentReference = "DOCUMENT_REFERENCE";
            break;
          case "NTID":
            NTID = mappedIndex.IndexValue;
            break;
        }
      }
    }

    public string PrimaryHolderName { get; set; }

    public string AccountNumber { get; set; }

    public string AgentReference { get; set; }

    public string AddresseePostCode { get; set; }

    public string EmailAddress { get; set; }

    public string FaxNumber { get; set; }

    public string ContractDate { get; set; }

    public string PaymentDate { get; set; }

    public string AccountingPerionEnd { get; set; }

    public string DocumentReference { get; set; }

    public string Designation { get; set; }

    public string DocumentId { get; set; }

    public string DocType { get; set; }

    public string SubDocType { get; set; }

    public string AddresseeSubType { get; set; }

    public string MailingName { get; set; }

    public string InvestorReference { get; set; }

    public string ManCo { get; set; }

    public string ManCoDisplay { get; set; }

    public string ProcessingDate { get; set; }

    public string MailingDate { get; set; }

    public string NTID { get; set; }

    public bool Selected { get; set; }

    public bool CheckedOut { get; set; }

    public string CheckedOutBy { get; set; }

    public DateTime CheckedOutDate { get; set; }

    public string ApprovalStatus { get; set; }

    public string ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string RejectedBy { get; set; }

    public DateTime? RejectionDate { get; set; }

    public string Grid { get; set; }

    public string MailPrintFlag { get; set; }

    public void AddCheckoutDetails(Entities.Document document)
    {
      if (document != null && document.CheckOut != null)
      {
        CheckedOutBy = document.CheckOut.CheckOutBy;
        CheckedOutDate = document.CheckOut.CheckOutDate.Value;
        CheckedOut = true;
      }
    }

    public void AddApprovalDetails(Entities.Document document)
    {
      if (document != null && document.Approval != null)
      {
        ApprovalStatus = "Approved";
        ApprovedBy = document.Approval.ApprovedBy;
        ApprovedDate = document.Approval.ApprovedDate;
      }
    }

    public void AddRejectionDetails(Entities.Document document)
    {
      if (document != null && document.Rejection != null)
      {
        ApprovalStatus = "Rejected";
        RejectedBy = document.Rejection.RejectedBy;
        RejectionDate = document.Rejection.RejectionDate;
      }
    }

    public void AddGridRunDetails(Entities.Document document)
    {
      if (document != null && document.GridRun != null)
      {
        Grid = document.GridRun.Grid;
      }
    }

    public void AddManCoDetails(Entities.Document document)
    {
      if (document != null && document.ManCo != null)
      {
        ManCoDisplay = string.Format("{0} - {1}", document.ManCo.Code, document.ManCo.Description);
        ManCo = document.ManCo.Code;
      }
    }

    public void AddDoNotPrintFlag(Entities.Document document)
    {
      if (document != null)
      {
        MailPrintFlag = document.MailPrintFlag;
      }
    }
  }
}