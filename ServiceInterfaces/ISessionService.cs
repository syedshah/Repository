// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISessionService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Page for Session Interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;

  using System.Collections.Generic;

  using Entities;

  public interface ISessionService
  {
    void Update(string guid, DateTime end);

    IList<Session> GetSessionsByGovReadOnlyAdmin(DateTime startDate, int domicileId);
  }
}
