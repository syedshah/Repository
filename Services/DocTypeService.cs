namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class DocTypeService : IDocTypeService
  {
    private readonly IDocTypeRepository _docTypeRepository;

    public DocTypeService(IDocTypeRepository docTypeRepository)
    {
      _docTypeRepository = docTypeRepository;
    }

    public DocType CreateDocType(string code, string description)
    {
      try
      {
        var docType = new DocType(code, description);
        return _docTypeRepository.Create(docType);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create doc type", e);
      }
    }

    public void AddSubDocType(int docTypeId, string code, string description)
    {
      try
      {
        _docTypeRepository.AddSubDocType(docTypeId, code, description);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create sub doc type", e);
      }
    }

    public DocType GetDocType(int id)
    {
      try
      {
        return _docTypeRepository.GetDocType(id);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve doc type", e);
      }
    }

    public DocType GetDocType(string code)
    {
      try
      {
        return _docTypeRepository.GetDocType(code);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve doc type", e);
      }
    }

    public IList<DocType> GetDocTypes()
    {
      try
      {
        return _docTypeRepository.GetDocTypes();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve doc types", e);
      }
    }

    public void Update(int docTypeId, string code, string description)
    {
      try
      {
        _docTypeRepository.Update(docTypeId, code, description);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update doc type", e);
      }
    }
  }
}
