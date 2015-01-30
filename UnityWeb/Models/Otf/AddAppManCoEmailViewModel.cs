namespace UnityWeb.Models.Otf
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Models.Applicaiton;
  using UnityWeb.Models.DocType;
  using UnityWeb.Models.ManCo;

  public class AddAppManCoEmailViewModel
  {
    public AddAppManCoEmailViewModel()
    {
          
    }

    public AddAppManCoEmailViewModel(IList<Entities.Application> applications, IList<Entities.ManCo> manCos, IList<Entities.DocType> docTypes)
    {
      Applications = new ApplicationsViewModel();
      Applications.AddApplications(applications);

      ManCos = new ManCosViewModel();
      ManCos.AddMancos(manCos);
        
      DocTypes = new DocTypesViewModel();
      DocTypes.AddDocTypes(docTypes);
    }

    [Required]
    public int ApplicationId { get; set; }

    public ApplicationsViewModel Applications { get; set; }

    [Required]
    public int ManCoId { get; set; }

    public ManCosViewModel ManCos { get; set; }

    [Required]
    public int DocTypeId { get; set; }

    public DocTypesViewModel DocTypes { get; set; }

    [Required]
    public string AccountNumber { get; set; }

    [Required]
    public string Email { get; set; }
  }
}