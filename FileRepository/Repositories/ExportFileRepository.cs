namespace FileRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using SystemFileAdapter;
  using Entities;
  using FileRepository;
  using FileRepository.Interfaces;
  using FileSystemInterfaces;

  public class ExportFileRepository : BaseFileRepository, IExportFileRepository
  {
    public ExportFileRepository(string path, IFileInfoFactory fileInfo, IDirectoryInfo directoryInfo)
      : base(path + "/exportFiles", fileInfo, directoryInfo)
    {

    }

    private string _fileId;

    private string _fileName;

    protected override string GenerateFileName()
    {
      var fileName = string.Format("{0}/{1}.pdf", Path, _fileId);
        _fileName = fileName;

      return fileName;
    }

    public ExportFile Create(ExportFile file)
    {
       _fileId = file.Id.ToString();
       base.Create(file.FileData);
       file.FileName = _fileName;

       return file;
    }

    public void Delete(string fileName)
    {
       base.Delete(fileName);
    }

    public IList<string> GetExportFileNames()
    {
      var fileInfos = base.Entities.ToList();

      return fileInfos.Select(x => x.FullName.Replace(x.DirectoryName, "").Substring(1)).ToList();
    }
  }
}
