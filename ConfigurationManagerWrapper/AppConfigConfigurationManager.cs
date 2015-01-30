namespace ConfigurationManagerWrapper
{
  using System.Collections.Specialized;
  using System.Configuration;
  using AbstractConfigurationManager;

  public class AppConfigConfigurationManager : IConfigurationManager
  {
    public string AppSetting(string name)
    {
      return ConfigurationManager.AppSettings[name];
    }

    public NameValueCollection AppSettings
    {
      get { return ConfigurationManager.AppSettings; }
    }
  }
}
