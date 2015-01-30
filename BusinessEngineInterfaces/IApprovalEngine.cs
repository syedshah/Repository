namespace BusinessEngineInterfaces
{
  public interface IApprovalEngine
  {
    void AutoApproveDocument(string documentId);

    void ApproveDocument(
      string userName,
      string documentId,
      string manCo,
      string docType,
      string subDocType);
  }
}
