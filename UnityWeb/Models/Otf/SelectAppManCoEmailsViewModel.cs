namespace UnityWeb.Models.Otf
{
  using System.Collections.Generic;
  using System.Linq;

  using UnityWeb.Models.Applicaiton;
  using UnityWeb.Models.ManCo;

  public class SelectAppManCoEmailsViewModel
  {
    public SelectAppManCoEmailsViewModel()
    {
      Applications = new List<ApplicationViewModel>();
      ManCos = new List<ManCoViewModel>();
    }

    public string SelectedAccountNumber { get; set; }

    public int SelectedManCoId { get; set; }

    public int SelectedApplicationId { get; set; }

    public List<ManCoViewModel> ManCos { get; set; }

    public List<ApplicationViewModel> Applications { get; set; }

    public void AddManCos(IList<Entities.ManCo> manCos)
    {
      foreach (Entities.ManCo manCo in manCos)
      {
        var mvm = new ManCoViewModel(manCo);
        mvm.AddDisplayName(manCos);
        ManCos.Add(mvm);
      }
    }

    public void AddApplications(IList<Entities.Application> applications)
    {
      foreach (Entities.Application application in applications.OrderBy(c => c.Code))
      {
        var app = new ApplicationViewModel(application);
        Applications.Add(app);
      }
    }

    public int Page { get; set; }
  }
}