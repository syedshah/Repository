namespace NTGEN94
{
  using System.Collections.Generic;

  public class OtfRecord
  {
    public OtfRecord()
    {
      Emails = new List<string>();
    }

    public OtfRecord(string manCo, string accountNumber, string docType)
    {
      this.ManCo = manCo;
      this.AccountNumber = accountNumber;
      this.DocType = docType;
      Emails = new List<string>();
    }

    public string ManCo { get; set; }

    public string AccountNumber { get; set; }

    public string DocType { get; set; }

    public List<string> Emails { get; set; }
  }
}
