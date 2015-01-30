namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class ManCoService : IManCoService
  {
    private readonly IManCoRepository _manCoRepository;

    public ManCoService(IManCoRepository manCoRepository)
    {
      _manCoRepository = manCoRepository;
    }

    public ManCo CreateManCo(string code, string description)
    {
      try
      {
        var manCo = new ManCo(code, description);
        return _manCoRepository.Create(manCo);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create man co", e);
      }
    }

    public ManCo GetManCo(int id)
    {
      try
      {
        return _manCoRepository.GetManCo(id);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve man co", e);
      }
    }

    public ManCo GetManCo(string code)
    {
      try
      {
        return _manCoRepository.GetManCo(code);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve man co", e);
      }
    }

    public IList<ManCo> GetManCos()
    {
      try
      {
        return _manCoRepository.GetManCos();
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve man cos", e);
      }
    }

    public void Update(int manCoId, string code, string description)
    {
      try
      {
        _manCoRepository.Update(manCoId, code, description);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update man co", e);
      }
    }


    public IEnumerable<ManCo> GetManCos(int domicileId)
    {
      try
      {
        return this._manCoRepository.GetManCos(domicileId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve mancoes", e);
      }
    }

    public IList<ManCo> GetManCosByUserId(string userId)
    {
      try
      {
        return this._manCoRepository.GetManCosByUserId(userId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve man cos", e);
      }
    }
  }
}
