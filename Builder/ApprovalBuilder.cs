namespace Builder
{
  using Entities;

  public class ApprovalBuilder : Builder<Approval>
  {
    public ApprovalBuilder()
    {
      Instance = new Approval();
    }

    public ApprovalBuilder WithDocument(Document document)
    {
      Instance.Document = document;
      return this;
    }
  }
}
