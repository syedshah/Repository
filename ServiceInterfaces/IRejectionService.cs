namespace ServiceInterfaces
{
  public interface IRejectionService
  {
    void RejectDocument(string userName, string documentId, string manCo, string docType, string subDocType);
  }
}
