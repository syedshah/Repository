namespace ServiceInterfaces
{
  using Entities;

  public interface IUserReportService
  {
    PagedResult<ApplicationUser> GetUserReport(int docicileId, int pageNumber, int numberOfItems);
  }
}
