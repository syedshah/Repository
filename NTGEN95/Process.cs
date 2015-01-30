namespace NTGEN95
{
  using System.Configuration;

  using BusinessEngineInterfaces;

  using Nexdox.Composer;

  using ServiceInterfaces;

  public class Process
  {
    private ApplicationInfo _appInfo;

    private NexdoxEngine _engine;

    private IDocumentService _documentService;

    private IAutoApprovalService _documentApprovalService;

    private ISubDocTypeService _subDocTypeService;

    private IGridRunEngine _gridRunEngine;

    private IManCoService _manCoService;

    private IApprovalEngine _approvalEngine;

    private IGridRunService _gridRunService;

    /// <summary>
    /// Default Constructor.
    /// </summary>
    /// <param name="appInfo">Application Info created by NexdoxLaunch and passed to the program on declaration.</param>
    public Process(ApplicationInfo appInfo)
    {
      _appInfo = appInfo;
    }

    public void Go()
    {
      // Start the nexdox messaging service
      NexdoxMessaging.StartEvent(this);

      // Initialise Statics
      Statics.Initialise(_engine, _appInfo);

      this.PerformIOC();

      // Allocation Stage
      Allocation allocationStage = new Allocation(
        _engine, _appInfo, _documentService, _documentApprovalService, _subDocTypeService, _gridRunEngine, _manCoService, _approvalEngine, _gridRunService);
      allocationStage.Process();

      // Stop the nexdox messaging service
      NexdoxMessaging.EndEvent(this);
    }

    private void PerformIOC()
    {
      string test = ConfigurationManager.AppSettings["companyCode"];

      IoCContainer.ResoloveDependencies();

      _documentService = IoCContainer.Resolve<IDocumentService>();
      _documentApprovalService = IoCContainer.Resolve<IAutoApprovalService>();
      _subDocTypeService = IoCContainer.Resolve<ISubDocTypeService>();
      _gridRunEngine = IoCContainer.Resolve<IGridRunEngine>();
      _manCoService = IoCContainer.Resolve<IManCoService>();
      _approvalEngine = IoCContainer.Resolve<IApprovalEngine>();
      _gridRunService = IoCContainer.Resolve<IGridRunService>();
    }

  }
}
