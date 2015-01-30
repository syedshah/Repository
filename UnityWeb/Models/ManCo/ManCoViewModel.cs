namespace UnityWeb.Models.ManCo
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;

  public class ManCoViewModel
  {
    public ManCoViewModel(Entities.ManCo manCo)
    {
      Id = manCo.Id;
      Code = manCo.Code;
      Description = manCo.Description;
      DisplayName = Code == "All" ? string.Format(Description) : string.Format("{0} - {1}", Code, Description);
    }

    public int Id { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }

    public string DisplayName { get; set; }

    public void AddDisplayName(IList<Entities.ManCo> manCos)
    {
      this.DisplayName = Code == "All" ? string.Format(Description) : string.Format("{0} - {1}", Code, Description);

      var codes = from m in manCos select m.Code;

      if (codes.Any(c => c == null))
        return;

      var groupedCodes = from m in manCos group m by m.Code.Length into g select g;

      if (groupedCodes.Count() > 1)
      {
        var paddedCode = Code;

        if (Code.Length < 5)
        {
          paddedCode = Code.PadRight(9, Convert.ToChar(160));
        }

        this.DisplayName = Code == "All" ? string.Format(Description) : string.Format("{0} - {1}", paddedCode, Description);
      }
      else
      {
        this.DisplayName = Code == "All" ? string.Format(Description) : string.Format("{0} - {1}", Code, Description);
      }
    }
  }
}