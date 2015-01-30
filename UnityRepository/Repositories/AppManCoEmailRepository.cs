namespace UnityRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class AppManCoEmailRepository : BaseEfRepository<AppManCoEmail>, IAppManCoEmailRepository
  {
    public AppManCoEmailRepository(string connectionString)
          : base(new UnityDbContext(connectionString))
    {
        
    }

    public AppManCoEmail GetAppManCoEmail(int id)
    {
      return (from a in Entities
              where a.Id == id
              select a).FirstOrDefault();
    }
      
    public IList<AppManCoEmail> GetAppManCoEmails()
    {
      return (from a in Entities.Include(x => x.ManCo).Include(x => x.Application).Include(x => x.DocType) select a).ToList();
    }

    public IList<AppManCoEmail> GetAppManCoEmails(int? appId, int? manCoId)
    {
        return (from a in Entities
                    .Include(x => x.ManCo)
                    .Include(x => x.Application)
                    .Include(x => x.DocType) 
                    where appId.HasValue & appId > 0? a.ApplicationId == appId:true
                    where manCoId.HasValue & manCoId > 0? a.ManCoId == manCoId: true
                select a).ToList();
    }

    public void UpdateAppManCoEmail(int id, int appId, int manCoId, int docTypeId, string accountNumber, string email, string userName)
    {
      var appManCoEmail = this.GetAppManCoEmail(id);

      var appManCoEmailHistory = new AppManCoEmailHistory
                                 {
                                   ChangeDate = DateTime.Now,
                                   ChangedBy = userName,
                                   ChangeInfo =
                                     String.Format(
                                       "Old ApplicationId : {0}, New ApplicationId: {1}, Old ManCoId: {2}, New ManCoId: {3}, Old DocType: {4}, New DocType: {5}, Old Account Number: {6}, New Account Number {7}, Old Email: {8}, New Email: {9}",
                                       appManCoEmail.ApplicationId,
                                       appId,
                                       appManCoEmail.ManCoId,
                                       manCoId,
                                       appManCoEmail.DocTypeId,
                                       docTypeId,
                                       appManCoEmail.AccountNumber,
                                       accountNumber,
                                       appManCoEmail.Email,
                                       email)
                                 };

      appManCoEmail.ApplicationId = appId;
      appManCoEmail.ManCoId = manCoId;
      appManCoEmail.DocTypeId = docTypeId;
      appManCoEmail.AccountNumber = accountNumber;
      appManCoEmail.Email = email;
      appManCoEmail.AppManCoEmailHistorys.Add(appManCoEmailHistory);

      this.Update(appManCoEmail);
    }

    public PagedResult<AppManCoEmail> GetPagedAppManCoEmails(int pageNumber, int numberOfItems)
    {
        var appManCoEmails = (from a in Entities.Include(x => x.ManCo).Include(x => x.Application).Include(x => x.DocType) select a).ToList();

        return new PagedResult<AppManCoEmail>
        {
            CurrentPage = pageNumber,
            ItemsPerPage = numberOfItems,
            TotalItems = appManCoEmails.Count(),
            Results = appManCoEmails.OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * numberOfItems)
            .Take(numberOfItems)
            .ToList(),
            StartRow = ((pageNumber - 1) * numberOfItems) + 1,
            EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
        };
    }

    public PagedResult<AppManCoEmail> GetPagedAppManCoEmails(string accountNumber, int? appId, int? manCoId, int pageNumber, int numberOfItems)
    {
      var appManCoEmails = (from a in Entities
                  .Include(x => x.ManCo)
                  .Include(x => x.Application)
                  .Include(x => x.DocType)
                            where appId.HasValue & appId > 0 ? a.ApplicationId == appId : true
                            where manCoId.HasValue & manCoId > 0 ? a.ManCoId == manCoId : true
                            where !string.IsNullOrEmpty(accountNumber) ? a.AccountNumber.Contains(accountNumber) : true
                            select a).ToList();

        return new PagedResult<AppManCoEmail>
        {
            CurrentPage = pageNumber,
            ItemsPerPage = numberOfItems,
            TotalItems = appManCoEmails.Count(),
            Results = appManCoEmails.OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * numberOfItems)
            .Take(numberOfItems)
            .ToList(),
            StartRow = ((pageNumber - 1) * numberOfItems) + 1,
            EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
        };
    }
  }
}
