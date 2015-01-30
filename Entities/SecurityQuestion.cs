namespace Entities
{
  using System;

  public class SecurityQuestion
  {
    public SecurityQuestion()
    {
      this.Id = Guid.NewGuid().ToString();  
    }

    public SecurityQuestion(string question)
    {
      this.Id = Guid.NewGuid().ToString();
      this.Question = question;
    }

    public string Id { get; set; }

    public string Question { get; set; }
  }
}
