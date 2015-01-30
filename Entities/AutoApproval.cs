namespace Entities
{
  public class AutoApproval
  {
    public int Id { get; set; }

    public int ManCoId { get; set; }

    public virtual ManCo Manco { get; set; }

    public int DocTypeId { get; set; }

    public virtual DocType DocType { get; set; }

    public int SubDocTypeId { get; set; }

    public virtual SubDocType SubDocType { get; set; }    
  }
}
