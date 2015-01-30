// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Session.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Entity that stores user sessions
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Session
  {
    public Int32 Id { get; set; }

    public DateTime Start { get; set; }

    public DateTime? End { get; set; }

    public String Guid { get; set; }
    
    [ForeignKey("ApplicationUser")]
    public virtual string UserId { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }
  }
}
