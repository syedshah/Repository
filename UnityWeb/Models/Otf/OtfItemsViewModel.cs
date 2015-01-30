
namespace UnityWeb.Models.Otf
{
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using UnityWeb.Models.Helper;

  public class OtfItemsViewModel
  {
    public OtfItemsViewModel()
    {
      PagingInfo = new PagingInfoViewModel();    
    }

    public OtfItemsViewModel(IList<Entities.Application> applications, IList<Entities.ManCo> manCos, IList<Entities.DocType> docTypes)
    {
      PagingInfo = new PagingInfoViewModel();
      AddAppManCoEmailViewModel = new AddAppManCoEmailViewModel(applications, manCos, docTypes);
    }

    public PagingInfoViewModel PagingInfo { get; set; }

    public string CurrentPage { get; set; }

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

    public void AddAppManCoEmails(PagedResult<Entities.AppManCoEmail> appManCoEmails)
    {
      appManCoEmails.Results.ToList().ForEach(x => AppManCoEmails.Add(new AppManCoEmailViewModel(x)));

      PagingInfo = new PagingInfoViewModel
      {
          CurrentPage = appManCoEmails.CurrentPage,
          TotalItems = appManCoEmails.TotalItems,
          ItemsPerPage = appManCoEmails.ItemsPerPage,
          TotalPages = appManCoEmails.TotalPages,
          StartRow = appManCoEmails.StartRow,
          EndRow = appManCoEmails.EndRow
      };

      CurrentPage = PagingInfo.CurrentPage.ToString();
    }
  }
}