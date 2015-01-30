namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class SubDocTypeService : ISubDocTypeService
  {
    private readonly ISubDocTypeRepository _subDocTypeRepository;

    public SubDocTypeService(ISubDocTypeRepository subDocTypeRepository)
    {
      _subDocTypeRepository = subDocTypeRepository;
    }

    public SubDocType GetSubDocType(int id)
    {
      try
      {
        return _subDocTypeRepository.GetSubDocType(id);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve sub doc type", e);
      }
    }

    public IList<SubDocType> GetSubDocTypes()
    {
      try
      {
        return _subDocTypeRepository.GetSubDocTypes();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve sub doc types", e);
      }
    }

    public IList<SubDocType> GetSubDocTypes(int docTypeId)
    {
      try
      {
        return _subDocTypeRepository.GetSubDocTypes(docTypeId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve sub doc types", e);
      }
    }

    public void Update(int subDocTypeId, string code, string description)
    {
      try
      {
        _subDocTypeRepository.Update(subDocTypeId, code, description);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update sub doc type", e);
      }
    }

    public SubDocType GetSubDocType(string code)
    {
      try
      {
        return _subDocTypeRepository.GetSubDocType(code);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve sub doc type", e);
      }
    }
  }
}
