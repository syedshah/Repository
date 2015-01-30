namespace NTGEN93
{
  using Nexdox.Composer;

  internal static class Statics
  {
    public static string UnitySQLServer;

    public static void Initialise(NexdoxEngine engine, ApplicationInfo appInfo)
    {
      UnitySQLServer = appInfo["UnitySQLServer"];
    }
  }
}
