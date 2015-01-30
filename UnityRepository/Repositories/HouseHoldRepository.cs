namespace UnityRepository.Repositories
{
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class HouseHoldRepository : BaseEfRepository<HouseHold>, IHouseHoldRepository
  {
    public HouseHoldRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }
  }
}
