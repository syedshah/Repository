

namespace nugen99
{
  using Nexdox.Composer;
  using ServiceInterfaces;

  /// <summary>
  /// Process Class - Main entry class for application.
  /// </summary>
  public class Process
  {
    #region Member Variables

    private ApplicationInfo _appInfo;

    private NexdoxEngine _engine;

    private IConFileService _conFileService;

    private IXmlFileService _xmlFileService;

    private IZipFileService _zipFileService;

    private IDocTypeService _docTypeService;

    private IManCoService _manCoService;

    #endregion

    #region Construction

    /// <summary>
    /// Default Constructor.
    /// </summary>
    /// <param name="appInfo">Application Info created by NexdoxLaunch and passed to the program on declaration.</param>
    public Process(ApplicationInfo appInfo, NexdoxEngine engine)
    {
      _appInfo = appInfo;
      _engine = engine;
    }

    #endregion

    #region Methods

    [Stage("Allocation Stage", 0, StreamTypesToAttach.None)]
    /// <summary>
    /// Allocation Stage
    /// </summary>
    public void AllocationStage()
    {
      //Start the nexdox messaging service
      NexdoxMessaging.StartEvent(this);

      //Initialise Statics
      Statics.Initialise(_engine, _appInfo);

      this.PerformIOC();

      /* Nexdox.PDF.NexdoxPDFSettings.PdfOuputRenderState = Nexdox.PDF.NexdoxPDFSettings.PDFState.ForceRasterise;
       Nexdox.PDF.NexdoxPDFSettings.JpgImageQuality = 85;
       Nexdox.PDF.NexdoxPDFSettings.DefaultDPI = 450;
       Nexdox.PDF.NexdoxPDFSettings.DefaultCompressionQuality = 125;*/

      //Allocation Stage
      Allocation allocationStage = new Allocation(
        _engine, _appInfo, _conFileService, _xmlFileService, _zipFileService, _docTypeService, _manCoService);
      allocationStage.Process();

      //Stop the nexdox messaging service
      NexdoxMessaging.EndEvent(this);
    }

    private void PerformIOC()
    {
      IoCContainer.ResoloveDependencies();

      _conFileService = IoCContainer.Resolve<IConFileService>();
      _zipFileService = IoCContainer.Resolve<IZipFileService>();
      _xmlFileService = IoCContainer.Resolve<IXmlFileService>();
      _docTypeService = IoCContainer.Resolve<IDocTypeService>();
      _manCoService = IoCContainer.Resolve<IManCoService>();
    }

    #endregion
  }
}
