namespace Builder
{
  using Entities;

  public class AutoApprovalBuilder : Builder<AutoApproval>
  {
    public AutoApprovalBuilder()
    {
      Instance = new AutoApproval();
    }

    public AutoApprovalBuilder WithDocType(DocType docType)
    {
      Instance.DocType = docType;
      return this;
    }

    public AutoApprovalBuilder WithSubDocType(SubDocType subDocType)
    {
      Instance.SubDocType = subDocType;
      return this;
    }

    public AutoApprovalBuilder WithManCo(ManCo manCo)
    {
      Instance.Manco = manCo;
      return this;
    }
  }
}
