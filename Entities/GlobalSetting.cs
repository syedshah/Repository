namespace Entities
{
  public class GlobalSetting
  {
    public int Id { get; set; }

    public int MinPasswordLength { get; set; }

    public int MinNonAlphaChars { get; set; }

    public int PasswordExpDays { get; set; }

    public int PassChangeBefore { get; set; }

    public bool NewUserPasswordReset { get; set; }

    public int MaxLogInAttempts { get; set; }
  }
}
