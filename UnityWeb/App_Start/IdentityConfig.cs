namespace UnityWeb.App_Start
{
  using IdentityWrapper.Identity;

  public class IdentityConfig
  {
    public static void Initialize()
    {
       IndentityConfigUtility.Initialize();   
    }
  }
}