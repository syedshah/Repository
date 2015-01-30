using System;
using System.Collections;
using System.Collections.Generic;
using Nexdox.Composer;
using Nexdox.IO;
using System.IO;
using Nexdox.Collections;
using Nexdox.StringTools;
using NTGEN00;

namespace ntgen99
{
  /// <summary>
  /// Class for holding static information accessible from all the classes in the application. 
  /// </summary>
  static class Statics
  {
    #region Member Variables

    static public bool OverwriteDataFiles;
    public static int HugeFileSplitThreshold;
    public static string CntFilesRoutingMode;
    static public string UnitySQLServer;

    static public NexdoxResourceManager Resources;
    static public NexdoxResourceManager CentralResources;
    static public ManCoSettingsCollection ManagementCompanySettings;
    static public List<string> AppsToIgnore;
    static public ZipPackageCollection zipPackage;

    #endregion

    #region Methods

    /// <summary>
    /// Initialise static variables.
    /// </summary>
    /// <param name="engine">Nexdox Engine created by NexdoxLaunch and passed to the program on declaration.</param>
    /// <param name="appInfo">Application Info created by NexdoxLaunch and passed to the program on declaration.</param>
    static public void Initialise(NexdoxEngine engine, ApplicationInfo appInfo)
    {
        OverwriteDataFiles = bool.Parse(appInfo["OverwriteDataFiles"]);
        HugeFileSplitThreshold = Int32.Parse(appInfo["HugeFileSplitThreshold"]) * 1000;
        CntFilesRoutingMode = appInfo["CntFilesRoutingMode"];
        UnitySQLServer = appInfo["UnitySQLServer"];
        //Resource Manager
        NexdoxResourceManager.Mode processingMode = NexdoxResourceManager.Mode.ProcessingNoDamConnection;
        switch (appInfo["DAMConnection"])
        {
            case "Compulsory":
                processingMode = NexdoxResourceManager.Mode.ProcessingCompulsoryDamConnection;
                break;
            case "Optional":
                processingMode = NexdoxResourceManager.Mode.ProcessingOptionalDamConnection;
                break;
        }

        string region = appInfo["Region"];
        NexdoxMessaging.SendMessage(string.Format("Setting up region: {0} resource manager", region), false, null);
        Resources = new NexdoxResourceManager(appInfo.ApplicationID, new DirectoryInfo(appInfo.ResourcePath), engine, processingMode, region);
        Resources.ResourceManagerAsset.GetLatestFromDam();
        CentralResources = Resources.ExternalApps[0];
        CentralResources.ResourceManagerAsset.GetLatestFromDam();

        //Read ManCoSettings from RMC
        ManagementCompanySettings = new ManCoSettingsCollection(CentralResources, NexdoxSettings.FontCollection, true);

        AppsToIgnore = new List<string>();

        //These applications will not be allocated data, the datafiles will be ignored
        FieldList ignoreApps = new FieldList(appInfo["AppsToIgnore"], ",", "\"", FieldList.SplitMethod.MicrosoftExcel);

        foreach (string app in ignoreApps)
            AppsToIgnore.Add(app.ToUpper());

        zipPackage = new ZipPackageCollection();

        //NTUtils.Statics.SharedFolderPath = appInfo["SharedResourceFolder"];
    }

    #endregion
  }
}
