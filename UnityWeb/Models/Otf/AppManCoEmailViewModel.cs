namespace UnityWeb.Models.Otf
{
  using UnityWeb.Models.Applicaiton;
  using UnityWeb.Models.DocType;
  using UnityWeb.Models.ManCo;

  public class AppManCoEmailViewModel
  {
    public AppManCoEmailViewModel(Entities.AppManCoEmail appManCoEmail)
    {
      this.Id = appManCoEmail.Id;
      this.AccountNumber = appManCoEmail.AccountNumber;
      this.Email = appManCoEmail.Email;
      this.ApplicationViewModel = new ApplicationViewModel(appManCoEmail.Application);
      this.ManCoViewModel = new ManCoViewModel(appManCoEmail.ManCo);
      this.DocTypeViewModel = new DocTypeViewModel(appManCoEmail.DocType);
    }

    public int Id { get; set; }

    public ApplicationViewModel ApplicationViewModel { get; set; }

    public ManCoViewModel ManCoViewModel { get; set; }

    public DocTypeViewModel DocTypeViewModel { get; set; }

    public string AccountNumber { get; set; }

    public string Email { get; set; }
  }
}