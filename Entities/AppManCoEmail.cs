namespace Entities
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;

  public class AppManCoEmail
  {
    public AppManCoEmail()
    {
      this.AppManCoEmailHistorys = new List<AppManCoEmailHistory>();
    }

    public AppManCoEmail(int appId, int manCoId, int docTypeId, string accountNumber, string email) : this()
    {
      this.ApplicationId = appId;
      this.ManCoId = manCoId;
      this.DocTypeId = docTypeId;
      this.AccountNumber = accountNumber;
      this.Email = email;
    }

    public int Id { get; set; }
      
    public int ApplicationId { get; set; }

    public Application Application { get; set; }

    public int ManCoId { get; set; }

    public ManCo ManCo { get; set; }

    public int DocTypeId { get; set; }

    public DocType DocType { get; set; }

    public string AccountNumber { get; set; }

    public string Email { get; set; }

    public virtual ICollection<AppManCoEmailHistory> AppManCoEmailHistorys { get; set; }
  }
}
