namespace UnityRepository.Repositories
{
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class GlobalSettingRepository : BaseEfRepository<GlobalSetting>, IGlobalSettingRepository
  {
    public GlobalSettingRepository(string connectionString)
          : base(new UnityDbContext(connectionString))
    {
          
    }

    public GlobalSetting Get()
    {
      return (from a in Entities
              select a).FirstOrDefault();
    }
  }
}
