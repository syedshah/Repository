namespace Serialization
{
  using System.Text;
  using Newtonsoft.Json;

  public static class JsonSerializer
  {
    private static readonly Encoding Encoding = Encoding.UTF8;

    public static string SerializeToString<T>(this T o)
    {
      const Formatting formatting = new Formatting();
      var settings = new JsonSerializerSettings
      {
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
      };

      return JsonConvert.SerializeObject(o, formatting, settings);
    }

    public static T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

  }
}