namespace AbstractConfigurationManager
{
  using System.Collections.Specialized;

  public interface IConfigurationManager
  {
    string AppSetting(string name);

    NameValueCollection AppSettings { get; }
  }
}
