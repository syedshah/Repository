namespace Services
{
  using System;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class UserReportService : IUserReportService
  {
    private readonly IApplicationUserRepository _applicationUserRepository;

    public UserReportService(IApplicationUserRepository applicationUserRepository)
    {
      _applicationUserRepository = applicationUserRepository;
    }

    public PagedResult<ApplicationUser> GetUserReport(int docicileId, int pageNumber, int numberOfItems)
    {
      try
      {
        return _applicationUserRepository.GetUserReport(docicileId, pageNumber, numberOfItems);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get user report information for docicile ", e);
      }
    }
  }
}
