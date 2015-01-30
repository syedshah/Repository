namespace UnityWeb.Models.Applicaiton
{
  using System.Collections.Generic;
  using System.Linq;

  public class ApplicationsViewModel
  {
    private List<ApplicationViewModel> _applications = new List<ApplicationViewModel>();

    public List<ApplicationViewModel> Applicaitons
    {
      get { return _applications; }
      set { _applications = value; }
    }

    public void AddApplications(IList<Entities.Application> applications)
    {
      foreach (Entities.Application application in applications.OrderBy(c => c.Code))
      {
        var avm = new ApplicationViewModel(application);
        Applicaitons.Add(avm);
      }
    }
  }
}