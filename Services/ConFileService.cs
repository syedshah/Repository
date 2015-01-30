namespace Services
{
  using System;
  using Entities.File;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class ConFileService : IConFileService
  {
    private readonly IConFileRepository _conFileRepository;

    public ConFileService(IConFileRepository conFileRepository)
    {
      _conFileRepository = conFileRepository;
    }

    public void CreateConFile(string documentSetId, string fileName, string parentFileName, DateTime received)
    {
      try
      {
        ConFile conFile = new ConFile(documentSetId, fileName, string.Empty, received);
        _conFileRepository.Create(conFile);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create con file", e);
      }
    }
  }
}
