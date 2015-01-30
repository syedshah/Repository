// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginReportViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Model used for log in report
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UnityWeb.Models.LogInReport
{
  using System.Collections.Generic;
  using System.Linq;

  public class LoginReportViewModel
  {
    public LoginReportViewModel()
    {
      LogInReport = new Dictionary<string, List<LogInViewModel>>();
    }

    //public string UserName { get; set; }

    public Dictionary<string, List<LogInViewModel>> LogInReport { get; set; }

    public void CreateReport(IList<Entities.Session> sessions)
    {
      var userSessionGroups = (from s in sessions
                               group s by new { s.ApplicationUser.UserName } into g
                               select g).OrderBy(d => d.Key.UserName);

      
      foreach (var userSessionGroup in userSessionGroups)
      {
        string userName = userSessionGroup.Key.UserName;
        List<LogInViewModel> logInViewModels = new List<LogInViewModel>();

        var dateSessionGroups = (from u in userSessionGroup 
                                 group u by new { u.Start.Date } into g 
                                 select g);

        foreach (var dateSessionGroup in dateSessionGroups)
        {
          foreach (var session in dateSessionGroup)
          {
            logInViewModels.Add(new LogInViewModel(dateSessionGroup.Key.Date.ToShortDateString(), session.Start, session.End));
          }
        }
        LogInReport.Add(userName, logInViewModels);
      }
    }
  }
}