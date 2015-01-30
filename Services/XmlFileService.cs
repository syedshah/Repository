namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities.File;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class XmlFileService : IXmlFileService
  {
    private readonly IXmlFileRepository _xmlFileRepository;

    public XmlFileService(IXmlFileRepository xmlFileRepository)
    {
      _xmlFileRepository = xmlFileRepository;
    }

    public XmlFile GetFile(string fileName)
    {
      try
      {
        return _xmlFileRepository.GetFile(fileName);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve xml file");
      }
    }

    public void CreateXmlFile(
      string documentSetId,
      string fileName,
      string parentFileName,
      bool offShore,
      int docTypeId,
      int manCoId,
      int status,
      string domicleId,
      string bigZip,
      DateTime allocated,
      string allocatorGrid,
      DateTime received)
    {
      try
      {
        var xmlFile = new XmlFile(
          documentSetId,
          fileName,
          parentFileName,
          offShore,
          docTypeId,
          manCoId,
          domicleId,
          bigZip,
          allocated,
          allocatorGrid,
          received);
        _xmlFileRepository.Create(xmlFile);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create XML file", e);
      }
    }

    public IList<XmlFile> Search(string fileName)
    {
      try
      {
        return _xmlFileRepository.Search(fileName);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to search for xml files", e);
      }
    }

    public IList<XmlFile> Search(string fileName, List<int> manCoIds)
    {
      try
      {
        return _xmlFileRepository.Search(fileName, manCoIds);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to search for xml files", e);
      }
    }
  }
}
