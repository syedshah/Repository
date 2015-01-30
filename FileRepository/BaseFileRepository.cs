namespace FileRepository
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using SystemFileAdapter;
  using Exceptions;
  using FileSystemInterfaces;
  using Repository;

  public abstract class BaseFileRepository // : IRepository<SystemIoFileInfo>
  {
    protected readonly string Path;

    protected readonly IFileInfoFactory FileInfoFactory;

    protected readonly IDirectoryInfo DirectoryInfo;

    protected BaseFileRepository(string path, IFileInfoFactory fileInfo, IDirectoryInfo directoryInfo)
    {
      Path = path;
      FileInfoFactory = fileInfo;
      DirectoryInfo = directoryInfo;
    }

    public IEnumerable<FileInfo> Entities
    {
      get
      {
        return DirectoryInfo.EnumerateFiles(Path, "*.*");
      }
    }

    public byte[] Create(byte[] entity)
    {
      var fileName = GenerateFileName();
      try
      {
        IFileInfo fileInfo = FileInfoFactory.CreateFileInfo(fileName);

        using (var stream = fileInfo.Create())
        {
          stream.Write(entity, 0, entity.Length);
        }
        return entity;
      }
      catch (IOException)
      {
        throw new RepositoryException(string.Format("Unable to create entry with filename: {0}", fileName));
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create entity", e);
      }
    }

    public void Delete(string fileName)
    {
      if (File.Exists(fileName))
      {
        File.Delete(fileName);
      }
    }

    protected abstract string GenerateFileName();

    public void Dispose()
    {
      
    }
  }
}
