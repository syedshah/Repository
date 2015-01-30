namespace BusinessEngineInterfaces
{
  public interface ICartItemEngine
  {
    void AddItem(string documentId, string cartId, string userName, string manCo, string docType, string subDocType);
  }
}
