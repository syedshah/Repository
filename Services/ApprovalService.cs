namespace Services
{
  using System;
  using BusinessEngineInterfaces;
  using Exceptions;
  using ServiceInterfaces;

  public class ApprovalService : IApprovalService
  {
    private readonly IApprovalEngine _approvalEngine;

    public ApprovalService(IApprovalEngine approvalEngine)
    {
      _approvalEngine = approvalEngine;
    }

    public void ApproveDocument(string userName, string documentId, string manCo, string docType, string subDocType)
    {
      try
      {
        _approvalEngine.ApproveDocument(userName, documentId, manCo, docType, subDocType);
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
