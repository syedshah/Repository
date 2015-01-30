namespace ServiceInterfaces
{
  using System.Collections.Generic;

  public interface IExportFileService
  {
    string ExportToZip(IList<string> documentIds);
  }
}
