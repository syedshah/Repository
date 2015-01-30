namespace UnityWeb.Models.Otf
{
   using System.Collections.Generic;
   using System.Linq;

   public class AppManCoEmailsViewModel
   {
      public AppManCoEmailsViewModel()
      {
          
      }

      public AppManCoEmailsViewModel(IList<Entities.Application> applications, IList<Entities.ManCo> manCos, IList<Entities.DocType> docTypes)
      {
         AddAppManCoEmailViewModel = new AddAppManCoEmailViewModel(applications, manCos, docTypes);
      }

      private List<AppManCoEmailViewModel> _appManCoEmails = new List<AppManCoEmailViewModel>();

      public List<AppManCoEmailViewModel> AppManCoEmails
      {
        get
        {
          return _appManCoEmails;
        }
        set
        {
          _appManCoEmails = value;
        }
      }

      public AddAppManCoEmailViewModel AddAppManCoEmailViewModel { get; set; }

      public void AddAppManCoEmails(IList<Entities.AppManCoEmail> appManCoEmails)
      {
         appManCoEmails.ToList().ForEach(x => AppManCoEmails.Add(new AppManCoEmailViewModel(x)));
      }
   }
}