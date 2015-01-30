namespace ServiceInterfaces
{
  using System.Collections.Generic;
  using Entities;

  public interface IExportService
  {
    void CreateExport(List<Export> exports);
  }
}
