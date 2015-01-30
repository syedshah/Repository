namespace Entities
{
  using System;

  public class PasswordHistory
  {
    public int Id { get; set; }

    public string PasswordHash { get; set; }

    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    public virtual DateTime LogDate { get; set; }
  }
}
