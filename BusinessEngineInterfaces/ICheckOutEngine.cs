namespace BusinessEngineInterfaces
{
  using Entities;

  public interface ICheckOutEngine
  {
    bool IsDocumentCheckedOut(string documentId);
    CheckOut CheckOutDocument(string userName, string documentId, string manCo, string docType, string subDocType);
  }
}
