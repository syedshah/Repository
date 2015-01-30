namespace FileRepository.Interfaces
{
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IExportFileRepository //: IRepository<ExportFile>
  {
    ExportFile Create(ExportFile file);

    void Delete(string fileName);

    IList<string> GetExportFileNames();
  }
}
