namespace ZipManagerWrapper
{
  using Xceed.Zip;

  public class ZipManager : IZipManager
  {
    public ZipManager()
    {
      Licenser.LicenseKey = "ZIN37-T1W1B-SW87P-N8AA";     
    }
       
    public void Zip(string zipFile, string[] files)
    {
      QuickZip.Zip(zipFile, true, true, false, files);
    }
  }
}
