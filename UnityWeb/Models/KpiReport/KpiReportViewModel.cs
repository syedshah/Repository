namespace UnityWeb.Models.KpiReport
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  using UnityWeb.Models.ManCo;

  public class KpiReportViewModel : IValidatableObject
  {
    public string SelectedManCoId { get; set; }

    public DateTime? DateTo { get; set; }

    public DateTime? DateFrom { get; set; }

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
      if (string.IsNullOrEmpty(SelectedManCoId))
      {
        yield return new ValidationResult("Please select a man co");
      }
      else if (!this.ValidateDateRange(DateFrom, DateTo))
      {
        yield return new ValidationResult("Please select a valid date range");
      }
    }

    private bool ValidateDateRange(DateTime? fromDate, DateTime? toDate)
    {
      if (fromDate == null || toDate == null)
      {
        return false;
      }
      else if (fromDate > toDate)
      {
        return false;
      }
      else if (fromDate > DateTime.Now)
      {
        return false;
      }
      else if (toDate > DateTime.Now)
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