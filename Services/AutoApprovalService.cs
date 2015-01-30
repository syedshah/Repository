namespace Services
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class AutoApprovalService : IAutoApprovalService
  {
    private readonly IAutoApprovalRepository _autoAprovalRepository;

    public AutoApprovalService(IAutoApprovalRepository autoApprovalRepository)
    {
      this._autoAprovalRepository = autoApprovalRepository;
    }

    public AutoApproval GetAutoApproval(int manCoId, int docTypeId, int subDocTypeId)
    {
      try
      {
        return this._autoAprovalRepository.GetAutoApproval(manCoId, docTypeId, subDocTypeId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve approval status", e);
      }
    }

    public AutoApproval GetAutoApproval(int autoApprovalId)
    {
      try
      {
        return this._autoAprovalRepository.GetAutoApproval(autoApprovalId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to get auto approval", e);
      }
    }

    public IList<AutoApproval> GetAutoApprovals()
    {
      try
      {
        return this._autoAprovalRepository.GetAutoApprovals();
      }
      catch (Exception e)
      {

        throw new UnityException("Unable to retrieve document approvals", e);
      }
    }

    public IList<AutoApproval> GetAutoApprovals(int manCoId)
    {
      try
      {
        return this._autoAprovalRepository.GetAutoApprovals(manCoId);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve document approvals", e);
      }
    }

    public IList<AutoApproval> GetAutoApprovals(string docTypCode)
    {
      try
      {
        return this._autoAprovalRepository.GetAutoApprovals(docTypCode);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to retrieve document approvals", e);
      }
    }

    public void AddAutoApproval(int manCoId, int docTypeId, int subDocTypeId)
    {
      var newAutoApproval = new AutoApproval
      {
        ManCoId = manCoId,
        DocTypeId = docTypeId,
        SubDocTypeId = subDocTypeId
      };

      try
      {
        AutoApproval autoApproval = _autoAprovalRepository.GetAutoApproval(manCoId, docTypeId, subDocTypeId);

        if (autoApproval == null)
        {
          this._autoAprovalRepository.Create(newAutoApproval);
          return;
        }
        throw new UnityAutoApprovalAlreadyExistsException("Unable to add auto approval. The auto approval already exists in the database");
      }
      catch (UnityAutoApprovalAlreadyExistsException)
      {
        throw;
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to create document approval", e);
      }
    }

    public void Update(int autoApprovalId, int manCoId, int docTypeId, int subDocTypeId)
    {
      try
      {
        AutoApproval autoApproval = _autoAprovalRepository.GetAutoApproval(manCoId, docTypeId, subDocTypeId);

        if (autoApproval == null)
        {
          this._autoAprovalRepository.Update(autoApprovalId, manCoId, docTypeId, subDocTypeId);
          return;
        }
        throw new UnityAutoApprovalAlreadyExistsException("Unable to update auto approval. The auto approval already exists in the database");
      }
      catch (UnityAutoApprovalAlreadyExistsException)
      {
        throw;
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to update document approval", e);
      }
    }

    public void Delete(string docTypeCode)
    {
      try
      {
        foreach (var autoApproval in GetAutoApprovals(docTypeCode))
        {
          Delete(autoApproval.Id);
        }
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to delete auto approval by type code", e);
      }
    }

    public void Delete(int autoApprovalId)
    {
      try
      {
        AutoApproval autoApproval = _autoAprovalRepository.GetAutoApproval(autoApprovalId);
        _autoAprovalRepository.Delete(autoApproval);
      }
      catch (Exception e)
      {
        throw new UnityException("Unable to delete auto approval", e);
      }
    }
  }
}
