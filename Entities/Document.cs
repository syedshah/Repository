namespace Entities
{
  public class Document
  {
    public Document()
    {
    }

    public Document(string mailPrintFlag)
    {
      MailPrintFlag = mailPrintFlag == string.Empty ? "Y" : mailPrintFlag;
    }

    public int Id { get; set; }

    public string DocumentId { get; set; }

    public int DocTypeId { get; set; }

    public virtual DocType DocType { get; set; }

    public int SubDocTypeId { get; set; }

    public virtual SubDocType SubDocType { get; set; }

    public int ManCoId { get; set; }

    public virtual ManCo ManCo { get; set; }

    public int? GridRunId { get; set; }

    public GridRun GridRun { get; set; }

    public virtual CheckOut CheckOut { get; set; }

    public Approval Approval { get; set; }

    public Rejection Rejection { get; set; }

    public Export Export { get; set; }

    public int? HouseHoldingRunId { get; set; }

    public HouseHoldingRun HouseHoldingRun { get; set; }

    public HouseHold HouseHold { get; set; }

    public string MailingName { get; set; }

    public string InvestorReference { get; set; }

    public string MailPrintFlag { get; private set; }

    public void UpdateDocument(int houseHoldingRunId)
    {
      HouseHoldingRunId = houseHoldingRunId;
    }
  }
}
