using Entities;

namespace Builder
{
  public class SubDocTypeBuilder : Builder<SubDocType>
  {
    public SubDocTypeBuilder()
    {
      Instance = new SubDocType();
    }

    public SubDocTypeBuilder WithDocType(DocType docType)
    {
      Instance.DocType = docType;
      return this;
    }
  }
}
