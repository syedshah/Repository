namespace NTGEN93
{
  using BusinessEngineInterfaces;

  using Nexdox.Composer;

  public class Process
  {
    private ApplicationInfo _appInfo;

    private NexdoxEngine _engine;

    private IHouseHoldingRunEngine _houseHoldingRunEngine;

    public Process(ApplicationInfo appInfo)
    {
      _appInfo = appInfo;
    }

    public void Go()
    {
      NexdoxMessaging.StartEvent(this);
      Statics.Initialise(_engine, _appInfo);

      this.PerformIOC();

      _houseHoldingRunEngine.ProcessHouseHoldingRun();

      NexdoxMessaging.EndEvent(this);
    }

    private void PerformIOC()
    {
      IoCContainer.ResoloveDependencies(_appInfo);

      _houseHoldingRunEngine = IoCContainer.Resolve<IHouseHoldingRunEngine>();
    }
  }
}
