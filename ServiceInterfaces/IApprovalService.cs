namespace ServiceInterfaces
{
  public interface IApprovalService
  {
    void ApproveDocument(string userName, string documentId, string manCo, string docType, string subDocType);
  }
}
