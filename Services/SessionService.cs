// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
  using System;
  using System.Collections.Generic;

  using Entities;

  using Exceptions;

  using ServiceInterfaces;

  using UnityRepository.Interfaces;

  public class SessionService : ISessionService
  {
    private readonly ISessionRepository _sessionRepository;

    public SessionService(ISessionRepository sessionRepository)
    {
      _sessionRepository = sessionRepository;
    }

    public void Update(string guid, DateTime end)
    {
      try
      {
        _sessionRepository.Update(guid, end);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update session", e);
      }
    }

    public IList<Session> GetSessionsByGovReadOnlyAdmin(DateTime startDate, int domicileId)
    {
      try
      {
        return _sessionRepository.GetSessionsByGovReadOnlyAdmin(startDate, domicileId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get sessions", e);
      }
    }
  }
}
