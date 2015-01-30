namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities.File;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class ZipFileService : IZipFileService
  {
    private readonly IZipFileRepository _zipFileRepository;

    public ZipFileService(IZipFileRepository zipFileRepository)
    {
      _zipFileRepository = zipFileRepository;
    }

    public void CreateBigZipFile(string documentSetId, string fileName, DateTime received)
    {
      try
      {
        var zipFile = new ZipFile(documentSetId, fileName, string.Empty, true, received);
        _zipFileRepository.Create(zipFile);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create big zip", e);
      }
    }

    public void CreateLittleZipFile(string documentSetId, string fileName, string parentFileName, DateTime received)
    {
      try
      {
        ZipFile zipFile = new ZipFile(documentSetId, fileName, parentFileName, false, received);
        _zipFileRepository.Create(zipFile);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create big zip", e);
      }
    }

    public IList<ZipFile> SearchBigZip(string fileName)
    {
      try
      {
        return this._zipFileRepository.SearchBigZip(fileName);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve big zip", e);
      }
    }

    public IList<ZipFile> SearchBigZip(string fileName, List<int> manCoIds)
    {
      try
      {
          return this._zipFileRepository.SearchBigZip(fileName, manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve big zip", e);
      }
    }
  }
}
