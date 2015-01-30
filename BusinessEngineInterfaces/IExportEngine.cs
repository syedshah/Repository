namespace BusinessEngineInterfaces
{
  using System.Collections.Generic;

  public interface IExportEngine
  {
    string SaveAsZip(List<string> documentIds);
  }
}
