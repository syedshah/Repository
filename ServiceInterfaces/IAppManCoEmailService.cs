namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface IAppManCoEmailService
  {
    AppManCoEmail GetAppManCoEmail(int id);

    IList<AppManCoEmail> GetAppManCoEmails();

    IList<AppManCoEmail> GetAppManCoEmails(int? appId, int? manCoId);

    PagedResult<AppManCoEmail> GetPagedAppManCoEmails(int pageNumber, int numberOfItems);

    PagedResult<AppManCoEmail> GetPagedAppManCoEmails(string accountNumber, int? appId, int? manCoId, int pageNumber, int numberOfItems);

    void CreateAppManCoEmail(int appId, int manCoId, int docTypeId, string accountNumber, string email);

    void UpdateAppManCoEmail(int id, int appId, int manCoId, int docTypeId, string accountNumber, string email, string userName);

    void DeleteAppManCoEmail(int id);
  }
}
