namespace Entities
{
  using System;
  using Encryptions;

  public class SecurityAnswer
  {
    public SecurityAnswer()
    {
      this.Id = Guid.NewGuid().ToString();
    }

    public SecurityAnswer(string id)
    {
      this.Id = id;
    }

    public SecurityAnswer(string answer, string securityQuestionId)
    {
      this.Id = Guid.NewGuid().ToString();
      this.Answer = UnityEncryption.Encrypt(answer);
      this.SecurityQuestionId = securityQuestionId;
    }

    public SecurityAnswer(string answer, string securityQuestionId, string userId)
    {
      this.Id = Guid.NewGuid().ToString();
      this.Answer = UnityEncryption.Encrypt(answer); 
      this.SecurityQuestionId = securityQuestionId;
      this.UserId = userId;
    }

    public string Id { get; set; }

    public string Answer { get; set; }

    public string SecurityQuestionId { get; set; }

    public virtual SecurityQuestion SecurityQuestion { get; set; }

    public string UserId { get; set; }

    public virtual ApplicationUser User { get; set; }
  }
}
