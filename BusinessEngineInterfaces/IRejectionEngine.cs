namespace BusinessEngineInterfaces
{
  public interface IRejectionEngine
  {
    void RejectDocument(
      string userName,
      string documentId,
      string manCo,
      string docType,
      string subDocType);
  }
}
