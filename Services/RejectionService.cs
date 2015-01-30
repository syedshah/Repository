namespace Services
{
  using System;
  using BusinessEngineInterfaces;
  using Exceptions;
  using ServiceInterfaces;

  public class RejectionService : IRejectionService
  {
    private readonly IRejectionEngine _rejectionEngine;

    public RejectionService(IRejectionEngine rejectionEngine)
    {
      _rejectionEngine = rejectionEngine;
    }

    public void RejectDocument(string userName, string documentId, string manCo, string docType, string subDocType)
    {
      try
      {
        _rejectionEngine.RejectDocument(userName, documentId, manCo, docType, subDocType);
      }
      catch (DocumentAlreadyApprovedException e)
      {
        throw new DocumentAlreadyApprovedException("Document is already approved");
      }
      catch (DocumentAlreadyRejectedException e)
      {
        throw new DocumentAlreadyRejectedException("Document is already rejected");
      }
      catch (Exception e)
      {
        throw new UnityException(string.Format("Unable to approve document guid {0}", documentId), e);
      }
    }
  }
}
