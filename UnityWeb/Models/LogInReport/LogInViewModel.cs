// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogInViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Used to hold individual log ins
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UnityWeb.Models.LogInReport
{
  using System;

  public class LogInViewModel
  {
    public LogInViewModel(string date, DateTime logIn, DateTime? logOut)
    {
      Date = date;
      LogIn = String.Format("{0:T}", logIn);
      LogOut = logOut == null ? string.Empty : String.Format("{0:T}", logOut.Value);
    }

    public string Date { get; set; }

    public string LogIn { get; set; }

    public string LogOut { get; set; }
  }
}