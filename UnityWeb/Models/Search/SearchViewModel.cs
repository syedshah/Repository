namespace UnityWeb.Models.Search
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Models.DocType;
  using UnityWeb.Models.ManCo;

  public class SearchViewModel : IValidatableObject
  {
    public SearchViewModel()
    {
      ManCoTexts = new List<string>();
    }

    public string SelectedDocId { get; set; }

    public string SelectedDocText { get; set; }

    public string SelectedSubDocId { get; set; }

    public string SelectedSubDocText { get; set; }

    public string SelectedManCoId { get; set; }

    public string SelectedManCoText { get; set; }

    public List<string> ManCoTexts { get; set; }    

    public string AddresseeSubType { get; set; }

    public string MailingName { get; set; }

    public DateTime? ProcessingDateTo { get; set; }

    public DateTime? ProcessingDateFrom { get; set; }

    public string PrimaryHolder { get; set; }

    public string InvestorReference { get; set; }

    public string AccountNumber { get; set; }

    public string AgentReference { get; set; }

    public string AddresseePostCode { get; set; }

    public string EmailAddress { get; set; }

    public string FaxNumber { get; set; }

    public DateTime? ContractDate { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? AccountPeriodEnd { get; set; }

    public string DocumentNumber { get; set; }

    private List<DocTypeViewModel> _docTypes = new List<DocTypeViewModel>();

    public List<DocTypeViewModel> DocTypes
    {
      get
      {
        return _docTypes;
      }
      set
      {
        _docTypes = value;
      }
    }

    private List<ManCoViewModel> _namCos = new List<ManCoViewModel>();

    public List<ManCoViewModel> ManCos
    {
      get
      {
        return _namCos;
      }
      set
      {
        _namCos = value;
      }
    }

    private List<SubDocTypeViewModel> _subDocTypes = new List<SubDocTypeViewModel>();

    public List<SubDocTypeViewModel> SubDocTypes
    {
      get
      {
        return _subDocTypes;
      }
      set
      {
        _subDocTypes = value;
      }
    }

    public void AddDocTypes(IList<Entities.DocType> docTypes)
    {
      foreach (Entities.DocType docType in docTypes)
      {
        var avm = new DocTypeViewModel(docType);
        DocTypes.Add(avm);
      }
    }

    public void AddSubDocTypes(IList<Entities.SubDocType> subDocTypes)
    {
      foreach (Entities.SubDocType subDocType in subDocTypes)
      {
        var avm = new SubDocTypeViewModel(subDocType);
        SubDocTypes.Add(avm);
      }
    }

    public void AddMancos(IList<Entities.ManCo> manCos)
    {
      foreach (Entities.ManCo manCo in manCos)
      {
        var mvm = new ManCoViewModel(manCo);
        mvm.AddDisplayName(manCos);
        ManCos.Add(mvm);
      }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (string.IsNullOrEmpty(SelectedDocText) && string.IsNullOrEmpty(SelectedSubDocText)
          && string.IsNullOrEmpty(SelectedManCoText) && string.IsNullOrEmpty(AddresseeSubType) 
          && string.IsNullOrEmpty(MailingName) && (ProcessingDateTo == null) && (ProcessingDateFrom == null)
          && string.IsNullOrEmpty(PrimaryHolder) && string.IsNullOrEmpty(InvestorReference)
          && string.IsNullOrEmpty(AccountNumber) && string.IsNullOrEmpty(AgentReference)
          && string.IsNullOrEmpty(AddresseePostCode) && string.IsNullOrEmpty(EmailAddress)
          && string.IsNullOrEmpty(FaxNumber) && ContractDate == null && PaymentDate == null && AccountPeriodEnd == null
          && string.IsNullOrEmpty(DocumentNumber))
      {
        yield return new ValidationResult("Please select a search criteria");
      }
      else if (!this.ValidateDateRange(ProcessingDateFrom, ProcessingDateTo))
      {
        yield return new ValidationResult("Please select a valid process date range");
      }
    }

    private bool ValidateDateRange(DateTime? fromDate, DateTime? toDate)
    {
        if (fromDate != null && toDate == null)
        {
           return false;
        }
        else if (toDate != null && fromDate == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
  }
}
  
