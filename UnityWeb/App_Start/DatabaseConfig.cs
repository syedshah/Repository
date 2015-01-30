namespace UnityWeb.App_Start
{
  using System.Configuration;
  using UnityRepository.Contexts;
  using UnityRepository.Initializers;

  public class DatabaseConfig
  {
    public static void Initialize()
    {
      var dbContext = new UnityDbContext(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      var dbIntializer = new UnityDbInitializer();

      dbIntializer.InitializeDatabase(dbContext);
    }
  }
}