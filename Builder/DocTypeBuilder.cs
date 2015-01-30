using Entities;

namespace Builder
{
  public class DocTypeBuilder : Builder<DocType>
  {
    public DocTypeBuilder()
    {
      Instance = new DocType();
    }
  }
}
