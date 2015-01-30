// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISessionRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UnityRepository.Interfaces
{
  using System;

  using System.Collections.Generic;

  using Entities;

  using Repository;

  public interface ISessionRepository : IRepository<Session>
  {
    Session GetSession(string guid);

    IList<Session> GetSessionsByGovReadOnlyAdmin(DateTime startDate, int domicileId);

    void Update(string guid, DateTime end);
  }
}
