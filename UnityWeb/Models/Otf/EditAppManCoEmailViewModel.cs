namespace UnityWeb.Models.Otf
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using UnityWeb.Models.Applicaiton;
  using UnityWeb.Models.DocType;
  using UnityWeb.Models.ManCo;

  public class EditAppManCoEmailViewModel
  {
    
    public EditAppManCoEmailViewModel(){
         
    }

    public EditAppManCoEmailViewModel(IList<Entities.Application> applications, IList<Entities.ManCo> manCos, IList<Entities.DocType> docTypes, Entities.AppManCoEmail appManCoEmail)
    {
      Applications = new ApplicationsViewModel();
      Applications.AddApplications(applications);

      ManCos = new ManCosViewModel();
      ManCos.AddMancos(manCos);

      DocTypes = new DocTypesViewModel();
      DocTypes.AddDocTypes(docTypes);

      Id = appManCoEmail.Id;
      DocTypeId = appManCoEmail.DocTypeId;
      ManCoId = appManCoEmail.ManCoId;
      ApplicationId = appManCoEmail.ApplicationId;
      OtfEmail = appManCoEmail.Email;
      OrigionalEmail = appManCoEmail.Email;
      OtfAccountNumber = appManCoEmail.AccountNumber;
      //ConfirmEmail = string.Empty;
    }

    public int Id { get; set; }

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
    public string OtfAccountNumber { get; set; }

    [Required]
    public string OtfEmail { get; set; }

    public string OrigionalEmail { get; set; }

    //public string ConfirmEmail { get; set; }
  }
}