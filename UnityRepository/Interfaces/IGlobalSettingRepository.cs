namespace UnityRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IGlobalSettingRepository : IRepository<GlobalSetting>
  {
    GlobalSetting Get();
  }
}
