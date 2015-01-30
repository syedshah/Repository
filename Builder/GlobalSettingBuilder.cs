namespace Builder
{
  using Entities;

  public class GlobalSettingBuilder : Builder<GlobalSetting>
  {
    public GlobalSettingBuilder()
    {
      Instance = new GlobalSetting();   
    }
  }
}
